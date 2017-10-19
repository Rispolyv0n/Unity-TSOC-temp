using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class StreetModeControl : MonoBehaviour
{

    public TangoStreet TangoStreetController;

    public GameObject upperGameInfo;
    public GameObject GamePanel;
    public GameObject InfoPanel;
    public GameObject CharImg;
    public GameObject map;

    public Toggle gameMode;
    public Toggle gameObj;
    public Toggle infoObj;

    // Use this for initialization
    void Start()
    {

        if (PlayerInfo.firstGoStreet)
        {
            SceneManager.LoadScene("instruction_street");
        }

        TangoStreetController = FindObjectOfType<TangoStreet>();

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
            foreach (GameObject obj in TangoStreetController.m_storeObjList)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in TangoStreetController.m_objList)
            {
                obj.SetActive(true);
            }
            PlayerInfo.streetMode.gameMode = true;
            upperGameInfo.SetActive(true);
            CharImg.SetActive(true);
            gameObj.isOn = true;
            infoObj.isOn = false;
            map.transform.localPosition = new Vector3(290, 332, 0);
        }
        else
        {
            foreach (GameObject obj in TangoStreetController.m_storeObjList)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in TangoStreetController.m_objList)
            {
                obj.SetActive(false);
            }
            PlayerInfo.streetMode.gameMode = false;
            upperGameInfo.SetActive(false);
            CharImg.SetActive(false);
            gameObj.isOn = false;
            infoObj.isOn = true;
            map.transform.localPosition = new Vector3(290, 492, 0);
        }

        StartCoroutine(PlayerInfo.uploadStreetMode());
    }

    private void setInfoObj(bool arg0)
    {
        if (arg0)
        {
            foreach (GameObject obj in TangoStreetController.m_storeObjList)
            {
                obj.SetActive(true);
            }

            //InfoPanel.SetActive(true);
            PlayerInfo.streetMode.infoObj = true;
        }
        else
        {

            foreach (GameObject obj in TangoStreetController.m_storeObjList)
            {
                obj.SetActive(false);
            }

            //InfoPanel.SetActive(false);
            PlayerInfo.streetMode.infoObj = false;
        }
        StartCoroutine(PlayerInfo.uploadStreetMode());
    }

    private void setGameObj(bool arg0)
    {
        if (arg0)
        {
            foreach (GameObject obj in TangoStreetController.m_objList)
            {
                obj.SetActive(true);
            }

            //GamePanel.SetActive(true);
            PlayerInfo.streetMode.gameObj = true;
        }
        else
        {
            foreach (GameObject obj in TangoStreetController.m_objList)
            {
                obj.SetActive(false);
            }

            //GamePanel.SetActive(false);
            PlayerInfo.streetMode.gameObj = false;
        }
        StartCoroutine(PlayerInfo.uploadStreetMode());
    }







    // Update is called once per frame
    void Update()
    {

    }
}
