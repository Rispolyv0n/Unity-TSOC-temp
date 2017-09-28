using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IfFirstLogIn : MonoBehaviour {

    public GameObject panel_firstLogIn;
    public GameObject panel_chooseMode;
    
    public Button goHome;
    public Button goStreet;

    public bool run = false;

    // Use this for initialization
    void Start () {

        //firstLogIn = true;
        // get http request: if first time log in
        StartCoroutine(PlayerInfo.downloadUserInfo());
        //while (PlayerInfo.doneDownloading == false) { };
        



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

    void removeLoadingPanel() {
        if (PlayerInfo.firstLogIn)
        {
            PlayerInfo.firstLogIn = false;
            GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().saveUserPace();
            //StartCoroutine(PlayerInfo.uploadBasicInfo());
            panel_firstLogIn.SetActive(true);
        }
        else
        {
            panel_chooseMode.SetActive(true);
            goHome.onClick.AddListener(goHomeControl);
            goStreet.onClick.AddListener(goStreetControl);
        }
    }

	// Update is called once per frame
	void Update () {
        if (PlayerInfo.doneDownloading && run == false) {
            removeLoadingPanel();
            run = true;
        }
	}
}
