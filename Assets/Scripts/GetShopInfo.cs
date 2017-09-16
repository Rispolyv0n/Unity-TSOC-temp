using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GetShopInfo : MonoBehaviour {

    public GameObject parent;
    public Text shop_title;
    public Text shop_address;
    public Button btn_fav;

    public string shop_id;

    private Text titlePrefab;
    private Text contentPrefab;



    // Use this for initialization
    void Start () {
        shop_id = PlayerInfo.currentCheckingShopID;

        // should get from http request


        // remove old content
        GameObject[] oldList = GameObject.FindGameObjectsWithTag("content");
        if (oldList.Length != 0)
        {
            foreach (GameObject obj in oldList)
            {
                Destroy(obj);
            }
        }

        // display new content
        GamingInfo.storeInfoItem info = new GamingInfo.storeInfoItem();
        foreach (GamingInfo.storeInfoWithID item in GamingInfo.storeInfo)
        {
            if (shop_id.Equals(item.id))
            {
                info = item.info;
                break;
            }
        }

        shop_title.text = info.shopName;
        shop_address.text = info.shopAddress;
        titlePrefab = Resources.Load<Text>("Prefabs/Text_title");
        contentPrefab = Resources.Load<Text>("Prefabs/Text_content");
        for (int i = 0; i < info.infoList.Count; ++i)
        {
            Text titleObj = Instantiate(titlePrefab);
            titleObj.transform.SetParent(parent.transform);
            titleObj.text = info.infoList[i].title;
            Text contentObj = Instantiate(contentPrefab);
            contentObj.transform.SetParent(parent.transform);
            contentObj.text = info.infoList[i].content;
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

    public void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update () {
	
	}
}
