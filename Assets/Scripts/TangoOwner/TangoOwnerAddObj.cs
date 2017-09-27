using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Tango;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Lean.Touch;
using UnityEngine.Experimental.Networking;
using System.Web.Script.Serialization;

/// <summary>
/// AreaLearningGUIController is responsible for the main game interaction.
/// 
/// This class also takes care of loading / save persistent data(marker), and loop closure handling.
/// </summary>
public class TangoOwnerAddObj : MonoBehaviour, ITangoPose, ITangoEvent, ITangoDepth, ITangoLifecycle
{
    public GameObject[] m_objPrefabs;   
    [HideInInspector]
    public AreaDescription m_curAreaDescription;

    //for store obj
    private List<GameObject> m_objList = new List<GameObject>();
    private GameObject newObj = null;
    private int m_currentObjType = 0;
    private ARStoreObject m_selectedObj;

    //other control
    private bool m_findPlaneWaitingForDepth;//If set, then the depth camera is on and we are waiting for the next depth update.
    private TangoPoseController m_poseController;
    private bool m_initialized = false;
    private TangoApplication m_tangoApplication;
    private string m_curAreaDescriptionUUID;
    //private Thread m_saveThread;

    //part of connecting
    private string getListURL = "https://kevin.imslab.org" + PlayerInfo.port + "/list_storage_hierarchy";
    private string createShopURL = "https://kevin.imslab.org" + PlayerInfo.port + "/create_shop";
    private string addObjURL = "https://kevin.imslab.org" + PlayerInfo.port + "/add_obj";
    private string auth_flag = "1";
    private string beaconID = "2";
    //private string adfID = OwnerInfo.curUUID;
    private string adfID;
    private string shopID = OwnerInfo.storeInfo._id;
    private string pw = OwnerInfo.ownerPW;    
    private bool hasCreated = false;    
    private byte[] contents = null;
    //private string[] listStorage;

    public class shop
    {
        public string name = "";
        public string _id = "";
    }
    public class adf
    {
        public string name = "";
        public string _id = "";
        public List<shop> shopID = new List<shop>();
    }
    public class listStorage
    {
        public string _id = "";
        public string beaconID = "";
        public List<adf> adfID = new List<adf>();
    }
    public List<listStorage> theListStorage = new List<listStorage>(); 

    /// <summary>
    /// Unity Start function.
    /// 
    /// We find and assign pose controller and tango application, and register this class to callback events.
    /// </summary>
    public void Start()
    {
        //m_modeController = FindObjectOfType<ButtonGroupToggle>();
        m_poseController = FindObjectOfType<TangoPoseController>();
        m_tangoApplication = FindObjectOfType<TangoApplication>();

        if (m_tangoApplication != null)
        {
            m_tangoApplication.Register(this);
            if (AndroidHelper.IsTangoCorePresent())
            {
                m_tangoApplication.RequestPermissions();
            }
        }

        if (shopID != null)
        {
            Debug.Log("shopID = " + shopID);
            StartCoroutine(createShop());
        }

        //Debug.Log("getting list start!!!");
        //StartCoroutine(getListStorage());         
    }    

    /// <summary>
    /// add an obj in markerList, and upload into the world with the upload function
    /// </summary>
    public void createObj()
    {
        //check that create obj after relocalize
        if(m_initialized)
        {            
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, m_poseController.transform.position.z + 1.5f);//Camera.main.nearClipPlane);
            Vector3 objPose = Camera.main.ScreenToWorldPoint(screenCenter);

            newObj = Instantiate(m_objPrefabs[m_currentObjType],
                                        objPose,
                                        //Camera.main.transform.rotation * Vector3.forward) as GameObject;
                                        Quaternion.identity) as GameObject;

            newObj.transform.GetChild(1);
            ARStoreObject objScript = newObj.GetComponent<ARStoreObject>();

            objScript.m_type = m_currentObjType;
            objScript.m_timestamp = (float)m_poseController.LastPoseTimestamp;
            objScript.m_storeName = OwnerInfo.storeInfo.shopName;
            for (int i = 0; i < OwnerInfo.storeInfo.infoList.Count; i++)
            {
                if (OwnerInfo.storeInfo.infoList[i].title == "店家介紹")
                {
                    objScript.m_storeIntro = OwnerInfo.storeInfo.infoList[i].content;
                }
            }
            
            Matrix4x4 uwTDevice = Matrix4x4.TRS(m_poseController.transform.position,
                                                m_poseController.transform.rotation,
                                                Vector3.one);
            Matrix4x4 uwTObj = Matrix4x4.TRS(newObj.transform.position,
                                                newObj.transform.rotation,
                                                Vector3.one);
            objScript.m_deviceTObj = Matrix4x4.Inverse(uwTDevice) * uwTObj;

            m_objList.Add(newObj);

            //m_selectedObj = newObj;
        }

    }

    //need tap to get which obj??
    public void deleteObj()
    {
        foreach(GameObject obj in m_objList)
        {
            //GameObject tmpObj = obj;
            //tmpObj = GameObject.FindGameObjectWithTag("cube_storeInfo");
            m_objList.Remove(obj);
            Destroy(obj);
        }        
    }

    /// <summary>
    /// Unity Update function.
    /// 
    /// Mainly handle the touch event and place mark in place.
    /// </summary>
    public void Update()
    {        
        if (Input.GetKey(KeyCode.Escape))
        {
#pragma warning disable 618
            Application.LoadLevel(Application.loadedLevel);
#pragma warning restore 618
        }

        if (!m_initialized)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject(0) || GUIUtility.hotControl != 0)
        {
            return;
        }        
    }

    /// <summary>
    /// Application onPause / onResume callback.
    /// </summary>
    /// <param name="pauseStatus"><c>true</c> if the application about to pause, otherwise <c>false</c>.</param>
    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && m_initialized)
        {
            // When application is backgrounded, we reload the level because the Tango Service is disconected. All
            // learned area and placed marker should be discarded as they are not saved.
#pragma warning disable 618
            Application.LoadLevel(Application.loadedLevel);
#pragma warning restore 618
        }
    }
       
    /// <summary>
    /// This is called each time a Tango event happens.
    /// </summary>
    /// <param name="tangoEvent">Tango event.</param>
    public void OnTangoEventAvailableEventHandler(Tango.TangoEvent tangoEvent)
    {
        // We will not have the saving progress when the learning mode is off.
        if (!m_tangoApplication.m_areaDescriptionLearningMode)
        {
            return;
        }

        if (tangoEvent.type == TangoEnums.TangoEventType.TANGO_EVENT_AREA_LEARNING
            && tangoEvent.event_key == "AreaDescriptionSaveProgress")
        {
            //m_savingText.text = "Saving. " + (float.Parse(tangoEvent.event_value) * 100) + "%";
        }
    }

    /// <summary>
    /// OnTangoPoseAvailable event from Tango.
    /// 
    /// In this function, we only listen to the Start-Of-Service with respect to Area-Description frame pair. This pair
    /// indicates a relocalization or loop closure event happened, base on that, we either start the initialize the
    /// interaction or do a bundle adjustment for all marker position.
    /// </summary>
    /// <param name="poseData">Returned pose data from TangoService.</param>
    public void OnTangoPoseAvailable(Tango.TangoPoseData poseData)
    {
        if (poseData.framePair.baseFrame ==
            TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION &&
            poseData.framePair.targetFrame ==
            TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE &&
            poseData.status_code == TangoEnums.TangoPoseStatusType.TANGO_POSE_VALID)
        {
            // When we get the first loop closure/ relocalization event, we initialized all the in-game interactions.
            if (!m_initialized)
            {
                Debug.Log("startup sofa in addObj success!!!");
                m_initialized = true;
                if (m_curAreaDescription == null)
                {
                    Debug.Log("AndroidInGameController.OnTangoPoseAvailable(): m_curAreaDescription is null");
                    return;
                }

                //_LoadMarkerFromDisk();
            }
        }
    }

    /// <summary>
    /// This is called each time new depth data is available.
    /// 
    /// On the Tango tablet, the depth callback occurs at 5 Hz.
    /// </summary>
    /// <param name="tangoDepth">Tango depth.</param>
    public void OnTangoDepthAvailable(TangoUnityDepth tangoDepth)
    {
        // Don't handle depth here because the PointCloud may not have been updated yet.  Just
        // tell the coroutine it can continue.
        m_findPlaneWaitingForDepth = false;
    }
        
    /// <summary>
    /// Write marker list to an xml file stored in application storage.
    /// </summary>
    public void _SaveObj()
    {
        Debug.Log("hasCreate = " + hasCreated.ToString());
        if(hasCreated == true)
        {
            string prefabPath = Application.dataPath + "/" + "Canvas_storeInfo 1.prefab";            
            contents = File.ReadAllBytes(prefabPath);

            Debug.Log("start save");            
            //foreach(GameObject obj in m_objList)            
            StartCoroutine(sendingAddObj());
        }

        /*
        // Compose a XML data list.
        List<storeObjectData> xmlDataList = new List<storeObjectData>();
        foreach (GameObject obj in m_objList)
        {            
            // Add marks data to the list, we intentionally didn't add the timestamp, because the timestamp will not be
            // useful when the next time Tango Service is connected. The timestamp is only used for loop closure pose
            // correction in current Tango connection.
            storeObjectData temp = new storeObjectData();
            temp.m_type = obj.GetComponent<ARStoreObject>().m_type;
            temp.m_position = obj.transform.position;
            temp.m_orientation = obj.transform.rotation;
            temp.m_scale = obj.transform.localScale;

            Text nameText = obj.transform.GetChild(1).gameObject.GetComponent<Text>();
            temp.m_name = nameText.text;
            Text introText = obj.transform.GetChild(2).gameObject.GetComponent<Text>();
            temp.m_introduce = introText.text;            

            xmlDataList.Add(temp);
        }

        string path = Application.persistentDataPath + "/" + m_curAreaDescription.m_uuid + ".xml";
        var serializer = new XmlSerializer(typeof(List<storeObjectData>));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, xmlDataList);
        }
        */
    }

    IEnumerator getListStorage()
    {
        UnityWebRequest sending = UnityWebRequest.Get(getListURL);
        yield return sending.Send();
        
        if(sending.error != null)
        {
            Debug.Log("error below : ");
            Debug.Log(sending.error);
        }
        else
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            theListStorage = js.Deserialize<List<listStorage>>(sending.downloadHandler.text);
            //Debug.Log("js test : ");
            //Debug.Log(listStorage);

            Debug.Log("correct below : ");
            Debug.Log(sending.downloadHandler.text);
            Debug.Log("shopId = " + theListStorage[0].adfID[0].shopID[0]._id);
            shopID = theListStorage[0].adfID[0].shopID[0]._id;

            if (shopID != null)
            {
                //Debug.Log("shopID = " + shopID);
                Debug.Log("create shop start!!!!!");
                StartCoroutine(createShop());
            }
        }
    }

    IEnumerator createShop()
    {
        WWWForm formdata = new WWWForm();
        formdata.AddField("shopID", shopID);
        formdata.AddField("password", OwnerInfo.ownerPW);
        formdata.AddField("auth_flag", auth_flag);
        formdata.AddField("beaconID", beaconID);
        //formdata.AddField("adfID", m_curAreaDescription.m_uuid);
        //formdata.AddField("adfID", "f2953b36-b477-2fb9-81c5-1682a435250e");
        formdata.AddField("adfID", "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66");//sofa

        UnityWebRequest sending = UnityWebRequest.Post(createShopURL, formdata);
        yield return sending.Send();
        if(sending.error != null)
        {
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below : ");
            if(sending.downloadHandler.text == "success" || sending.downloadHandler.text == "success-total")
            {
                hasCreated = true;
                Debug.Log("success : " + sending.downloadHandler.text);
            }
            else if(sending.downloadHandler.text == "duplicate-shopID")
            {
                hasCreated = true;
                Debug.Log("success : " + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log("false : " + sending.downloadHandler.text);
            }
        }
    }

    IEnumerator sendingAddObj()
    {
        ARStoreObject objToAdd = FindObjectOfType<ARStoreObject>();

        WWWForm formdata = new WWWForm();
        formdata.AddField("beaconID", beaconID);
        //formdata.AddField("adfID", m_curAreaDescription.m_uuid);
        //formdata.AddField("adfID", "f2953b36-b477-2fb9-81c5-1682a435250e");
        formdata.AddField("adfID", "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66");//sofa
        formdata.AddField("shopID", shopID);
        formdata.AddField("password", OwnerInfo.ownerPW);
        formdata.AddField("shopName", objToAdd.m_storeName);//formdata.AddField("shopName", OwnerInfo.storeInfo.shopName);
        formdata.AddField("shopIntro", objToAdd.m_storeIntro);//formdata.AddField("shopIntro", OwnerInfo.storeInfo.infoList[i].content);
        formdata.AddField("id", objToAdd.m_type);
        formdata.AddField("pos", objToAdd.transform.position.ToString());
        formdata.AddField("rot", objToAdd.transform.rotation.ToString());
        formdata.AddField("scale", objToAdd.transform.localScale.ToString());
        formdata.AddBinaryData("file", contents);
        
        UnityWebRequest sending = UnityWebRequest.Post(addObjURL, formdata);
        yield return sending.Send();
        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "create" || sending.downloadHandler.text == "update")
            {
                Debug.Log(sending.downloadHandler.text);
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }

    }

    /// <summary>
    /// Convert a 3D bounding box represented by a <c>Bounds</c> object into a 2D 
    /// rectangle represented by a <c>Rect</c> object.
    /// </summary>
    /// <returns>The 2D rectangle in Screen coordinates.</returns>
    /// <param name="cam">Camera to use.</param>
    /// <param name="bounds">3D bounding box.</param>
    private Rect _WorldBoundsToScreen(Camera cam, Bounds bounds)
    {
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;
        Bounds screenBounds = new Bounds(cam.WorldToScreenPoint(center), Vector3.zero);

        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, +extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, +extents.y, -extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, -extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, -extents.y, -extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, +extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, +extents.y, -extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, -extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, -extents.y, -extents.z)));
        return Rect.MinMaxRect(screenBounds.min.x, screenBounds.min.y, screenBounds.max.x, screenBounds.max.y);
    }

    public void OnTangoPermissions(bool permissionsGranted)
    {
        if(permissionsGranted)
        {
            m_curAreaDescriptionUUID = "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66";//sofa
            //m_curAreaDescriptionUUID = "f2953b36-b477-2fb9-81c5-1682a435250e";//204
            //m_curAreaDescriptionUUID = "ff8c3413-ced8-28f7-9801-38627ec90271";//d24test4
            //m_curAreaDescriptionUUID = "ff8c341e-ced8-28f7-9898-6ef42a5060b6";//d24test5
            //m_curAreaDescriptionUUID = OwnerInfo.curUUID;
            AreaDescription areaDescription = AreaDescription.ForUUID(m_curAreaDescriptionUUID);
            m_curAreaDescription = areaDescription;
            m_tangoApplication.m_areaDescriptionLearningMode = false;//m_enableLearningToggle.isOn;
            m_tangoApplication.Startup(m_curAreaDescription);

            m_poseController.gameObject.SetActive(true);
        }
    }

    public void OnTangoServiceConnected()
    {
    }

    public void OnTangoServiceDisconnected()
    {
    }
    /*
    /// <summary>
    /// Data container for marker.
    /// 
    /// Used for serializing/deserializing marker to xml.
    /// </summary>
    [System.Serializable]
    public class storeObjectData
    {
        /// <summary>
        /// Marker's type.
        /// 
        /// Red, green or blue markers. In a real game scenario, this could be different game objects
        /// (e.g. banana, apple, watermelon, persimmons).
        /// </summary>
        [XmlElement("type")]
        public int m_type;

        /// <summary>
        /// Position of the this mark, with respect to the origin of the game world.
        /// </summary>
        [XmlElement("position")]
        public Vector3 m_position;

        /// <summary>
        /// Rotation of the this mark.
        /// </summary>
        [XmlElement("orientation")]
        public Quaternion m_orientation;

        /// <summary>
        /// Scale of this mark.
        /// </summary>
        [XmlElement("scale")]
        public Vector3 m_scale;

        /// <summary>
        /// name text
        /// </summary>
        [XmlElement("name")]
        public string m_name;

        /// <summary>
        /// introduce text
        /// </summary>
        [XmlElement("introduce")]
        public string m_introduce;
    }
    */
}
