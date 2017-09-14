using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetShopInfo_simple : MonoBehaviour {

    private Button thisBtn;
    public Image open;
    public Text shopName;
    public Text category; // not yet
    public Button fav;
    public string shopID;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(openInfo);

        // should get from http request
        foreach (GamingInfo.storeInfoWithID item in GamingInfo.storeInfo) {
            if (shopID == item.id) {
                shopName.text = item.info.shopName;
                break;
            }
        }
        if (GetFavShopList.ifStoreOpen(shopID) == true)
        {
            category.text = "營業中"; // temp
            open.color = new Color(223/255f,161/255f,105/255f);
        }
        else {
            category.text = "休息中"; // temp
            open.color = new Color(161/255f, 156/255f, 151/255f);
        }
	}

    void openInfo() {

    }

	// Update is called once per frame
	void Update () {
	
	}
}
