using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Experimental.Networking;
using UnityEngine.SceneManagement;


public class SignUpBtn : MonoBehaviour {

    private Button thisBtn;
    //private bool invalid;

    public InputField getUserID;
    public InputField getMail;
    public InputField getPw;

    public Text resultDisplay;

    public GameObject panel_succeed;
	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(clickToSend);
	}
    
    void clickToSend() {
        StartCoroutine(Register());
    }
    /*
    IEnumerator checkEmail() {
        // check if email has been registered
        string checkMailUrl = "https://kevin.imslab.org:4000/checkmail?email=";
        Debug.Log("checking:" + getUserID.text + "," + getMail.text + "," + getPw.text);
        using (UnityWebRequest sending = UnityWebRequest.Get(checkMailUrl + getMail.text))
        {
            yield return sending.Send();
            if (sending.error != null)
            {
                resultDisplay.text = "註冊錯誤，請再次嘗試";
                Debug.Log("error below:");
                Debug.Log(sending.error);

            }
            else
            {
                Debug.Log("correct below:");

                if (sending.downloadHandler.text == "existed")
                {
                    resultDisplay.text = "此信箱已被註冊過，請輸入其他信箱";
                    Debug.Log(sending.downloadHandler.text);
                }
                else if (sending.downloadHandler.text == "not found")
                {
                    resultDisplay.text = "";
                    Debug.Log(sending.downloadHandler.text);
                    StartCoroutine(Register());
                }

            }
        }
        
    }
    */

    IEnumerator Register()
    {
        string registerUrl = "https://kevin.imslab.org"+PlayerInfo.port+"/register";
        WWWForm formdata = new WWWForm();
        formdata.AddField("username", getUserID.text);
        formdata.AddField("email", getMail.text);
        formdata.AddField("password", getPw.text);

        Debug.Log("register:"+getUserID.text+","+getMail.text+","+getPw.text);

        using (UnityWebRequest sending = UnityWebRequest.Post(registerUrl, formdata))
        {
            yield return sending.Send();
            if (sending.error != null)
            {
                Debug.Log("error below:");
                if (sending.error == "duplicated")
                {
                    resultDisplay.text = "使用者名稱已被使用";
                    Debug.Log(sending.error);
                }
                else {
                    resultDisplay.text = "註冊錯誤，請再次嘗試";
                    Debug.Log(sending.error);
                }
                
            }
            else
            {
                Debug.Log("correct below:");

                if (sending.downloadHandler.text == "success")
                {
                    resultDisplay.text = "註冊成功，返回登入畫面中...";
                    Debug.Log(sending.downloadHandler.text);
                    panel_succeed.SetActive(true);
                    Invoke("switchTheScene",3);
                }
                else if(sending.downloadHandler.text == "duplicated")
                {
                    resultDisplay.text = "使用者名稱已被使用";
                    Debug.Log(sending.downloadHandler.text);
                }
                else
                {
                    resultDisplay.text = "註冊錯誤，請再次嘗試";
                    Debug.Log(sending.downloadHandler.text);
                }

            }
        }
    }

    void switchTheScene() {
        SceneManager.LoadScene("logIn");
    }

	// Update is called once per frame
	void Update () {
	
	}
}
