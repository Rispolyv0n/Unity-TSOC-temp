using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Experimental.Networking;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class GetShopComments : MonoBehaviour
{

    public GameObject commentsParent;
    public GameObject commentPrefab;
    public Text shopScoreText;

    private float finalScore;
    private float totalScore;

    private string commentContent;
    private string userID;
    private string date;

    public class oneComment
    {
        public string _id;
        public string userID;
        public string shopID;
        public string text_content;
        public string time;
        public int score;
    }
    public List<oneComment> commentList = new List<oneComment>();

    // Use this for initialization
    void Start()
    {
        // when user open the scene shopInfo, auto load the comments about the shop
        // load & instantiate
        finalScore = 0;
        totalScore = 0;
        StartCoroutine(getComments());

    }

    IEnumerator getComments()
    {
        string toUrl = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/get_shopComm?shopID=" + PlayerInfo.currentCheckingShopID;

        Debug.Log("get comments--------");
        UnityWebRequest sending = UnityWebRequest.Get(toUrl);
        yield return sending.Send();
        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            JavaScriptSerializer js = new JavaScriptSerializer();
            commentList = js.Deserialize<List<oneComment>>(sending.downloadHandler.text);
            Debug.Log("instantiate" + commentList.Count + "comments");

            // instantiate
            foreach (oneComment comment in commentList)
            {
                totalScore += comment.score;
                GameObject btn = Instantiate(commentPrefab);
                btn.transform.SetParent(commentsParent.transform);
                for (int i = 0; i < 5; ++i)
                {
                    if ((i + 1) <= comment.score)
                    {
                        btn.transform.Find("Image_star" + (i + 1)).GetComponent<Image>().color = new Color(230 / 255f, 224 / 255f, 87 / 255f);
                    }
                    else
                    {
                        btn.transform.Find("Image_star" + (i + 1)).GetComponent<Image>().color = new Color(193 / 255f, 193 / 255f, 193 / 255f);
                    }
                }
                btn.transform.Find("Text_comment").GetComponent<Text>().text = comment.text_content;
                btn.transform.Find("Text_userID").GetComponent<Text>().text = "- " + comment.userID;
                btn.transform.Find("Text_date").GetComponent<Text>().text = comment.time;
            }

            // display score
            finalScore = totalScore / commentList.Count;
            shopScoreText.text = "顧客評分:" + finalScore;

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
