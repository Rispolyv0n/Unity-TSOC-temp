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
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Experimental.Networking;
using System.Web.Script.Serialization;
using Lean.Touch;
//using Newtonsoft.Json;

public class TangoStreet : MonoBehaviour, ITangoPose, ITangoEvent, ITangoDepth, ITangoLifecycle
{
    //to sprinkle    
    public GameObject[] m_objPrefabs;//for gamign objects. 0 : coins, 1 : diamond    
    public GameObject[] m_storeInfoPrefabs;//for store objects(now only one)
    public List<GameObject> m_objList = new List<GameObject>();//need sprinkle and delete
    public List<GameObject> m_storeObjList = new List<GameObject>();//need load

    private GameObject newObjObject = null;
    private ARObjects m_selectedObj;
    private ARStoreObject m_selectedStore;
    private int m_curObjType = 0;

    //other control
    public RectTransform m_prefabTouchEffect;
    public Canvas m_canvas;
    [HideInInspector]
    public AreaDescription m_curAreaDescription;
    
    private string m_curAreaDescriptionUUID = "aa305c0a-fd20-2325-8ab0-27ae08db9a54";//lab1014
    //private string m_curAreaDescriptionUUID = "aa305c08-fd20-2325-8b83-7e6e47b0bacc";//65104
    //private string m_curAreaDescriptionUUID = "f5b2ca87-af86-2899-86f7-2789d3d1ce3d";//0929_2
    //private string m_curAreaDescriptionUUID = "f5b2ca84-af86-2899-84f3-ba48570d17b2";//0929
    //private string m_curAreaDescriptionUUID = "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66";//sofa
    //private string m_curAreaDescriptionUUID = "0c641a06-f1e7-4fe4-8f3a-cedc91df8035";//new sofa
    //private string m_curAreaDescriptionUUID = "f2953b36-b477-2fb9-81c5-1682a435250e";//204
    //private string m_curAreaDescriptionUUID = "e12e5a3c-5a09-29b9-98c6-7b3d6fd42737";//d24test6
    //private string m_curAreaDescriptionUUID = "ff8c341e-ced8-28f7-9898-6ef42a5060b6";//d24test5   
    private bool m_initialized = false;
    private bool m_findPlaneWaitingForDepth;    
    private TangoApplication m_tangoApplication;
    private TangoPoseController m_poseController;
    private Rect m_selectedRect;
    //private Thread m_saveThread;

    //to get adf
    private string loadAdfURL;
    private byte[] loadAdfContents;

    //to get all shop id in shopIDList
    private string beaconID = "1";
    private string getShopIdURL = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/get_shopIDs?beaconID=" + "1" + "&adfID=" + "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66";//sofa
    //private string getShopIdURL = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/get_shopIDs?beaconID=" + "1" + "&adfID=" + "aa305c08-fd20-2325-8b83-7e6e47b0bacc";//65104
    //private string getShopIdURL = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/get_shopIDs?beaconID=" + "1" + "&adfID=" + "aa305c0a-fd20-2325-8ab0-27ae08db9a54";//lab1014
    
    //to load store obj
    private string getStoreObjURL;
    private string shopID;
    public string loadShopName;
    public string loadShopIntro;
    public Vector3 loadObjPos;
    public Quaternion loadObjRot;
    public Vector3 loadObjScale;

    public class ColumnItemOfShopIDList
    {       
        public string name = "";        
        public string _id = "";//field names need to be the same QAQ!!!
    }
    //public ColumnItemOfShopIDList IDlist = new ColumnItemOfShopIDList();
    public List<ColumnItemOfShopIDList> shopIDlist = new List<ColumnItemOfShopIDList>();

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

    public void Start()
    {
        Debug.Log("start to load adf");
        StartCoroutine(loadAdf());

        m_poseController = FindObjectOfType<TangoPoseController>();
        m_tangoApplication = FindObjectOfType<TangoApplication>();

        if (m_tangoApplication != null)
        {
            m_tangoApplication.Register(this);
            //m_tangoApplication.RequestPermissions();
            if (AndroidHelper.IsTangoCorePresent())
            {
                m_tangoApplication.RequestPermissions();
            }
        }
        else
        {
            Debug.Log("No Tango Manager found in scene.");
        }

    }

    //load adf
    IEnumerator loadAdf()
    {
        loadAdfURL = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/get_adf?beaconID=" + "1" + "&adfID=" + "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66";//sofa
        UnityWebRequest sending = UnityWebRequest.Get(loadAdfURL);
        yield return sending.Send();

        if(sending.error != null)
        {
            Debug.Log("error while load adf");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct while load adf");
            Debug.Log(sending.downloadHandler.data.ToString());
            loadAdfContents = sending.downloadHandler.data;
            Debug.Log(loadAdfContents[0].ToString());
            Debug.Log(loadAdfContents[1].ToString());
            Debug.Log(loadAdfContents[2].ToString());
            Debug.Log("length = " + loadAdfContents.Length.ToString());
            File.WriteAllBytes("Assets/Resources/fileToWrite.txt", loadAdfContents);
            //File.WriteAllBytes("/sdcard/" + "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66", loadAdfContents);
            //ImportAreaDescription();
        }
    }
    
    public void ImportAreaDescription()
    {
        StartCoroutine(_DoImportAreaDescription());
    }

    private IEnumerator _DoImportAreaDescription()
    {
        if (TouchScreenKeyboard.visible)
        {
            yield break;
        }

        TouchScreenKeyboard kb = TouchScreenKeyboard.Open("/sdcard/", TouchScreenKeyboardType.Default, false);
        while (!kb.done && !kb.wasCanceled)
        {
            yield return null;
        }

        if (kb.done)
        {
            AreaDescription.ImportFromFile(kb.text);
        }
    }
    
    public void sprinkleObjects(int sprinkleType, int nums)//(List<Vector2> touchPoseList)
    {
        m_curObjType = sprinkleType;
        for (int i = 0; i < nums; i++)
        {
            Vector3 objPos = new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f));
            
            //instantiate cube object
            newObjObject = Instantiate(m_objPrefabs[m_curObjType], objPos, Quaternion.identity) as GameObject;//Instantiate : object type -> need 'as XX' to change type

            ARObjects objScript = newObjObject.GetComponent<ARObjects>();

            objScript.m_type = m_curObjType;
            objScript.m_timestamp = (float)m_poseController.LastPoseTimestamp;

            Matrix4x4 uwTDevice = Matrix4x4.TRS(m_poseController.transform.position,
                                                m_poseController.transform.rotation,
                                                Vector3.one);
            Matrix4x4 uwTObj = Matrix4x4.TRS(newObjObject.transform.position,
                                                newObjObject.transform.rotation,
                                                Vector3.one);
            objScript.m_deviceTObj = Matrix4x4.Inverse(uwTDevice) * uwTObj;
            
            if (PlayerInfo.streetMode.gameObj)
            {
                newObjObject.SetActive(true);
            }
            else
            {
                newObjObject.SetActive(false);
            }
            /*
            Renderer newObjRender = newObjObject.GetComponent<Renderer>();
            foreach (GameObject obj in m_objList)
            {
                if(newObjRender.bounds.Intersects(obj.GetComponent<Renderer>().bounds))
                {
                    Destroy(newObjObject);
                }
                else
                {
                    m_objList.Add(newObjObject);
                    i++;
                }                
            }
            */

            //newObjObject.SetActive(true);

            m_objList.Add(newObjObject);
            m_selectedObj = null;
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
            /*
            #pragma warning disable 618
            Application.LoadLevel(Application.loadedLevel);
            #pragma warning restore 618
            */
            AndroidHelper.AndroidQuit();
        }

        if (!m_initialized)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject(0) || GUIUtility.hotControl != 0)
        {
            return;
        }

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            Vector2 guiPosition = new Vector2(t.position.x, Screen.height - t.position.y);
            Camera cam = Camera.main;
            RaycastHit hitInfo;

            if (t.phase != TouchPhase.Began)
            {
                return;
            }

            if (m_selectedRect.Contains(guiPosition))
            {
                // do nothing, the button will handle it
            }
            else if (Physics.Raycast(cam.ScreenPointToRay(t.position), out hitInfo))
            {                
                GameObject tapped = hitInfo.collider.gameObject;
                if (tapped.transform.parent.GetComponent<ARStoreObject>() != null)
                {
                    m_selectedStore = tapped.transform.parent.GetComponent<ARStoreObject>();
                }
                else
                {
                    m_selectedObj = tapped.GetComponent<ARObjects>();
                }

            }        
            else
            {                
                RectTransform touchEffectRectTransform = Instantiate(m_prefabTouchEffect) as RectTransform;
                touchEffectRectTransform.transform.SetParent(m_canvas.transform, false);
                Vector2 normalizedPosition = t.position;
                normalizedPosition.x /= Screen.width;
                normalizedPosition.y /= Screen.height;
                touchEffectRectTransform.anchorMin = touchEffectRectTransform.anchorMax = normalizedPosition;
            }
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
    /// Unity OnGUI function.
    /// 
    /// Mainly for removing markers.
    /// </summary>
    public void OnGUI()
    {
        if (m_selectedObj != null)
        {
            if(m_selectedObj.m_type == 0)
            {
                GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().increaseValue_money((int)UnityEngine.Random.Range(20, 50));
            }
            else if(m_selectedObj.m_type == 1)
            {
                SceneManager.LoadScene("game_1", LoadSceneMode.Additive);
            }
            else 
            {
                GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().increaseValue_like((int)UnityEngine.Random.Range(20, 50));
                //GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().increa
            }
            m_objList.Remove(m_selectedObj.gameObject);            
            m_selectedObj.SendMessage("Hide");
            m_selectedObj = null;
        }
        else if(m_selectedStore != null)
        {
            GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().setCheckingShopName(m_selectedStore.m_storeName);
            SceneManager.LoadScene("shopInfo", LoadSceneMode.Additive);
            
            m_selectedStore = null;
        }
        else
        {
                       
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
                Debug.Log("startup sofa success!!!");
                m_initialized = true;
                if (m_curAreaDescription == null)
                {
                    Debug.Log("AndroidInGameController.OnTangoPoseAvailable(): m_curAreaDescription is null");
                    return;
                }

                Debug.Log("start to get the shop ID");
                StartCoroutine(getShopID());

                sprinkleObjects(0, 10);//coins
                sprinkleObjects(1, 10);//gameDiamonds
                sprinkleObjects(PlayerInfo.currentCharacterID + 2, 15);
                //createStoreObj();
                //_LoadStoreObj();
            }
        }
    }

    IEnumerator getShopID()
    {
        UnityWebRequest sending = UnityWebRequest.Get(getShopIdURL);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below : ");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below : ");
            JavaScriptSerializer js = new JavaScriptSerializer();
            //ColumnItemOfShopIDList[] IDlist = js.Deserialize<ColumnItemOfShopIDList[]>(sending.downloadHandler.text);
            shopIDlist = js.Deserialize<List<ColumnItemOfShopIDList>>(sending.downloadHandler.text);
            //Debug.Log(sending.downloadHandler.text);
            Debug.Log("name = " + shopIDlist[0].name);
            Debug.Log("id = " + shopIDlist[0]._id);
            shopID = shopIDlist[0].name;
            Debug.Log("start to get the obj");
            StartCoroutine(loadstoreInfo());
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
            Debug.Log("correct while load store info");
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
        }
    }

    private void createStoreObj()
    {
        //check that create obj after relocalize
        if (m_initialized)
        {
            Debug.Log("load exist store obj");
            GameObject newObj = Instantiate(m_storeInfoPrefabs[0],
                                        loadObjPos,
                                        loadObjRot) as GameObject;

            newObj.transform.GetChild(1);

            ARStoreObject objScript = newObj.GetComponent<ARStoreObject>();

            objScript.m_type = 0;
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

            newObj.GetComponent<LeanTranslate>().enabled = false;

            m_storeObjList.Add(newObj);           

            //m_selectedObj = newObj;               
        }
    }

    /*
    /// <summary>
    /// get the storeInfo from database
    /// </summary>
    private void createStoreObj()
    {
        Debug.Log("load exist store obj");
        GameObject temp = Instantiate(m_storeInfoPrefabs[0],
                                          objPos,
                                          objRot) as GameObject;
        temp.transform.localScale = objScale;

        temp.transform.GetComponent<ARStoreObject>().m_storeName = shopName;
        temp.transform.GetComponent<ARStoreObject>().m_storeIntro = shopIntro;

        if (PlayerInfo.streetMode.infoObj)
        {
            temp.SetActive(true);
        }
        else
        {
            temp.SetActive(false);
        }

        m_storeObjList.Add(temp);
    }*/
    /*
    private void _LoadStoreObj()
    {
        // Attempt to load the exsiting markers from storage.
        string path = Application.persistentDataPath + "/" + m_curAreaDescription.m_uuid + ".xml";
        //string path = "/storage/emulated/0/Android/data/com.editor.test01/files" + "/" + m_curAreaDescription.m_uuid + ".xml";

        var serializer = new XmlSerializer(typeof(List<storeObjectData>));
        var stream = new FileStream(path, FileMode.Open);

        List<storeObjectData> xmlDataList = serializer.Deserialize(stream) as List<storeObjectData>;

        if (xmlDataList == null)
        {
            Debug.Log("AndroidInGameController._LoadMarkerFromDisk(): xmlDataList is null");
            return;
        }

        m_storeObjList.Clear();
        foreach (storeObjectData store in xmlDataList)
        {
            // Instantiate all markers' gameobject.
            GameObject temp = Instantiate(m_storeInfoPrefabs[store.m_type],
                                          store.m_position,
                                          store.m_orientation) as GameObject;
            temp.transform.localScale = store.m_scale;

            temp.transform.GetComponent<ARStoreObject>().m_storeName = store.m_name;
            temp.transform.GetComponent<ARStoreObject>().m_storeIntro = store.m_introduce;
            
            
            //temp.transform.GetChild(1).gameObject.GetComponent<Text>().text = store.m_name;
            //temp.transform.GetChild(2).gameObject.GetComponent<Text>().text = store.m_introduce;

            //Text introText = temp.transform.GetChild(2).gameObject.GetComponent<Text>();
            //for (int i = 0; i < OwnerInfo.storeInfo.infoList.Count; i++)
            //{
            //    if (OwnerInfo.storeInfo.infoList[i].title == "©±®a¤¶²Ð")
            //    {
            //        introText.text = OwnerInfo.storeInfo.infoList[i].content;
            //    }
            //}
            

            if(PlayerInfo.streetMode.infoObj)
            {
                temp.SetActive(true);
            }
            else
            {
                temp.SetActive(false);
            }

            m_storeObjList.Add(temp);
        }
    }
    */

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

    private List<Beacon> mybeacons = new List<Beacon>();
    private void InitBeaconDetect()
    {
        DebugText("Bluetooth initial");
        BluetoothState.Init();
        DebugText("Add event");
        iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
        DebugText("Add Region");
        iBeaconReceiver.regions = new iBeaconRegion[] { new iBeaconRegion("iBeacon", new Beacon()) };
        DebugText("Start scan");
        iBeaconReceiver.Scan();
        DebugText("End Init");
    }

    private void OnBeaconRangeChanged(Beacon[] beacons)
    {
        DebugText("change");
        foreach (Beacon b in beacons)
        {
            var index = mybeacons.IndexOf(b);
            if (index == -1)
            {
                mybeacons.Add(b);
            }
            else
            {
                mybeacons[index] = b;
            }
        }
        for (int i = mybeacons.Count - 1; i >= 0; --i)
        {
            if (mybeacons[i].lastSeen.AddSeconds(10) < DateTime.Now)
            {
                mybeacons.RemoveAt(i);
            }
        }
        FindBeacons();
    }

    void DebugText(String str)
    {
        Text textView1 = GameObject.Find("Canvas_street/Text").GetComponent<Text>();
        textView1.text = str;
    }

    void FindBeacons()
    {
        String beacon_str = "";
        foreach (Beacon b in mybeacons)
        {
            beacon_str = beacon_str + b.UUID.ToString() + " " + b.major.ToString() + " " + b.minor.ToString() + "\n\n";
        }
        DebugText(beacon_str);
    }

    public void OnTangoPermissions(bool permissionsGranted)
    {
        if (permissionsGranted)
        {
            Debug.Log("permission pass!!!!!");
            DebugText("Start init beacon");
            InitBeaconDetect();
            //Get Beacon Id and request ADF from hear
            AreaDescription areaDescription = AreaDescription.ForUUID(m_curAreaDescriptionUUID);
            m_curAreaDescription = areaDescription;
            m_tangoApplication.m_areaDescriptionLearningMode = false;//m_enableLearningToggle.isOn;        
            m_tangoApplication.Startup(m_curAreaDescription);

            m_poseController.gameObject.SetActive(true);            
        }
        else
        {
            AndroidHelper.ShowAndroidToastMessage("Motion Tracking and Area Learning Permissions Needed");

            // This is a fix for a lifecycle issue where calling
            // Application.Quit() here, and restarting the application
            // immediately results in a deadlocked app.
            AndroidHelper.AndroidQuit();
        }
    }

    public void OnTangoServiceConnected()
    {
        //throw new NotImplementedException();
    }

    public void OnTangoServiceDisconnected()
    {
        //throw new NotImplementedException();
    }
       
    public Vector3 stringToVector3(string stringToConvert)
    {
        if(stringToConvert.StartsWith("(") && stringToConvert.EndsWith(")"))
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
