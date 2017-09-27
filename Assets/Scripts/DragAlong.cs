using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// attach on the furni obj put in the home scene

public class DragAlong : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Image thisImg;
    public int id;
    public int numInUserList;

    // Use this for initialization
    void Start()
    {
        thisImg = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameObject.FindGameObjectWithTag("home_stockControl").GetComponent<StockControl>().decorateMode)
        {
            thisImg.transform.position = Input.mousePosition;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameObject.FindGameObjectWithTag("home_stockControl").GetComponent<StockControl>().decorateMode)
        {
            thisImg.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameObject.FindGameObjectWithTag("home_stockControl").GetComponent<StockControl>().decorateMode)
        {
            thisImg.transform.position = Input.mousePosition;
            if (thisImg.transform.localPosition.y < -765)
            {
                Debug.Log("destroy this");
                PlayerInfo.furni_quant[numInUserList] = new PlayerInfo.stockItem(id, PlayerInfo.furni_quant[numInUserList].quant + 1);
                StartCoroutine(PlayerInfo.insertFurniInfo(id));
                if (GameObject.FindGameObjectWithTag("home_stock_furni") != null)
                {
                    GameObject.FindGameObjectWithTag("home_stockControl").GetComponent<StockControl>().setStockItem_furni();
                }

                Destroy(gameObject);
            }
        }

    }



    // Update is called once per frame
    void Update()
    {

    }
}
