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
        toUrl = "https://kevin.imslab.org:4000/login";
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

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor){
            Debug.Log("go windows");

            WWWForm formdata = new WWWForm();
            formdata.AddField("username", getID.text);
            formdata.AddField("password", getPW.text);

            using (UnityWebRequest sending = UnityWebRequest.Post(toUrl,formdata)) {
                
                yield return sending.Send();

                if (sending.error != null)
                {
                    warningText.text = "登入錯誤，請再次嘗試(W";
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

            /*
            //this works too
            WWW download = new WWW(toUrl);
            yield return download;
            if (download.error != null)
            {
                Debug.Log(download.error);
            }
            else
            {
                Debug.Log(download.text);
            }
            */


        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            string userAgent = "Mozilla/5.0 (Linux; Android 5.0.1; LG-H440AR Build/LRX21Y; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/43.0.2357.121 Mobile Safari/537.36";
            header.Add("user-agent", userAgent);
            if (!header.ContainsKey("Content-Type"))
            {
                header.Add("Content-Type", "application/x-www-form-urlencoded");
            }


            WWWForm formdata = new WWWForm();
            formdata.AddField("username", getID.text);
            formdata.AddField("password", getPW.text);
            

            Debug.Log("go android");
            // With android, using http instead of https

            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
            formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));


            /*
            using (UnityWebRequest sending = UnityWebRequest.Post("http://kevin.imslab.org:4000/login", formdata))
            {
                
                while (!sending.isDone)
                {
                    warningText.text = "not done...";
                    yield return null;
                }
                

                yield return sending.Send();

                if (sending.error != null)
                {
                    warningText.text = "登入錯誤:"+sending.error;
                    Debug.Log("error below:");
                    Debug.Log(sending.error);

                }
                else
                {
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
                    else
                    {
                        warningText.text = "登入失敗，請再次確認您輸入的使用者名稱與密碼(A";
                        Debug.Log(sending.downloadHandler.text);
                    }

                }

            }
        */

            
            WWW sending = new WWW("http://kevin.imslab.org:4000/login", formdata.data, header);
            while (!sending.isDone)
            {
                warningText.text = "not done...";
                yield return null;
            }
            yield return sending;

            try
            {
                if (!string.IsNullOrEmpty(sending.error))
                {
                    Debug.Log("error below:");

                    if (sending.error == "login failed")
                    {
                        warningText.text = "登入錯誤，請再次嘗試(A:login failed";
                    }
                    else
                    {
                        warningText.text = "???:" + sending.error; //print error
                    }

                    Debug.Log(sending.error);
                }
                else
                {
                    Debug.Log("correct below:");

                    if (sending.text == "success")
                    {
                        warningText.text = "登入成功，畫面切換中...";
                        PlayerInfo.user_id = getID.text;
                        PlayerInfo.user_pw = getPW.text;
                        SceneManager.LoadScene("choose_path");
                    }
                    else
                    {
                        warningText.text = "登入失敗，請再次確認您輸入的使用者名稱與密碼";
                    }

                    Debug.Log(sending.text);
                }
            }
            catch (Exception e) {
                Debug.Log(e.StackTrace);
            }
            
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}