using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Experimental.Networking;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using UnityEngine.SceneManagement;

public class GetShopComments : MonoBehaviour
{

    public GameObject commentsParent;
    public GameObject commentPrefab;
    public Text shopScoreText;
    public GameObject imgPrefabToBeInstantiate;
    public GameObject loadingPanel;

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
        
        public picStruct picture;

        public oneComment() {
            picture.data.data = new byte[] { };
        }

        public struct dataStruct {
            public String type;
            public byte[] data;
        };
        public struct picStruct{
            public dataStruct data;
            public String contentType;
        };
        
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
            SceneManager.LoadScene("street");
        }
        else
        {
            Debug.Log("correct below:");
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = int.MaxValue;
            commentList = js.Deserialize<List<oneComment>>(sending.downloadHandler.text);
            Debug.Log("instantiate" + commentList.Count + "comments");

            // instantiate
            foreach (oneComment comment in commentList)
            {
                totalScore += comment.score;
                GameObject btn = Instantiate(commentPrefab);
                btn.transform.SetParent(commentsParent.transform);
                btn.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

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
                btn.transform.Find("Text_user").GetComponent<Text>().text = "- " + comment.userID;
                btn.transform.Find("Text_date").GetComponent<Text>().text = comment.time;
                
                if (comment.picture.data.data != null && comment.picture.data.data.Length != 0) {
                    Debug.Log("comment got image");
                    Texture2D imgText = new Texture2D(1040,600);
                    imgText.LoadImage(comment.picture.data.data);
                    Sprite imgSprite = Sprite.Create(imgText, new Rect(0, 0, imgText.width, imgText.height), Vector2.zero);
                    GameObject btnImg = Instantiate(imgPrefabToBeInstantiate) as GameObject;
                    
                    /*
                    btnImg.AddComponent<RectTransform>();
                    btnImg.AddComponent<Button>();
                    btnImg.AddComponent<Image>();
                    btnImg.AddComponent<LayoutElement>();
                    btnImg.GetComponent<LayoutElement>().preferredHeight = 600;
                    btnImg.GetComponent<LayoutElement>().preferredWidth = 1040;
                    */
                    btnImg.GetComponent<Image>().overrideSprite = imgSprite;
                    btnImg.transform.SetParent(btn.transform);
                    btnImg.transform.SetSiblingIndex(6);
                    btnImg.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }
                
                
                
            }

            // display score
            finalScore = totalScore / commentList.Count;
            shopScoreText.text = "顧客評分:" + finalScore;

            loadingPanel.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
