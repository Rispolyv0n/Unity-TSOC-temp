using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Experimental.Networking;
using System.Web.Script.Serialization;

public class SendUserReport : MonoBehaviour {

    public Button sendBtn;
    public InputField contentInput;
    public GameObject uploadingPanel;
    public GameObject sentPanel;
    public reportPackage pack;
    
    public class reportPackage {
        public string who;
        public string content;
        public string date;
    }
    

	// Use this for initialization
	void Start () {
        sendBtn.onClick.AddListener(checkContent);
	}

    void checkContent() {
        if (contentInput.text == null || contentInput.text == "") {
            uploadingPanel.SetActive(true);
            uploadingPanel.transform.Find("Text").GetComponent<Text>().text = "您的問題回饋內容不可為空";
            return;
        }
        uploadingPanel.SetActive(true);
        uploadingPanel.transform.Find("Text").GetComponent<Text>().text = "上傳資料中...";
        StartCoroutine(sendReport());
    }

    IEnumerator sendReport() {
        pack = new reportPackage();
        pack.who = PlayerInfo.user_id;
        pack.content = contentInput.text;
        pack.date = DateTime.Now.ToString();

        string toUrl = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/add_problemReport";

        JavaScriptSerializer js = new JavaScriptSerializer();
        string reportList = js.Serialize(pack);

        Debug.Log("upload report~:" + reportList);

        WWWForm formdata = new WWWForm();
        formdata.AddField("shopID", PlayerInfo.currentCheckingShopID);
        formdata.AddField("reportPackage", reportList);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("upload report~" + sending.downloadHandler.text);
                uploadingPanel.SetActive(false);
                sentPanel.SetActive(true);
                sentPanel.transform.Find("Text").GetComponent<Text>().text = "上傳成功!";
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }

    }

	// Update is called once per frame
	void Update () {
	
	}
}
