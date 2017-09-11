using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// attach on every stockItem(a prefab which will be instantiated when checking the stock) in the home scene (in the stock panel)
// adding ownNum on its own, setting infoText and activating infoPanel when pressing

public class StockItemGetInfo : MonoBehaviour , IPointerDownHandler, IPointerUpHandler{

    private Button thisBtn;

    private GameObject infoPanel;
    private Text infoText;

    public Text ownNumText;
    public int ownNum;

    public int itemNum;
    public int categoryNum;
    public Image img;

    public int numInUserList;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        
        // find the correspond num in player's stockItemList
        switch (categoryNum) {
            case 0:
                for (int i = 0; i < PlayerInfo.props_quant.Count; ++i)
                {
                    if (itemNum == PlayerInfo.props_quant[i].id)
                    {
                        numInUserList = i;
                        break;
                    }
                }
                break;
            case 1:
                for (int i = 0; i < PlayerInfo.clothes_quant.Count; ++i)
                {
                    if (itemNum == PlayerInfo.clothes_quant[i].id)
                    {
                        numInUserList = i;
                        break;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < PlayerInfo.furni_quant.Count; ++i)
                {
                    if (itemNum == PlayerInfo.furni_quant[i].id)
                    {
                        numInUserList = i;
                        break;
                    }
                }
                break;
            default:
                break;
        }

        
        switch (categoryNum) {
            case 0:
                // should find the correspond id
                img.overrideSprite = GamingInfo.props_info[itemNum].img;
                ownNumText.text = "擁有數量 : " + PlayerInfo.props_quant[numInUserList].quant;
                break;
            case 1:
                img.overrideSprite = GamingInfo.clothes_info[itemNum].img;
                ownNumText.text = "擁有數量 : " + PlayerInfo.clothes_quant[numInUserList].quant;
                break;
            case 2:
                img.overrideSprite = GamingInfo.furni_info[itemNum].img;
                ownNumText.text = "擁有數量 : " + PlayerInfo.furni_quant[numInUserList].quant;
                break;
            default:
                break;
        }
        
	}
    

    // Update is called once per frame
    void Update () {

	}

    public void OnPointerUp(PointerEventData eventData)
    {
        infoPanel.SetActive(false);
        //throw new NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        infoPanel = GameObject.FindGameObjectWithTag("home_stockControl").GetComponent<StockControl>().infoPanel;
        infoPanel.SetActive(true);
        infoText = GameObject.FindGameObjectWithTag("home_stockControl").GetComponent<StockControl>().infoText;

        switch (categoryNum)
        {
            case 0:
                infoText.text = GamingInfo.props_info[itemNum].name + "。\n" + GamingInfo.props_info[itemNum].price + "元\n" + GamingInfo.props_info[itemNum].info;
                break;
            case 1:
                infoText.text = GamingInfo.clothes_info[itemNum].name + "。\n" + GamingInfo.clothes_info[itemNum].price + "元\n" + GamingInfo.clothes_info[itemNum].info;
                break;
            case 2:
                infoText.text = GamingInfo.furni_info[itemNum].name + "。\n" + GamingInfo.furni_info[itemNum].price + "元\n" + GamingInfo.furni_info[itemNum].info;
                break;
        }

        //throw new NotImplementedException();
    }
}
