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
        thisBtn.onClick.AddListener(sendRequest);
        
    }

    void sendRequest()
    {
        warningText.text = "正在嘗試登入中...";
        StartCoroutine(toLogIn());
        //SceneManager.LoadScene("choose_path");
    }

    IEnumerator toLogIn()
    {
        WWWForm formdata = new WWWForm();
        formdata.AddField("username", getID.text);
        formdata.AddField("password", getPW.text);

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
                PlayerInfo.user_id = getID.text;
                PlayerInfo.user_pw = getPW.text;
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