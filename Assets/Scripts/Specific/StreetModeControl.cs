using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class StreetModeControl : MonoBehaviour {

    public GameObject upperGameInfo;
    public GameObject GamePanel;
    public GameObject InfoPanel;
    public GameObject CharImg;
    public GameObject map;

    public Toggle gameMode;
    public Toggle gameObj;
    public Toggle infoObj;

    // Use this for initialization
    void Start () {

        if (PlayerInfo.firstGoStreet) {
            SceneManager.LoadScene("instruction_street");
        }
        
        setGameMode(PlayerInfo.streetMode.gameMode);
        gameMode.isOn = PlayerInfo.streetMode.gameMode;

        setGameObj(PlayerInfo.streetMode.gameObj);
        gameObj.isOn = PlayerInfo.streetMode.gameObj;

        setInfoObj(PlayerInfo.streetMode.infoObj);
        infoObj.isOn = PlayerInfo.streetMode.infoObj;


        gameMode.onValueChanged.AddListener(setGameMode);
        gameObj.onValueChanged.AddListener(setGameObj);
        infoObj.onValueChanged.AddListener(setInfoObj);
	}

    private void setGameMode(bool arg0)
    {
        if (arg0)
        {
            PlayerInfo.streetMode.gameMode = true;
            upperGameInfo.SetActive(true);
            CharImg.SetActive(true);
            gameObj.isOn = true;
            infoObj.isOn = false;
            map.transform.localPosition = new Vector3(290, 332, 0);
        }
        else {
            PlayerInfo.streetMode.gameMode = false;
            upperGameInfo.SetActive(false);
            CharImg.SetActive(false);
            gameObj.isOn = false;
            infoObj.isOn = true;
            map.transform.localPosition = new Vector3(290, 492, 0);
        }
    }

    private void setInfoObj(bool arg0)
    {
        if (arg0)
        {
            InfoPanel.SetActive(true);
            PlayerInfo.streetMode.infoObj = true;
        }
        else {
            InfoPanel.SetActive(false);
            PlayerInfo.streetMode.infoObj = false;
        }
    }

    private void setGameObj(bool arg0)
    {
        if (arg0)
        {
            GamePanel.SetActive(true);
            PlayerInfo.streetMode.gameObj = true;
        }
        else
        {
            GamePanel.SetActive(false);
            PlayerInfo.streetMode.gameObj = false;
        }
    }

    

  

    
	
	// Update is called once per frame
	void Update () {
	
	}
}
