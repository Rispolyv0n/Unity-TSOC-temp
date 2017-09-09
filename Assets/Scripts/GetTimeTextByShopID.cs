using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetTimeTextByShopID : MonoBehaviour {

    private Text thisText;

    public string shop_id;
    public GameObject control;


    // Use this for initialization
    void Start()
    {
        thisText = GetComponent<Text>();
        shop_id = control.GetComponent<GetShopInfo>().shop_id;
        thisText.text = parseOpenTimeStruct();
    }

    public string parseOpenTimeStruct()
    {
        GamingInfo.storeInfoItem info = new GamingInfo.storeInfoItem();
        foreach (GamingInfo.storeInfoWithID item in GamingInfo.storeInfo)
        {
            if (shop_id == item.id)
            {
                info = item.info;
                break;
            }
        }

        string result = "";
        for (int i = 0; i < 7; ++i)
        {

            switch (i)
            {
                case 0:
                    result += "周一 ";
                    break;
                case 1:
                    result += "周二 ";
                    break;
                case 2:
                    result += "周三 ";
                    break;
                case 3:
                    result += "周四 ";
                    break;
                case 4:
                    result += "周五 ";
                    break;
                case 5:
                    result += "周六 ";
                    break;
                case 6:
                    result += "周日 ";
                    break;
            }

            if (info.openTime[i].open == true)
            {
                for (int j = 0; j < info.openTime[i].timePeriod.Count; ++j)
                {
                    result += info.openTime[i].timePeriod[j].begin_hr + ":" + info.openTime[i].timePeriod[j].begin_min + " - " + info.openTime[i].timePeriod[j].end_hr + ":" + info.openTime[i].timePeriod[j].end_min + "  ";
                }
                result += "\n";
            }
            else
            {
                result += "公休\n";
            }

        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
