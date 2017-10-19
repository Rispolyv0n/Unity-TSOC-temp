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
    public GameObject[] m_storeInfoPrefabs;   
    [HideInInspector]
    public AreaDescription m_curAreaDescription;

    //for store obj
    private List<GameObject> m_storeObjList = new List<GameObject>();
    private GameObject newObj = null;
    private int m_currentObjType = 0;
    private ARStoreObject m_selectedObj;

    //other control
    private bool m_findPlaneWaitingForDepth;//If set, then the depth camera is on and we are waiting for the next depth update.
    private TangoPoseController m_poseController;
    private bool m_initialized = false;
    private TangoApplication m_tangoApplication;

    //private string m_curAreaDescriptionUUID = OwnerInfo.curUUID;
    private string m_curAreaDescriptionUUID = "aa305c0a-fd20-2325-8ab0-27ae08db9a54";//lab1014
    //private string m_curAreaDescriptionUUID = "aa305c08-fd20-2325-8b83-7e6e47b0bacc";//65104
    //private string m_curAreaDescriptionUUID = "f5b2ca87-af86-2899-86f7-2789d3d1ce3d";//0929_2
    //private string m_curAreaDescriptionUUID = "f5b2ca84-af86-2899-84f3-ba48570d17b2";//0929
    //private string m_curAreaDescriptionUUID = "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66";//sofa
    //private string m_curAreaDescriptionUUID = "0c641a06-f1e7-4fe4-8f3a-cedc91df8035";//new sofa
    //private string m_curAreaDescriptionUUID = "f2953b36-b477-2fb9-81c5-1682a435250e";//204
    //private string m_curAreaDescriptionUUID = "e12e5a3c-5a09-29b9-98c6-7b3d6fd42737";//d24test6
    //private string m_curAreaDescriptionUUID = "ff8c341e-ced8-28f7-9898-6ef42a5060b6";//d24test5
    //private Thread m_saveThread;

    //part of connecting
    private string getListURL = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/list_storage_hierarchy";
    private string createShopURL = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/create_shop";
    private string addObjURL = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/add_obj";
    private string auth_flag = "1";
    private string beaconID = "1";
    //private string adfID = OwnerInfo.curUUID;
    private string adfID;
    //private string shopID = OwnerInfo.storeInfo._id;//???
    private string shopID = null;// = OwnerInfo.ownerID;
    private string pw = OwnerInfo.ownerPW;    
    private bool hasCreatedShop = false;
    private bool hasGetList = false;  
    private byte[] contents = { 0x00 };
    //private string[] listStorage;

    //to load store obj
    //public GameObject[] m_storeInfoPrefabs;//for store objects(now only one)
    //public List<GameObject> m_storeList = new List<GameObject>();//need load
    private bool hasCreatedObj = false;
    private string getStoreObjURL;
    //private string shopID;
    public string loadShopName;
    public string loadShopIntro;
    public Vector3 loadObjPos;
    public Quaternion loadObjRot;
    public Vector3 loadObjScale;

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
    
    public class columnItemofStoreObj
    {
        public string shopName;
        public string shopIntro;
        public string pos;
        public string rot;
        public string scale;
    }
    //public List<columnItemofStoreObj> storeObjList = new List<columnItemofStoreObj>();
    public columnItemofStoreObj storeObj = new columnItemofStoreObj();

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

        /*if (shopID != null)
        {
            Debug.Log("shopID = " + shopID);
            StartCoroutine(createShop());
        }*/

        //StartCoroutine(createShop());
        
            
    }    

    /// <summary>
    /// add an obj in markerList, and upload into the world with the upload function
    /// </summary>
    public void createObj()
    {
        //check that create obj after relocalize
        if(m_initialized)
        {                 
            if(hasCreatedObj == true)
            {
                Debug.Log("already has an obj");
            }
            else
            {
                Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, m_poseController.transform.position.z + 1.5f);//Camera.main.nearClipPlane);
                Vector3 objPose = Camera.main.ScreenToWorldPoint(screenCenter);

                newObj = Instantiate(m_storeInfoPrefabs[m_currentObjType],
                                            objPose,
                                            //Camera.main.transform.rotation * Vector3.forward) as GameObject;
                                            Quaternion.identity) as GameObject;
                /*(Quaternion)(objPose + Camera.main.transform.rotation * Vector3.forward),
Camera.main.transform.rotation * Vector3.up) as GameObject;*/

                //newObj.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                //Camera.main.transform.rotation * Vector3.up);
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

                m_storeObjList.Add(newObj);

                ButtonGroupToggle button = GameObject.FindObjectOfType<ButtonGroupToggle>();
                button.moveMode();
            }            

            //m_selectedObj = newObj;
        }

    }

    //need tap to get which obj??
    public void deleteObj()
    {
        foreach(GameObject obj in m_storeObjList)
        {
            //GameObject tmpObj = obj;
            //tmpObj = GameObject.FindGameObjectWithTag("cube_storeInfo");
            m_storeObjList.Remove(obj);
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
                if(hasGetList == false)
                {
                    hasGetList = true;
                    Debug.Log("getting list start!!!");
                    StartCoroutine(getListStorage());
                }
                
                //_LoadMarkerFromDisk();
            }
        }
    }

    IEnumerator getListStorage()
    {
        UnityWebRequest sending = UnityWebRequest.Get(getListURL);
        yield return sending.Send();

        if (sending.error != null)
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
            //Debug.Log("shopId = " + theListStorage[0].adfID[0].shopID[0].name);
            //shopID = theListStorage[0].adfID[0].shopID[0].name;

            if (sending.downloadHandler.text == "[]")
            {
                //Debug.Log("shopID = " + shopID);
                Debug.Log("create shop start!!!!!");
                StartCoroutine(createShop());
            }
            else
            {
                shopID = theListStorage[0].adfID[0].shopID[0].name;
                Debug.Log("load exist shop obj");
                hasCreatedObj = true;
                StartCoroutine(loadstoreInfo());
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
        //connect part
        
        Debug.Log("hasCreatedShop = " + hasCreatedShop.ToString());
        if(hasCreatedShop == true)
        {
            /*
            string prefabPath = Application.dataPath + "/" + "Canvas_storeInfo 1.prefab";            
            contents = File.ReadAllBytes(prefabPath);
            */
            Debug.Log("start save");            
            //foreach(GameObject obj in m_storeObjList)            
            StartCoroutine(sendingAddObj());
        }
        
        /*
        // Compose a XML data list.
        List<storeObjectData> xmlDataList = new List<storeObjectData>();
        foreach (GameObject obj in m_storeObjList)
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

    IEnumerator sendingAddObj()
    {
        ARStoreObject objToAdd = FindObjectOfType<ARStoreObject>();

        WWWForm formdata = new WWWForm();
        formdata.AddField("beaconID", beaconID);
        //formdata.AddField("adfID", OwnerInfo.curUUID);
        //formdata.AddField("adfID", m_curAreaDescription.m_uuid);
        //formdata.AddField("adfID", "f2953b36-b477-2fb9-81c5-1682a435250e");
        formdata.AddField("adfID", "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66");//sofa
        //formdata.AddField("adfID", "f5b2ca84-af86-2899-84f3-ba48570d17b2");//0929
        //formdata.AddField("adfID", "aa305c08-fd20-2325-8b83-7e6e47b0bacc");//65104
        //formdata.AddField("adfID", "aa305c0a-fd20-2325-8ab0-27ae08db9a54");//lab1014
        formdata.AddField("shopID", OwnerInfo.ownerID);
        formdata.AddField("password", OwnerInfo.ownerPW);
        formdata.AddField("shopName", objToAdd.m_storeName);//formdata.AddField("shopName", OwnerInfo.storeInfo.shopName);
        formdata.AddField("shopIntro", objToAdd.m_storeIntro);//formdata.AddField("shopIntro", OwnerInfo.storeInfo.infoList[i].content);
        formdata.AddField("id", objToAdd.m_type);
        formdata.AddField("pos", objToAdd.transform.position.ToString());
        formdata.AddField("rot", objToAdd.transform.rotation.ToString());
        Debug.Log("sending scale = " + objToAdd.transform.localScale.ToString());
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


    IEnumerator createShop()
    {
        WWWForm formdata = new WWWForm();
        formdata.AddField("shopID", OwnerInfo.ownerID);
        formdata.AddField("password", OwnerInfo.ownerPW);
        formdata.AddField("auth_flag", auth_flag);
        formdata.AddField("beaconID", beaconID);
        //formdata.AddField("adfID", OwnerInfo.curUUID);
        //formdata.AddField("adfID", m_curAreaDescription.m_uuid);
        //formdata.AddField("adfID", "f2953b36-b477-2fb9-81c5-1682a435250e");
        formdata.AddField("adfID", "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66");//sofa
        //formdata.AddField("adfID", "f5b2ca84-af86-2899-84f3-ba48570d17b2");//0929
        //formdata.AddField("adfID", "aa305c08-fd20-2325-8b83-7e6e47b0bacc");//65104
        //formdata.AddField("adfID", "aa305c0a-fd20-2325-8ab0-27ae08db9a54");//lab1014

        UnityWebRequest sending = UnityWebRequest.Post(createShopURL, formdata);
        yield return sending.Send();
        if(sending.error != null)
        {
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct while create shop : ");
            if(sending.downloadHandler.text == "success" || sending.downloadHandler.text == "success-total")
            {
                hasCreatedShop = true;
                Debug.Log("success : " + sending.downloadHandler.text);
            }
            else if(sending.downloadHandler.text == "duplicate-shopID")
            {
                hasCreatedShop = true;
                Debug.Log("success : " + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log("false : " + sending.downloadHandler.text);
            }
        }
    }


    IEnumerator loadstoreInfo()
    {
        getStoreObjURL = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/get_obj_info?beaconID=" + "1" + "&adfID=" + "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66" + "&shopID=" + shopID + "&id=" + "0";
        UnityWebRequest sending = UnityWebRequest.Get(getStoreObjURL);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below");
            Debug.Log(sending.downloadHandler.text);
            JavaScriptSerializer js = new JavaScriptSerializer();
            storeObj = js.Deserialize<columnItemofStoreObj>(sending.downloadHandler.text);
            loadShopName = storeObj.shopName;
            loadShopIntro = storeObj.shopIntro;
            loadObjPos = stringToVector3(storeObj.pos);
            loadObjRot = stringToQuaternion(storeObj.rot);
            loadObjScale = stringToVector3(storeObj.scale);
            Debug.Log("shopName = " + loadShopName);
            Debug.Log("shopIntro = " + loadShopIntro);
            Debug.Log("objPos = " + loadObjPos.ToString());
            Debug.Log("objRot = " + loadObjRot.ToString());
            Debug.Log("objScale = " + loadObjScale.ToString());
            createStoreObj();
            //createObj();
        }
    }    
    
    private void createStoreObj()
    {
        //check that create obj after relocalize
        if (m_initialized)
        {
            Debug.Log("load exist store obj");
            newObj = Instantiate(m_storeInfoPrefabs[m_currentObjType],
                                        loadObjPos,
                                        loadObjRot) as GameObject;
            newObj.transform.localScale = loadObjScale;
            newObj.transform.GetChild(1);

            ARStoreObject objScript = newObj.GetComponent<ARStoreObject>();

            objScript.m_type = m_currentObjType;
            objScript.m_timestamp = (float)m_poseController.LastPoseTimestamp;
            objScript.m_storeName = loadShopName;
            objScript.m_storeIntro = loadShopIntro;

            Matrix4x4 uwTDevice = Matrix4x4.TRS(m_poseController.transform.position,
                                                m_poseController.transform.rotation,
                                                Vector3.one);
            Matrix4x4 uwTObj = Matrix4x4.TRS(newObj.transform.position,
                                                newObj.transform.rotation,
                                                Vector3.one);
            objScript.m_deviceTObj = Matrix4x4.Inverse(uwTDevice) * uwTObj;

            m_storeObjList.Add(newObj);

            ButtonGroupToggle button = GameObject.FindObjectOfType<ButtonGroupToggle>();
            button.moveMode();

            //m_selectedObj = newObj;               
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


    public Vector3 stringToVector3(string stringToConvert)
    {
        if (stringToConvert.StartsWith("(") && stringToConvert.EndsWith(")"))
        {
            stringToConvert = stringToConvert.Substring(1, stringToConvert.Length - 2);
        }

        string[] elementArray = stringToConvert.Split(',');

        Vector3 result = new Vector3(
            float.Parse(elementArray[0]),
            float.Parse(elementArray[1]),
            float.Parse(elementArray[2]));

        return result;
    }

    public Quaternion stringToQuaternion(string stringToConvert)
    {
        if (stringToConvert.StartsWith("(") && stringToConvert.EndsWith(")"))
        {
            stringToConvert = stringToConvert.Substring(1, stringToConvert.Length - 2);
        }

        string[] elementArray = stringToConvert.Split(',');

        Quaternion result = new Quaternion(
            float.Parse(elementArray[0]),
            float.Parse(elementArray[1]),
            float.Parse(elementArray[2]),
            float.Parse(elementArray[3]));

        return result;
    }
    /*
    [System.Serializable]
    public class storeObjectData
    {        
        [XmlElement("type")]
        public int m_type;
        
        [XmlElement("position")]
        public Vector3 m_position;
        
        [XmlElement("orientation")]
        public Quaternion m_orientation;
       
        [XmlElement("scale")]
        public Vector3 m_scale;
        
        [XmlElement("name")]
        public string m_name;
        
        [XmlElement("introduce")]
        public string m_introduce;
    }
    */
}
