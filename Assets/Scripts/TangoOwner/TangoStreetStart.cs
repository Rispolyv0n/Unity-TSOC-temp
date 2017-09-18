using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tango;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// List controller of the scrolling list.
/// 
/// This list controller present a toggle group of Tango space Area Descriptions. The list class also has interface
/// to start the game and connect to Tango Service.
/// </summary>
public class TangoStreetStart : MonoBehaviour, ITangoLifecycle
{
    /// <summary>
    /// The reference of the TangoPoseController object.
    /// 
    /// TangoPoseController listens to pose updates and applies the correct pose to itself and its built-in camera.
    /// </summary>
    public TangoPoseController m_poseController;

    /// <summary>
    /// The GUI controller.
    /// 
    /// GUI controller will be enabled when the game starts.
    /// </summary>
    public TangoStreet m_guiController;

    /// <summary>
    /// A reference to TangoApplication instance.
    /// </summary>
    private TangoApplication m_tangoApplication;

    /// <summary>
    /// The UUID of the selected Area Description.
    /// </summary>
    private string m_curAreaDescriptionUUID;

    /// <summary>
    /// Start the game.
    /// 
    /// This will start the service connection, and start pose estimation from Tango Service.
    /// </summary>
    /// <param name="isNewAreaDescription">If set to <c>true</c> game with start to learn a new Area 
    /// Description.</param>
    public void StartGame()//bool isNewAreaDescription)
    {
        gameObject.SetActive(false);

        //m_curAreaDescriptionUUID = "f2953b36-b477-2fb9-81c5-1682a435250e";//204
        //m_curAreaDescriptionUUID = "e12e5a3c-5a09-29b9-98c6-7b3d6fd42737";//d24test6
        m_curAreaDescriptionUUID = "ff8c341e-ced8-28f7-9898-6ef42a5060b6";//d24test5
        AreaDescription areaDescription = AreaDescription.ForUUID(m_curAreaDescriptionUUID);
        m_guiController.m_curAreaDescription = areaDescription;
        m_tangoApplication.m_areaDescriptionLearningMode = false;//m_enableLearningToggle.isOn;        
        m_tangoApplication.Startup(m_guiController.m_curAreaDescription);

        m_poseController.gameObject.SetActive(true);
        m_guiController.enabled = true;
        //canvas_street.gameObject.SetActive(true);
        //m_gameControlPanel.SetActive(true);
    }

    /// <summary>
    /// Internal callback when a permissions event happens.
    /// </summary>
    /// <param name="permissionsGranted">If set to <c>true</c> permissions granted.</param>
    public void OnTangoPermissions(bool permissionsGranted)
    {
        if (permissionsGranted)
        {
            StartGame();
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
    
    /// <summary>
    /// This is called when successfully connected to the Tango service.
    /// </summary>
    public void OnTangoServiceConnected()
    {
    }
    
    /// <summary>
    /// This is called when disconnected from the Tango service.
    /// </summary>
    public void OnTangoServiceDisconnected()
    {
    }

    /// <summary>
    /// Unity Start function.
    /// 
    /// This function is responsible for connecting callbacks, set up TangoApplication and initialize the data list.
    /// </summary>
    public void Start()
    {
        m_tangoApplication = FindObjectOfType<TangoApplication>();
        
        if (m_tangoApplication != null)
        {
            m_tangoApplication.Register(this);
            m_tangoApplication.RequestPermissions();
            /*if (AndroidHelper.IsTangoCorePresent())
            {
                m_tangoApplication.RequestPermissions();
            }*/
        }
        else
        {
            Debug.Log("No Tango Manager found in scene.");
        }

        //Invoke("StartGame", 0.1f);
    }

    /// <summary>
    /// Unity Update function.
    /// 
    /// Application will be closed when click the back button.
    /// </summary>
    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            // This is a fix for a lifecycle issue where calling
            // Application.Quit() here, and restarting the application
            // immediately results in a deadlocked app.
            AndroidHelper.AndroidQuit();
        }
    }

}
