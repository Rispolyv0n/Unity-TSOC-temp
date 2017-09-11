using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on Button_fav in the prefab "ImageBtn_favItem" which will be instantiated in the scene "favorite_shop"

public class RemoveFavShop : MonoBehaviour {

    private Button thisBtn;
    private Image btnImg;
    public string shopID;

    // Use this for initialization
    void Start () {
        thisBtn = GetComponent<Button>();
        btnImg = thisBtn.GetComponent<Image>();
        shopID = thisBtn.transform.parent.GetComponent<GetShopInfo_simple>().shopID;
        thisBtn.onClick.AddListener(removeItem);
    }

    void removeItem() {
        Debug.Log("remove:"+shopID);
        // remove it from fav list

        for (int i = 0; i < PlayerInfo.fav_shopID_list.Count; ++i) {
            if (shopID == PlayerInfo.fav_shopID_list[i]) {
                PlayerInfo.fav_shopID_list.RemoveAt(i);
                break;
            }
        }
        GetFavShopList.refreshContent();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
