using UnityEngine;
using System.Collections;
using System;


public class GetFavShopList : MonoBehaviour
{

    private GameObject favItemPrefab;
    public GameObject favItemParent;
    static public GameObject favItemParent_s;
    static public GameObject favItemPrefab_s;



    // Use this for initialization
    public void Start()
    {
        favItemParent_s = favItemParent;
        favItemPrefab = Resources.Load<GameObject>("Prefabs/ImageBtn_favItem");
        favItemPrefab_s = Resources.Load<GameObject>("Prefabs/ImageBtn_favItem");
        foreach (string shopID in PlayerInfo.fav_shopID_list)
        {
            GameObject obj = Instantiate(favItemPrefab);
            obj.transform.SetParent(favItemParent.transform);
            obj.GetComponent<GetShopInfo_simple>().shopID = shopID;
        }
    }

    static public bool ifStoreOpen(GamingInfo.oneInfo theInfo)
    {

        int dayOfWeek = -1;
        switch (DateTime.Now.DayOfWeek)
        {
            case DayOfWeek.Monday:
                dayOfWeek = 0;
                break;
            case DayOfWeek.Tuesday:
                dayOfWeek = 1;
                break;
            case DayOfWeek.Wednesday:
                dayOfWeek = 2;
                break;
            case DayOfWeek.Thursday:
                dayOfWeek = 3;
                break;
            case DayOfWeek.Friday:
                dayOfWeek = 4;
                break;
            case DayOfWeek.Saturday:
                dayOfWeek = 5;
                break;
            case DayOfWeek.Sunday:
                dayOfWeek = 6;
                break;
        }

        // catch exception?
        if (theInfo.openTime[dayOfWeek].open == true)
        {
            foreach (GamingInfo.period periodItem in theInfo.openTime[dayOfWeek].timePeriod)
            {
                if (DateTime.Now.Hour == int.Parse(periodItem.begin_hr) && DateTime.Now.Minute >= int.Parse(periodItem.begin_min))
                {
                    return true;
                }
                else if (DateTime.Now.Hour > int.Parse(periodItem.begin_hr) && DateTime.Now.Hour < int.Parse(periodItem.end_hr))
                {
                    return true;
                }
                else if (DateTime.Now.Hour == int.Parse(periodItem.end_hr) && DateTime.Now.Minute <= int.Parse(periodItem.end_min))
                {
                    return true;
                }

            }
            return false;
        }
        else
        {
            return false;
        }

    }

    static public void refreshContent()
    {
        for (int i = 0; i < favItemParent_s.transform.childCount; ++i)
        {
            GameObject obj = favItemParent_s.transform.GetChild(i).gameObject;
            Destroy(obj);
        }
        foreach (string shopID in PlayerInfo.fav_shopID_list)
        {
            GameObject obj = Instantiate(favItemPrefab_s);
            obj.transform.SetParent(favItemParent_s.transform);
            obj.GetComponent<GetShopInfo_simple>().shopID = shopID;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
