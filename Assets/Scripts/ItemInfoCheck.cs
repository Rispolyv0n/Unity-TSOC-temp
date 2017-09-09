using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attached on each shopItem button in the shop scene
// shopItem's buyPanel visibility control & infoDisplay, ownNum obtaining

public class ItemInfoCheck : MonoBehaviour {

    public GameObject itsPanel;
    public Text itemTextInfo;
    public int itemNum;
    public int categoryNum;
    public Image itemImg;
    public Text ownNumText;

    private Button thisBtn;

    // initialization
    void Start () {
        thisBtn = GetComponent<Button>();
        if (thisBtn != null)
        {
            thisBtn.onClick.AddListener(OnMouseDown);
        }
        if (thisBtn.tag != "shop_ctgrBtn") {
            btnSetting();
        }
        
    }
	
	void Update () {
	}

    // when btn clicked: update the item info and setActive(false) of other panel(shop_buyBtn)
    private void OnMouseDown()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("shop_buyBtn");

        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
        }

        // shop item info display
        if (thisBtn.tag != "shop_ctgrBtn")
        {
            switch (categoryNum) {
                case GamingInfo.shop_props_ctgrNum:
                    itemTextInfo.text = GamingInfo.props_info[itemNum].name + "\n" + GamingInfo.props_info[itemNum].price.ToString() + "元\n" + GamingInfo.props_info[itemNum].info;
                    break;
                case GamingInfo.shop_clothes_ctgrNum:
                    itemTextInfo.text = GamingInfo.clothes_info[itemNum].name + "\n" + GamingInfo.clothes_info[itemNum].price.ToString() + "元\n" + GamingInfo.clothes_info[itemNum].info;
                    break;
                case GamingInfo.shop_furni_ctgrNum:
                    itemTextInfo.text = GamingInfo.furni_info[itemNum].name + "\n" + GamingInfo.furni_info[itemNum].price.ToString() + "元\n" + GamingInfo.furni_info[itemNum].info;
                    break;
                default:
                    break;
            }
            
            itsPanel.SetActive(true);
        }
        
    }

    private void btnSetting()
    {
        // setting ownNum
        bool hasItem = false;
        switch (categoryNum)
        {
            case 0:
                hasItem = false;
                foreach (PlayerInfo.stockItem item in PlayerInfo.props_quant)
                {
                    if (item.id == itemNum)
                    {
                        ownNumText.text = "擁有數量: " + item.quant;
                        hasItem = true;
                        break;
                    }
                }
                if (!hasItem) { ownNumText.text = "擁有數量: " + 0; }
                break;
            case 1:
                hasItem = false;
                foreach (PlayerInfo.stockItem item in PlayerInfo.clothes_quant)
                {
                    if (item.id == itemNum)
                    {
                        ownNumText.text = "擁有數量: " + item.quant;
                        hasItem = true;
                        break;
                    }
                }
                if (!hasItem) { ownNumText.text = "擁有數量: " + 0; }
                break;
            case 2:
                hasItem = false;
                foreach (PlayerInfo.stockItem item in PlayerInfo.furni_quant)
                {
                    if (item.id == itemNum)
                    {
                        ownNumText.text = "擁有數量: " + item.quant;
                        hasItem = true;
                        break;
                    }
                }
                if (!hasItem) { ownNumText.text = "擁有數量: " + 0; }
                break;
            default:
                break;
        }

        // setting image
        switch (categoryNum) {
            case 0:
                itemImg.overrideSprite = GamingInfo.props_info[itemNum].img;
                break;
            case 1:
                itemImg.overrideSprite = GamingInfo.clothes_info[itemNum].img;
                break;
            case 2:
                itemImg.overrideSprite = GamingInfo.furni_info[itemNum].img;
                break;
            default:
                break;
        }
        
    }
}
