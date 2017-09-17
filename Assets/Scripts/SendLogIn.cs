using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Text;

public class SendLogIn : MonoBehaviour
{
    private Button thisBtn;
    private string toUrl;

    public InputField getID;
    public InputField getPW;
    public Text warningText;
    

    // Use this for initialization
    void Start()
    {
        toUrl = "https://kevin.imslab.org"+PlayerInfo.port+"/login";
        // Btn event
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(delegate { warningText.text = "登入中..."; StartCoroutine(toLogIn(getID.text, getPW.text)); });

        if (!PlayerInfo.justLogOut && loadUserAccountSuccess())
        {
            warningText.text = "自動登入中...";
            StartCoroutine(toLogIn(PlayerInfo.user_id, PlayerInfo.user_pw));
        }

    }

    public bool loadUserAccountSuccess()
    {
        if (PlayerPrefs.HasKey("UserID"))
        {
            PlayerInfo.user_id = PlayerPrefs.GetString("UserID");
            PlayerInfo.user_pw = PlayerPrefs.GetString("UserPW");
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator toLogIn(string id, string pw)
    {
        WWWForm formdata = new WWWForm();
        formdata.AddField("username", id);
        formdata.AddField("password", pw);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();
        if (sending.error != null)
        {
            warningText.text = "登入錯誤，請再次確認您輸入的使用者名稱與密碼";
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                warningText.text = "登入成功，畫面切換中...";
                Debug.Log(sending.downloadHandler.text);
                PlayerInfo.user_id = id;
                PlayerInfo.user_pw = pw;
                PlayerInfo.justLogOut = true;
                GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().saveUserAccount();
                //PlayerInfo.user_email = ;
                SceneManager.LoadScene("choose_path");
            }
            else {
                warningText.text = "登入失敗，請再次確認您輸入的使用者名稱與密碼";
                Debug.Log(sending.downloadHandler.text);
            }     
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}