using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IfFirstLogIn : MonoBehaviour {

    public GameObject panel_firstLogIn;
    public GameObject panel_chooseMode;
    private bool firstLogIn;
    
    public Button goHome;
    public Button goStreet;

    // Use this for initialization
    void Start () {
        
        //firstLogIn = true;
        // get http request: if first time log in
        if (PlayerInfo.firstLogIn)
        {
            panel_firstLogIn.SetActive(true);
        }
        else {
            panel_chooseMode.SetActive(true);
            goHome.onClick.AddListener(goHomeControl);
            goStreet.onClick.AddListener(goStreetControl);
        }



	}

    void goHomeControl() {
        if (PlayerInfo.firstGoHome)
        {
            SceneManager.LoadScene("instruction_home");
        }
        else {
            SceneManager.LoadScene("home");
        }
    }

    void goStreetControl() {
        PlayerInfo.streetMode.gameMode = false;
        PlayerInfo.streetMode.gameObj = false;
        PlayerInfo.streetMode.infoObj = true;

        if (PlayerInfo.firstGoStreet)
        {
            SceneManager.LoadScene("instruction_street");
        }
        else
        {
            SceneManager.LoadScene("street");
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
