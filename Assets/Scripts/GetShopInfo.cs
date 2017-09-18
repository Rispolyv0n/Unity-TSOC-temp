using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Experimental.Networking;
using System.Web.Script.Serialization;

public class GetShopInfo : MonoBehaviour
{

    public GameObject parent;
    public Text shop_title;
    public Text shop_address;
    public Button btn_fav;

    public string shop_id;

    private Text titlePrefab;
    private Text contentPrefab;


    public GamingInfo.oneInfo theInfo = new GamingInfo.oneInfo();


    // Use this for initialization
    void Start()
    {
        shop_id = PlayerInfo.currentCheckingShopID;

        // remove old content
        GameObject[] oldList = GameObject.FindGameObjectsWithTag("content");
        if (oldList.Length != 0)
        {
            foreach (GameObject obj in oldList)
            {
                Destroy(obj);
            }
        }

        // should get shopInfo from http request
        theInfo.openTime = new GamingInfo.day[7];
        for (int i = 0; i < 7; ++i)
        {
            theInfo.openTime[i] = new GamingInfo.day();
        }

        StartCoroutine(loadShopContent());


    }

    IEnumerator loadShopContent()
    {
        string toUrl = "https://kevin.imslab.org" + PlayerInfo.port + "/get_shopInfo?shopID=" + shop_id;
        UnityWebRequest sending = UnityWebRequest.Get(toUrl);
        yield return sending.Send();
        Debug.Log("load the shopInfo data---");

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            JavaScriptSerializer js = new JavaScriptSerializer();
            theInfo = js.Deserialize<GamingInfo.oneInfo>(sending.downloadHandler.text);
            Debug.Log(sending.downloadHandler.text);


            // display new content
            shop_title.text = theInfo.shopName;
            shop_address.text = theInfo.shopAddress;
            titlePrefab = Resources.Load<Text>("Prefabs/Text_title");
            contentPrefab = Resources.Load<Text>("Prefabs/Text_content");
            for (int i = 0; i < theInfo.infoList.Count; ++i)
            {
                Text titleObj = Instantiate(titlePrefab);
                titleObj.transform.SetParent(parent.transform);
                titleObj.text = theInfo.infoList[i].title;
                Text contentObj = Instantiate(contentPrefab);
                contentObj.transform.SetParent(parent.transform);
                contentObj.text = theInfo.infoList[i].content;
            }

            // check if fav and set img
            bool found = false;
            foreach (string shopID in PlayerInfo.fav_shopID_list)
            {
                if (shopID.Equals(PlayerInfo.currentCheckingShopID))
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                btn_fav.GetComponent<AddFavShop>().isFav = true;
                btn_fav.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ImageSource/BackgroundImage/Street/btn_favoriteList");
            }
            else
            {
                btn_fav.GetComponent<AddFavShop>().isFav = false;
                btn_fav.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ImageSource/BackgroundImage/ShopInfo/btn_favorite");
            }
        }

    }


    // Update is called once per frame
    void Update()
    {

    }
}
