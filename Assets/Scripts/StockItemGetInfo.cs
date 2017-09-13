using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// attach on every stockItem(a prefab which will be instantiated when checking the stock) in the home scene (in the stock panel)
// adding ownNum on its own, setting infoText and activating infoPanel when pressing
// instantiating furni obj in the scene home, control the first time dragging

public class StockItemGetInfo : MonoBehaviour , IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler{

    private Button thisBtn;

    private GameObject infoPanel;
    private Text infoText;

    private Image objImg; // for dragging to the home
    private GameObject objPanel; // for objs

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

        objPanel = GameObject.FindGameObjectWithTag("furniParent");

    }
    
    // Update is called once per frame
    void Update () {

	}

    IEnumerator adjustPanelSize() {
        yield return 0;
        infoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(infoPanel.GetComponent<RectTransform>().sizeDelta.x, infoText.GetComponent<RectTransform>().sizeDelta.y + 20);
        infoPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(infoPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x, infoText.GetComponent<RectTransform>().sizeDelta.y + 20);
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

        StartCoroutine(adjustPanelSize());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        infoPanel.SetActive(false);
    }

    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (categoryNum == 2 && ownNum > 0)
        {
            /*
            Image obj = Resources.Load<Image>("Prefabs/Image_furniObj");
            objImg = Instantiate(obj);
            */
            objImg = Instantiate(img);
            objImg.tag = "furniObj";
            objImg.sprite = img.overrideSprite;
            objImg.transform.SetParent(objPanel.transform);
            objImg.gameObject.AddComponent<DragAlong>();
            objImg.GetComponent<DragAlong>().id = itemNum;
            objImg.GetComponent<DragAlong>().numInUserList = numInUserList;
        }
        else if (categoryNum == 0 && ownNum > 0) {
            objImg = Instantiate(img);
            objImg.sprite = img.overrideSprite;
            objImg.transform.SetParent(objPanel.transform);
            objImg.transform.position = Input.mousePosition;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (objImg != null) {
            objImg.transform.position = Input.mousePosition;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (objImg != null)
        {
            if (objImg.transform.localPosition.y < -765)
            {
                Destroy(objImg.gameObject);
            }
            else {
                if (categoryNum == 2)
                {
                    objImg.transform.position = Input.mousePosition;
                    if (ownNum > 0)
                    {
                        ownNum--;
                        PlayerInfo.furni_quant[numInUserList] = new PlayerInfo.stockItem(itemNum, PlayerInfo.furni_quant[numInUserList].quant - 1);
                    }
                    ownNumText.text = "擁有數量 : " + PlayerInfo.furni_quant[numInUserList].quant;
                }
                else if (categoryNum == 0) {
                    objImg.transform.position = Input.mousePosition;
                    if (ownNum > 0)
                    {
                        ownNum--;
                        PlayerInfo.props_quant[numInUserList] = new PlayerInfo.stockItem(itemNum, PlayerInfo.props_quant[numInUserList].quant - 1);
                        switch (itemNum) {
                            case 0:
                                PlayerInfo.value_strength += 10;
                                break;
                            case 1:
                                PlayerInfo.value_intelligence += 10;
                                break;
                            case 2:
                                PlayerInfo.value_like += 10;
                                break;
                        }
                    }
                    ownNumText.text = "擁有數量 : " + PlayerInfo.props_quant[numInUserList].quant;
                    Destroy(objImg.gameObject);
                }
                
            }
            
        }
    }
}
