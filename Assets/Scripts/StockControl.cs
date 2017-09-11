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

    // Use this for initialization
    void Start () {
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
        Debug.Log(stock_props_size+","+stock_clothes_size+","+stock_furni_size);

        obj_prefab = Resources.Load("Prefabs/Button_stockItem") as GameObject;

        
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setStockItem_props() {
        GameObject[] stockItemList = GameObject.FindGameObjectsWithTag("home_stock_items");
        foreach (GameObject itemBtn in stockItemList) {
            Destroy(itemBtn);
        }

        // instantaite, scale, set info
        for (int i = 0; i < stock_props_size; ++i)
        {
            GameObject obj = Instantiate(obj_prefab);
            obj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            obj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            obj.GetComponent<RectTransform>().localScale = new Vector3(0.54f, 0.54f, 0.54f);
            obj.transform.localScale = new Vector3(0.54f, 0.54f, 0.54f);
            obj.transform.SetParent(GameObject.FindGameObjectWithTag("home_stock_props").transform);

            obj.GetComponent<StockItemGetInfo>().itemNum = PlayerInfo.props_quant[i].id;
            obj.GetComponent<StockItemGetInfo>().categoryNum = GamingInfo.shop_props_ctgrNum;
            obj.GetComponent<StockItemGetInfo>().ownNum = PlayerInfo.props_quant[i].quant;
            //obj.GetComponent<StockItemGetInfo>().img.overrideSprite = GamingInfo.props_info[PlayerInfo.props_quant[i].id].img;



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
            obj.GetComponent<RectTransform>().localScale = new Vector3(0.54f, 0.54f, 0.54f);
            obj.transform.localScale = new Vector3(0.54f, 0.54f, 0.54f);
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
            GameObject obj = Instantiate(obj_prefab);
            obj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            obj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            obj.GetComponent<RectTransform>().localScale = new Vector3(0.54f, 0.54f, 0.54f);
            obj.transform.localScale = new Vector3(0.54f, 0.54f, 0.54f);
            obj.transform.SetParent(GameObject.FindGameObjectWithTag("home_stock_furni").transform);

            obj.GetComponent<StockItemGetInfo>().itemNum = PlayerInfo.furni_quant[i].id;
            obj.GetComponent<StockItemGetInfo>().categoryNum = GamingInfo.shop_furni_ctgrNum;
            obj.GetComponent<StockItemGetInfo>().ownNum = PlayerInfo.furni_quant[i].quant;
            //obj.GetComponent<StockItemGetInfo>().img.overrideSprite = GamingInfo.furni_info[PlayerInfo.furni_quant[i].id].img;
        }
    }
}
