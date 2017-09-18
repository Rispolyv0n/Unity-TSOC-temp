using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Experimental.Networking;
using UnityEngine.SceneManagement;

public class SendOwnerLogIn : MonoBehaviour {

    private Button thisBtn;
    private string toUrl;

    public InputField getID;
    public InputField getPW;
    public Text warningText;

    // Use this for initialization
    void Start () {
        toUrl = "https://kevin.imslab.org"+PlayerInfo.port+"/login_shop";
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(delegate { warningText.text = "登入中..."; StartCoroutine(toLogIn(getID.text, getPW.text)); });

    }

    IEnumerator toLogIn(string id, string pw)
    {
        WWWForm formdata = new WWWForm();
        formdata.AddField("shopID", id);
        formdata.AddField("password", pw);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();
        if (sending.error != null)
        {
            warningText.text = "登入錯誤，請再次確認您輸入的店家版帳號與密碼";
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
                OwnerInfo.ownerID = id;
                OwnerInfo.ownerPW = pw;
                SceneManager.LoadScene("ownerMenu");
            }
            else
            {
                warningText.text = "登入失敗，請再次確認您輸入的店家版帳號與密碼";
                Debug.Log(sending.downloadHandler.text);
            }
        }

    }

    // Update is called once per frame
    void Update () {
	
	}
}
