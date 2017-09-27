using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Networking;

public class BtnFuncChanging : MonoBehaviour
{

    private Button thisObj;
    private Text thisBtnText;
    private string getPwUrl;

    public InputField getUserID;
    public Text resultDisplay;
    public GameObject deBtn;

    // Use this for initialization
    void Start()
    {
        getPwUrl = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/forget_pass?username=";

        thisObj = GetComponent<Button>();
        thisBtnText = GetComponentInChildren<Text>();
        thisObj.onClick.AddListener(ActOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ActOnClick()
    {
        if (thisBtnText.text == "Back")
        {
            SceneManager.LoadScene("logIn");
        }
        else
        {
            resultDisplay.text = "確認使用者名稱中，請稍後...";
            StartCoroutine(getPwBack());
        }
    }

    IEnumerator getPwBack()
    {

        using (UnityWebRequest sending = UnityWebRequest.Get(getPwUrl + getUserID.text))
        {
            yield return sending.Send();
            if (sending.error != null)
            {
                resultDisplay.text = "程序錯誤，請再次嘗試";
                Debug.Log("error below:");
                Debug.Log(sending.error);

            }
            else
            {
                Debug.Log("correct below:");
                if (sending.downloadHandler.text == "Check your mail")
                {
                    resultDisplay.text = "密碼尋回方式(password recovery rules)及密鑰(code for recovery)兩封信件已寄至您的註冊信箱，請檢查您的信箱並遵照信件中的指示尋回密碼。請注意信件可能被分類到垃圾信件匣。";

                    deBtn.SetActive(false);
                    thisBtnText.text = "Back";
                }
                else if (sending.downloadHandler.text == "wrong account")
                {
                    resultDisplay.text = "無效的使用者名稱，請再次嘗試";
                }
                else
                {
                    resultDisplay.text = "程序錯誤，請再次嘗試";
                }
                Debug.Log(sending.downloadHandler.text);

            }
        }
    }
}
