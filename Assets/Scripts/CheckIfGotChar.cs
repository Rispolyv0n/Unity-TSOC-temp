using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckIfGotChar : MonoBehaviour {

    private Button homeBtn;

    public bool goHome; // true for home, false for street;
    public GameObject warningPanel;

	// Use this for initialization
	void Start () {
        homeBtn = GetComponent<Button>();
        homeBtn.onClick.AddListener(checkAndSwitchScene);
	}

    public void setGoHome() {
        if (PlayerInfo.streetMode.gameMode == false || PlayerInfo.streetMode.gameObj == false) {
            warningPanel.SetActive(true);
            goHome = true;
        }
        else if (!PlayerInfo.firstLogIn)
        {
            SceneManager.LoadScene("home");
        }
        else {
            goHome = true;
            warningPanel.SetActive(true);
        }
        
    }

    public void setGoShop() {
        if (PlayerInfo.streetMode.gameMode == false || PlayerInfo.streetMode.gameObj == false)
        {
            warningPanel.SetActive(true);
            goHome = false;
        }
        else if (!PlayerInfo.firstLogIn)
        {
            GamingInfo.fromHomeStreet = false;
            SceneManager.LoadScene("shop");
        }
        else {
            goHome = false;
            warningPanel.SetActive(true);
        }
        
    }

    void checkAndSwitchScene() {
        if (goHome)
        {
                SceneManager.LoadScene("home");
        }
        else {
                GamingInfo.fromHomeStreet = false;
                SceneManager.LoadScene("shop");
        }
        
    }

	// Update is called once per frame
	void Update () {
	
	}
}
