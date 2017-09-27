using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Experimental.Networking;
using System.Web.Script.Serialization;

public class GetShopInfo_simple : MonoBehaviour
{

    private Button thisBtn;
    public Image open;
    public Text shopName;
    public Text category; // not yet
    public Button fav;
    public string shopID;


    public GamingInfo.oneInfo theInfo = new GamingInfo.oneInfo();


    // Use this for initialization
    void Start()
    {

        thisBtn = GetComponent<Button>();

        theInfo.openTime = new GamingInfo.day[7];
        for (int i = 0; i < 7; ++i)
        {
            theInfo.openTime[i] = new GamingInfo.day();
        }

        StartCoroutine(loadShopContent());



    }


    IEnumerator loadShopContent()
    {
        string toUrl = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/get_shopInfo?shopID=" + shopID;
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

            shopName.text = theInfo.shopName;
            if (GetFavShopList.ifStoreOpen(theInfo) == true)
            {
                category.text = "營業中"; // temp
                open.color = new Color(223 / 255f, 161 / 255f, 105 / 255f);
            }
            else
            {
                category.text = "休息中"; // temp
                open.color = new Color(161 / 255f, 156 / 255f, 151 / 255f);
            }

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
