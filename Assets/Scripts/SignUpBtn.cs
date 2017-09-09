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
        //getMail.onValueChanged.AddListener(delegate { checkMailForm(); });
	}
    /*
    void checkMailForm() {
        string strIn = getMail.text;
        if (strIn == null)
        {
            resultDisplay.text = "email格式錯誤，請再次確認";
            thisBtn.interactable = false;
        }
        else {
            resultDisplay.text = "";
            thisBtn.interactable = true;
        }

        invalid = false;
        // Use IdnMapping class to convert Unicode domain names.
        strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper);
        if (invalid) {
            resultDisplay.text = "email格式錯誤，請再次確認";
            thisBtn.interactable = false;
        }
        else
        {
            resultDisplay.text = "";
            thisBtn.interactable = true;
        }

        // Return true if strIn is in valid e-mail format.
        if (!Regex.IsMatch(strIn, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")) {
            resultDisplay.text = "email格式錯誤，請再次確認";
            thisBtn.interactable = false;
        }
        else
        {
            resultDisplay.text = "";
            thisBtn.interactable = true;
        }
    }

    private string DomainMapper(Match match)
    {
        
        // IdnMapping class with default property values.
        IdnMapping idn = new IdnMapping();

        string domainName = match.Groups[2].Value;
        try
        {
            domainName = idn.GetAscii(domainName);
        }
        catch (ArgumentException)
        {
            invalid = true;
        }
        return match.Groups[1].Value + domainName;
    }
    */


    void clickToSend() {
        //StartCoroutine(checkEmail());
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
        string registerUrl = "https://kevin.imslab.org:4000/register";
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
