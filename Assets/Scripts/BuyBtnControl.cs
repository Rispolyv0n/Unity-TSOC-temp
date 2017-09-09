using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on each buy button in the shop scene
// when clicking the button, check if the money is sufficient, then do the transaction, info displaying and ownNum control

public class BuyBtnControl : MonoBehaviour {

    public Text itemInfoText;
    public int itemNum;
    public int categoryNum;
    public Text ownNumText;

    // Use this for initialization
    void Start () {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnMouseDown);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnMouseDown()
    {
        switch (categoryNum) {
            case GamingInfo.shop_props_ctgrNum:
                if (PlayerInfo.value_money >= GamingInfo.props_info[itemNum].price)
                {
                    bool hasItem = false;
                    for (int i=0;i<PlayerInfo.props_quant.Count;++i) {
                        if (PlayerInfo.props_quant[i].id == itemNum) {
                            int originNum = PlayerInfo.props_quant[i].quant;
                            originNum++;
                            PlayerInfo.props_quant[i] = new PlayerInfo.stockItem(PlayerInfo.props_quant[i].id, originNum);
                            hasItem = true;
                            break;
                        }
                    }
                    if (!hasItem) {
                        PlayerInfo.props_quant.Add(new PlayerInfo.stockItem(itemNum,1));
                    }

                    PlayerInfo.value_money -= GamingInfo.props_info[itemNum].price;
                    ownNumSetting();
                    itemInfoText.text = "購買成功!";
                }
                else
                {
                    itemInfoText.text = "金額不足QQ";
                }
                break;
            case GamingInfo.shop_clothes_ctgrNum:
                if (PlayerInfo.value_money >= GamingInfo.clothes_info[itemNum].price)
                {
                    bool hasItem = false;
                    for (int i = 0; i < PlayerInfo.clothes_quant.Count; ++i)
                    {
                        if (PlayerInfo.clothes_quant[i].id == itemNum)
                        {
                            int originNum = PlayerInfo.clothes_quant[i].quant;
                            originNum++;
                            PlayerInfo.clothes_quant[i] = new PlayerInfo.stockItem(PlayerInfo.clothes_quant[i].id, originNum);
                            hasItem = true;
                            break;
                        }
                    }
                    if (!hasItem)
                    {
                        PlayerInfo.clothes_quant.Add(new PlayerInfo.stockItem(itemNum, 1));
                    }
                    PlayerInfo.value_money -= GamingInfo.clothes_info[itemNum].price;
                    ownNumSetting();
                    itemInfoText.text = "購買成功!";
                }
                else
                {
                    itemInfoText.text = "金額不足QQ";
                }
                break;
            case GamingInfo.shop_furni_ctgrNum:
                if (PlayerInfo.value_money >= GamingInfo.furni_info[itemNum].price)
                {
                    bool hasItem = false;
                    for (int i = 0; i < PlayerInfo.furni_quant.Count; ++i)
                    {
                        if (PlayerInfo.furni_quant[i].id == itemNum)
                        {
                            int originNum = PlayerInfo.furni_quant[i].quant;
                            originNum++;
                            PlayerInfo.furni_quant[i] = new PlayerInfo.stockItem(PlayerInfo.furni_quant[i].id, originNum);
                            hasItem = true;
                            break;
                        }
                    }
                    if (!hasItem)
                    {
                        PlayerInfo.furni_quant.Add(new PlayerInfo.stockItem(itemNum, 1));
                    }
                    PlayerInfo.value_money -= GamingInfo.furni_info[itemNum].price;
                    ownNumSetting();
                    itemInfoText.text = "購買成功!";
                }
                else
                {
                    itemInfoText.text = "金額不足QQ";
                }
                break;
            default:
                break;
        }
    }

    void ownNumSetting() {
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
    }

    
}
