using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Experimental.Networking;
using System.Web.Script.Serialization;



public class DisplayUpdateTime : MonoBehaviour
{

    private Text thisText;
    private string time;

    // Use this for initialization
    void Start()
    {
        thisText = GetComponent<Text>();
        StartCoroutine(loadUpdateTime());


    }

    public IEnumerator loadUpdateTime()
    {
        string toUrl = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/get_shopUpdateTime?shopID=";

        Debug.Log("load update time~");

        UnityWebRequest sending = UnityWebRequest.Get(toUrl + OwnerInfo.ownerID);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            time = sending.downloadHandler.text;
            Debug.Log("load time:" + sending.downloadHandler.text);
            if (time != null && time != "")
            {
                //thisText.text = "您上次更新於 " + OwnerInfo.updateTime.ToString("yyyy/MM/dd HH:mm:ss");
                thisText.text = "您上次更新於 " + time;
            }
            else
            {
                thisText.text = "您尚未做任何更新";
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
