using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Experimental.Networking;
using UnityEngine.SceneManagement;

public class CommentControl : MonoBehaviour
{

    public Button star_1;
    public Button star_2;
    public Button star_3;
    public Button star_4;
    public Button star_5;

    private Button[] starBtns;

    public InputField comment;

    public Button sendBtn;
    public GameObject statePanel;
    public GameObject failPanel;

    private int stars;
    private string commentContent;

    // Use this for initialization
    void Start()
    {
        stars = 3;
        starBtns = new Button[5] { star_1, star_2, star_3, star_4, star_5 };
        star_1.onClick.AddListener(delegate { starClick(1); });
        star_2.onClick.AddListener(delegate { starClick(2); });
        star_3.onClick.AddListener(delegate { starClick(3); });
        star_4.onClick.AddListener(delegate { starClick(4); });
        star_5.onClick.AddListener(delegate { starClick(5); });
        sendBtn.onClick.AddListener(sendComment);
    }

    void starClick(int starNum)
    {
        foreach (Button btn in starBtns)
        {
            btn.GetComponent<Image>().color = new Color(222 / 255f, 222 / 255f, 222 / 255f);
        }
        for (int i = 0; i < starNum; ++i)
        {
            starBtns[i].GetComponent<Image>().color = new Color(244 / 255f, 201 / 255f, 118 / 255f);
        }
        stars = starNum;
    }

    void sendComment()
    {
        statePanel.SetActive(true);
        commentContent = comment.text;
        Debug.Log(stars + ":" + commentContent);
        // send the request
        StartCoroutine(uploadComment());
        // close the scene : remember to remove the sceneSwitch component on the send Button
    }

    IEnumerator uploadComment()
    {
        string toUrl = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/upload_comment";

        WWWForm formdata = new WWWForm();
        formdata.AddField("userID", PlayerInfo.user_id);
        formdata.AddField("shopID", PlayerInfo.currentCheckingShopID);
        formdata.AddField("text_content", commentContent);
        formdata.AddField("time", DateTime.Now.ToString());
        formdata.AddField("score", stars);
        //formdata.AddField("picture", pw);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();
        if (sending.error != null)
        {
            statePanel.SetActive(false);
            failPanel.SetActive(true);
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                statePanel.transform.Find("Text").GetComponent<Text>().text = "評論上傳成功!";
                Invoke("closeScene", 1);
                Debug.Log(sending.downloadHandler.text);
            }
            else
            {
                statePanel.SetActive(false);
                failPanel.SetActive(true);
                Debug.Log(sending.downloadHandler.text);
            }
        }
    }

    void closeScene()
    {
        SceneManager.UnloadScene("share_comment");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
