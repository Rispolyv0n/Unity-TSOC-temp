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
using UnityEngine.Experimental.Networking;
using System.Web.Script.Serialization;

/// <summary>
/// AreaLearningGUIController is responsible for the main game interaction.
/// 
/// This class also takes care of loading / save persistent data(marker), and loop closure handling.
/// </summary>
public class TangoOwnerRecording : MonoBehaviour, ITangoPose, ITangoEvent, ITangoDepth
{
    //to create adf 
    private string createAdfURL = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/create_adfID";
    private string auth_flag = "1";
    private bool hasCreatedAdf = false;

    //to upload the adf
    //public byte[] adfContent = { 0x01, 0x02, 0x03 };
    public byte[] adfContent;
    private string addAdfURL = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/add_adf";
    private string beaconID = "1";
    private string adfID = "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66";//sofa
    private bool hasSendAdf = false;

    //other control
    public UnityEngine.UI.Text m_savingText;    
    [HideInInspector]
    public AreaDescription m_curAreaDescription;
    public GameObject panelConfirm;
    public GameObject panelsaved;
    private string m_guiTextInputContents;
    private bool m_displayGuiTextInput;
    private bool m_initialized = false;
    private bool m_guiTextInputResult;    
    private bool m_findPlaneWaitingForDepth;
    private TangoPoseController m_poseController;
    private Rect m_selectedRect;
    private TangoApplication m_tangoApplication;
    private Thread m_saveThread;

    /*
    //create shop
    private string createShopURL = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/create_shop";
    private string auth_flag = "1";
    private bool hasCreatedShop = false;
    */

    /// <summary>
    /// Unity Start function.
    /// 
    /// We find and assign pose controller and tango application, and register this class to callback events.
    /// </summary>
    public void Start()
    {
        /*
        if (File.Exists("Assets/Resources/file.txt"))
        {
            adfContent = File.ReadAllBytes("Assets/Resources/file.txt");

            Debug.Log("adfContent[0] = " + adfContent[0].ToString());
            Debug.Log("adfContent[1] = " + adfContent[1].ToString());
            Debug.Log("length = " + adfContent.Length.ToString());
        }
        */

        m_poseController = FindObjectOfType<TangoPoseController>();
        m_tangoApplication = FindObjectOfType<TangoApplication>();

        if (m_tangoApplication != null)
        {
            m_tangoApplication.Register(this);
        }
        /*
        //create shop
        Debug.Log("start create shop in recording");
        StartCoroutine(createShop());
        */

    }
    /*
    IEnumerator createShop()
    {
        WWWForm formdata = new WWWForm();
        formdata.AddField("shopID", OwnerInfo.ownerID);
        formdata.AddField("password", OwnerInfo.ownerPW);
        formdata.AddField("auth_flag", auth_flag);
        formdata.AddField("beaconID", beaconID);
        //formdata.AddField("adfID", m_curAreaDescription.m_uuid);
        //formdata.AddField("adfID", "f2953b36-b477-2fb9-81c5-1682a435250e");
        formdata.AddField("adfID", "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66");//sofa
        //formdata.AddField("adfID", "f5b2ca84-af86-2899-84f3-ba48570d17b2");//0929
        //formdata.AddField("adfID", "aa305c08-fd20-2325-8b83-7e6e47b0bacc");//65104
        //formdata.AddField("adfID", "aa305c0a-fd20-2325-8ab0-27ae08db9a54");//lab1014

        UnityWebRequest sending = UnityWebRequest.Post(createShopURL, formdata);
        yield return sending.Send();
        if (sending.error != null)
        {
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct while create shop : ");
            if (sending.downloadHandler.text == "success" || sending.downloadHandler.text == "success-total")
            {
                hasCreatedShop = true;
                Debug.Log("success : " + sending.downloadHandler.text);
            }
            else if (sending.downloadHandler.text == "duplicate-shopID")
            {
                hasCreatedShop = true;
                Debug.Log("success : " + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log("false : " + sending.downloadHandler.text);
            }
            
            if(hasCreatedShop == true)
            {
                //try to send adf
                Debug.Log("start to send adf");
                StartCoroutine(sendingAdfFile());
            }
            
        }
    }
    */

    /// <summary>
    /// Unity Update function.
    /// 
    /// Mainly handle the touch event and place mark in place.
    /// </summary>
    public void Update()
    {       
        if (m_saveThread != null && m_saveThread.ThreadState != ThreadState.Running)
        {

            panelsaved.SetActive(true);

            //m_poseController.gameObject.SetActive(false);
            //Destroy(m_poseController);
            //Camera.main.enabled = false;
            // After saving an Area Description or mark data, hgo back to the menu
            //SceneManager.LoadScene("ownerMenu",LoadSceneMode.Single);
        }
        
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
    /// Unity OnGUI function.
    /// 
    /// Mainly for removing markers.
    /// </summary>
    public void OnGUI()
    {        
#if UNITY_EDITOR
        // Handle text input when there is no device keyboard in the editor.
        if (m_displayGuiTextInput)
        {
            Rect textBoxRect = new Rect(100,
                                        Screen.height - 200,
                                        Screen.width - 200,
                                        100);

            Rect okButtonRect = textBoxRect;
            okButtonRect.y += 100;
            okButtonRect.width /= 2;

            Rect cancelButtonRect = okButtonRect;
            cancelButtonRect.x = textBoxRect.center.x;

            GUI.SetNextControlName("TextField");
            GUIStyle customTextFieldStyle = new GUIStyle(GUI.skin.textField);
            customTextFieldStyle.alignment = TextAnchor.MiddleCenter;
            m_guiTextInputContents = 
                GUI.TextField(textBoxRect, m_guiTextInputContents, customTextFieldStyle);
            GUI.FocusControl("TextField");

            if (GUI.Button(okButtonRect, "OK")
                || (Event.current.type == EventType.keyDown && Event.current.character == '\n'))
            {
                m_displayGuiTextInput = false;
                m_guiTextInputResult = true;
            }
            else if (GUI.Button(cancelButtonRect, "Cancel"))
            {
                m_displayGuiTextInput = false;
                m_guiTextInputResult = false;
            }
        }
#endif
    }
    
    public void Save()
    {
        panelConfirm.SetActive(false);
        StartCoroutine(_DoSaveCurrentAreaDescription());
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
            m_savingText.text = "Saving. " + (float.Parse(tangoEvent.event_value) * 100) + "%";
            //ExportSelectedAreaDescription();
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
        // This frame pair's callback indicates that a loop closure or relocalization has happened. 
        //
        // When learning mode is on, this callback indicates the loop closure event. Loop closure will happen when the
        // system recognizes a pre-visited area, the loop closure operation will correct the previously saved pose 
        // to achieve more accurate result. (pose can be queried through GetPoseAtTime based on previously saved
        // timestamp).
        // Loop closure definition: https://en.wikipedia.org/wiki/Simultaneous_localization_and_mapping#Loop_closure
        //
        // When learning mode is off, and an Area Description is loaded, this callback indicates a
        // relocalization event. Relocalization is when the device finds out where it is with respect to the loaded
        // Area Description. In our case, when the device is relocalized, the markers will be loaded because we
        // know the relative device location to the markers.
        if (poseData.framePair.baseFrame ==
            TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION &&
            poseData.framePair.targetFrame ==
            TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE &&
            poseData.status_code == TangoEnums.TangoPoseStatusType.TANGO_POSE_VALID)
        {
            // When we get the first loop closure/ relocalization event, we initialized all the in-game interactions.
            if (!m_initialized)
            {
                m_initialized = true;
                if (m_curAreaDescription == null)
                {
                    Debug.Log("AndroidInGameController.OnTangoPoseAvailable(): m_curAreaDescription is null");
                    return;
                }

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
    /// Actually do the Area Description save.
    /// </summary>
    /// <returns>Coroutine IEnumerator.</returns>
    private IEnumerator _DoSaveCurrentAreaDescription()
    {
#if UNITY_EDITOR
        // Work around lack of on-screen keyboard in editor:
        if (m_displayGuiTextInput || m_saveThread != null)
        {
            yield break;
        }

        m_displayGuiTextInput = true;
        m_guiTextInputContents = "Unnamed";
        while (m_displayGuiTextInput)
        {
            yield return null;
        }

        bool saveConfirmed = m_guiTextInputResult;
#else
        if (TouchScreenKeyboard.visible || m_saveThread != null)
        {
            yield break;
        }

        TouchScreenKeyboard kb = TouchScreenKeyboard.Open("Unnamed");
        while (!kb.done && !kb.wasCanceled)
        {
            yield return null;
        }

        bool saveConfirmed = kb.done;
#endif
        if (saveConfirmed)
        {
            // Disable interaction before saving.
            m_initialized = false;
            m_savingText.text = "Saving...";
            m_savingText.gameObject.SetActive(true);
            if (m_tangoApplication.m_areaDescriptionLearningMode)
            {
                // The keyboard is not readable if you are not in the Unity main thread. Cache the value here.
                string name;
#if UNITY_EDITOR
                name = m_guiTextInputContents;
#else
                name = kb.text;
#endif

                m_saveThread = new Thread(delegate ()
                {
                    // Start saving process in another thread.
                    m_curAreaDescription = AreaDescription.SaveCurrent();
                    AreaDescription.Metadata metadata = m_curAreaDescription.GetMetadata();
                    metadata.m_name = name;
                    m_curAreaDescription.SaveMetadata(metadata);
                    OwnerInfo.curUUID = m_curAreaDescription.m_uuid;
                });
                m_saveThread.Start();
                
            }
            else
            {
                //_SaveMarkerToDisk();
#pragma warning disable 618
                Application.LoadLevel(Application.loadedLevel);
#pragma warning restore 618
            }
        }
    }

    /// <summary>
    /// Export an Area Description.
    /// </summary>
    public void ExportSelectedAreaDescription()
    {
        Debug.Log("start to export the new adf");
        if (m_curAreaDescription != null)
        {
            StartCoroutine(_DoExportAreaDescription(m_curAreaDescription));
        }
    }

    private IEnumerator _DoExportAreaDescription(AreaDescription areaDescription)
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
            areaDescription.ExportToFile(kb.text);
            //if kb.text is the path...
            adfContent = File.ReadAllBytes(kb.text);

            Debug.Log("start to create adf : uuid = " + OwnerInfo.curUUID);
            StartCoroutine(createAdf());

        }
    }

    IEnumerator createAdf()
    {
        WWWForm formdata = new WWWForm();
        formdata.AddField("shopID", OwnerInfo.ownerID);
        formdata.AddField("password", OwnerInfo.ownerPW);
        formdata.AddField("auth_flag", auth_flag);
        formdata.AddField("beaconID", beaconID);
        formdata.AddField("adfID", m_curAreaDescription.m_uuid);
        //formdata.AddField("adfID", "f2953b36-b477-2fb9-81c5-1682a435250e");
        //formdata.AddField("adfID", "e3eaeaf2-a65d-4e45-8b90-9675e8b31b66");//sofa
        //formdata.AddField("adfID", "f5b2ca84-af86-2899-84f3-ba48570d17b2");//0929
        //formdata.AddField("adfID", "aa305c08-fd20-2325-8b83-7e6e47b0bacc");//65104
        //formdata.AddField("adfID", "aa305c0a-fd20-2325-8ab0-27ae08db9a54");//lab1014

        UnityWebRequest sending = UnityWebRequest.Post(createAdfURL, formdata);
        yield return sending.Send();
        if (sending.error != null)
        {
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct while create adf : ");
            if (sending.downloadHandler.text == "success" || sending.downloadHandler.text == "success-total")
            {
                hasCreatedAdf = true;
                Debug.Log("success : " + sending.downloadHandler.text);
            }
            else if (sending.downloadHandler.text == "duplicate-adfID")
            {
                hasCreatedAdf = true;
                Debug.Log("success : " + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log("false : " + sending.downloadHandler.text);
            }

            if (hasCreatedAdf == true)
            {
                //try to send adf
                Debug.Log("start to send adf");
                StartCoroutine(sendingAdfFile());
            }
        }
    }

    IEnumerator sendingAdfFile()
    {
        WWWForm formdata = new WWWForm();
        formdata.AddField("beaconID", beaconID);
        //formdata.AddField("adfID", adfID);
        formdata.AddField("adfID", OwnerInfo.curUUID);
        formdata.AddBinaryData("file", adfContent);

        UnityWebRequest sending = UnityWebRequest.Post(addAdfURL, formdata);
        yield return sending.Send();
        if(sending.error != null)
        {
            Debug.Log("error while sending adf");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct while sending adf");
            Debug.Log(sending.downloadHandler.text);
            hasSendAdf = true;
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
    
}

