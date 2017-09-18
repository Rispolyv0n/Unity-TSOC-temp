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
public class TangoOwnerAddObjStart : MonoBehaviour, ITangoLifecycle
{
    //public Canvas canvas_street;

    /// <summary>
    /// The prefab of a standard button in the scrolling list.
    /// </summary>
    //public GameObject m_listElement;

    /// <summary>
    /// The container panel of the Tango space Area Description scrolling list.
    /// </summary>
    //public RectTransform m_listContentParent;

    /// <summary>
    /// Toggle group for the Area Description list.
    /// 
    /// You can only toggle one Area Description at a time. After we get the list of Area Description from Tango,
    /// they are all added to this toggle group.
    /// </summary>
    //public ToggleGroup m_toggleGroup;

    /// <summary>
    /// Enable learning mode toggle.
    /// 
    /// Learning Mode allows the loaded Area Description to be extended with more knowledge about the area..
    /// </summary>
    //public Toggle m_enableLearningToggle;

    /// <summary>
    /// The reference of the TangoPoseController object.
    /// 
    /// TangoPoseController listens to pose updates and applies the correct pose to itself and its built-in camera.
    /// </summary>
    public TangoPoseController m_poseController;

    /// <summary>
    /// Control panel game object.
    /// 
    /// The panel will be enabled when the game starts.
    /// </summary>
    //public GameObject m_gameControlPanel;

    /// <summary>
    /// The GUI controller.
    /// 
    /// GUI controller will be enabled when the game starts.
    /// </summary>    
    public TangoOwnerAddObj m_guiController;

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
        //m_curAreaDescriptionUUID = "ff8c3413-ced8-28f7-9801-38627ec90271";//d24test4
        m_curAreaDescriptionUUID = "ff8c341e-ced8-28f7-9898-6ef42a5060b6";//d24test5
        //m_curAreaDescriptionUUID = OwnerInfo.curUUID;
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
            if (AndroidHelper.IsTangoCorePresent())
            {
                m_tangoApplication.RequestPermissions();
            }
        }
        else
        {
            Debug.Log("No Tango Manager found in scene.");
        }

        //Invoke("startGame", 0.1f);

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

    /*
    /// <summary>
    /// Refresh the scrolling list's content for both list.
    /// 
    /// This function will query from the Tango API for the Tango space Area Description. Also, when it populates 
    /// the scrolling list content, it will connect the delegate for each button in the list. The delegate is
    /// responsible for the actual import/export  through the Tango API.
    /// </summary>
    private void _PopulateList()
    {
        foreach (Transform t in m_listContentParent.transform)
        {
            Destroy(t.gameObject);
        }

        // Update Tango space Area Description list.
        AreaDescription[] areaDescriptionList = AreaDescription.GetList();

        if (areaDescriptionList == null)
        {
            return;
        }

        foreach (AreaDescription areaDescription in areaDescriptionList)
        {
            GameObject newElement = Instantiate(m_listElement) as GameObject;
            AreaDescriptionListElement listElement = newElement.GetComponent<AreaDescriptionListElement>();
            listElement.m_toggle.group = m_toggleGroup;
            listElement.m_areaDescriptionName.text = areaDescription.GetMetadata().m_name;
            listElement.m_areaDescriptionUUID.text = areaDescription.m_uuid;

            // Ensure the lambda makes a copy of areaDescription.
            AreaDescription lambdaParam = areaDescription;
            listElement.m_toggle.onValueChanged.AddListener((value) => _OnToggleChanged(lambdaParam, value));
            newElement.transform.SetParent(m_listContentParent.transform, false);
        }
    }
    */
    /*
    /// <summary>
    /// Callback function when toggle button is selected.
    /// </summary>
    /// <param name="item">Caller item object.</param>
    /// <param name="value">Selected value of the toggle button.</param>
    private void _OnToggleChanged(AreaDescription item, bool value)
    {
        if (value)
        {
            m_curAreaDescriptionUUID = item.m_uuid;
        }
    }
    */
}
