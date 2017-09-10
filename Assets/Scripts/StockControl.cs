using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// attach on the empty object in the home scene
// when the three stock category buttons are clicked, call the correspond function
// to instantiate the stock items owned by the user



public class StockControl : MonoBehaviour {

    private int stock_props_size;
    private int stock_clothes_size;
    private int stock_furni_size;

    private GameObject obj_prefab;

    public GameObject infoPanel;
    public Text infoText;

    public bool decorateMode;

    public GameObject loadingPanel;

    // Use this for initialization
    void Start () {
        decorateMode = false;

        if (PlayerInfo.firstGoHome)
        {
            SceneManager.LoadScene("instruction_home");
        }else if (PlayerInfo.currentCharacterID < 0)
        {
            SceneManager.LoadScene("choose_char");
        }

        stock_props_size = PlayerInfo.props_quant.Count;
        stock_clothes_size = PlayerInfo.clothes_quant.Count;
        stock_furni_size = PlayerInfo.furni_quant.Count;
        //Debug.Log(stock_props_size+","+stock_clothes_size+","+stock_furni_size);

        obj_prefab = Resources.Load("Prefabs/Button_stockItem") as GameObject;

        setDecorationPos();
        Invoke("setLoadingOff",0.5f);
    }

    void setLoadingOff() {
        loadingPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void setDecorateMode(bool on) {
        decorateMode = on;
    }

    public void saveDecorationPos() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("furniObj");
        foreach (GameObject obj in objs) {
            PlayerInfo.decoInfo info = new PlayerInfo.decoInfo();
            info.id = obj.GetComponent<DragAlong>().id;
            info.numInUserList = obj.GetComponent<DragAlong>().numInUserList;
            info.pos = obj.transform.position;
            info.img = obj.GetComponent<Image>().sprite;
            PlayerInfo.decoration.Add(info);
        }
    }

    public void setDecorationPos() {
        Debug.Log("now have:"+PlayerInfo.decoration.Count);
        foreach (PlayerInfo.decoInfo info in PlayerInfo.decoration) {
            GameObject obj = new GameObject();
            obj.AddComponent<Image>().overrideSprite = info.img;
            obj.transform.position = info.pos;
            obj.AddComponent<DragAlong>().id = info.id;
            obj.GetComponent<DragAlong>().numInUserList = info.numInUserList;
            obj.GetComponent<Image>().preserveAspect = true;
            Instantiate(obj);
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(156, 110);
            obj.transform.SetParent(GameObject.FindGameObjectWithTag("furniParent").transform);
        }
    }

    public void setStockItem_props() {
        GameObject[] stockItemList = GameObject.FindGameObjectsWithTag("home_stock_items");
        foreach (GameObject itemBtn in stockItemList) {
            Destroy(itemBtn);
        }

        // instantaite, scale, set info
        for (int i = 0; i < stock_props_size; ++i)
        {
            if (PlayerInfo.props_quant[i].quant > 0) {
                GameObject obj = Instantiate(obj_prefab);
                obj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                obj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                obj.transform.SetParent(GameObject.FindGameObjectWithTag("home_stock_props").transform);

                obj.GetComponent<StockItemGetInfo>().itemNum = PlayerInfo.props_quant[i].id;
                obj.GetComponent<StockItemGetInfo>().categoryNum = GamingInfo.shop_props_ctgrNum;
                obj.GetComponent<StockItemGetInfo>().ownNum = PlayerInfo.props_quant[i].quant;
                //obj.GetComponent<StockItemGetInfo>().img.overrideSprite = GamingInfo.props_info[PlayerInfo.props_quant[i].id].img;
            }
        }
    }

    public void setStockItem_clothes() {
        GameObject[] stockItemList = GameObject.FindGameObjectsWithTag("home_stock_items");
        foreach (GameObject itemBtn in stockItemList)
        {
            Destroy(itemBtn);
        }

        for (int i = 0; i < stock_clothes_size; ++i)
        {
            GameObject obj = Instantiate(obj_prefab);
            obj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            obj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            obj.transform.SetParent(GameObject.FindGameObjectWithTag("home_stock_clothes").transform);

            obj.GetComponent<StockItemGetInfo>().itemNum = PlayerInfo.clothes_quant[i].id;
            obj.GetComponent<StockItemGetInfo>().categoryNum = GamingInfo.shop_clothes_ctgrNum;
            obj.GetComponent<StockItemGetInfo>().ownNum = PlayerInfo.clothes_quant[i].quant;
            //obj.GetComponent<StockItemGetInfo>().img.overrideSprite = GamingInfo.clothes_info[PlayerInfo.clothes_quant[i].id].img;
        }
    }

    public void setStockItem_furni() {
        GameObject[] stockItemList = GameObject.FindGameObjectsWithTag("home_stock_items");
        foreach (GameObject itemBtn in stockItemList)
        {
            Destroy(itemBtn);
        }

        for (int i = 0; i < stock_furni_size; ++i)
        {
            if (PlayerInfo.furni_quant[i].quant >= 0) {
                GameObject obj = Instantiate(obj_prefab);
                obj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                obj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                obj.transform.SetParent(GameObject.FindGameObjectWithTag("home_stock_furni").transform);

                obj.GetComponent<StockItemGetInfo>().itemNum = PlayerInfo.furni_quant[i].id;
                obj.GetComponent<StockItemGetInfo>().categoryNum = GamingInfo.shop_furni_ctgrNum;
                obj.GetComponent<StockItemGetInfo>().ownNum = PlayerInfo.furni_quant[i].quant;
                //obj.GetComponent<StockItemGetInfo>().img.overrideSprite = GamingInfo.furni_info[PlayerInfo.furni_quant[i].id].img;
            }

        }
    }
}
