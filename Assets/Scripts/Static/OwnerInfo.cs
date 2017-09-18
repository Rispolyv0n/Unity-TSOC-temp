using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Experimental.Networking;
using System.Web.Script.Serialization;

public class OwnerInfo : MonoBehaviour {

    static OwnerInfo ownerInfo;

    static public string ownerID;
    static public string ownerPW;
    static public bool hasADF;

    static public string curUUID;

    // openTime structures
    public class period
    {
        public string begin_hr = "";
        public string begin_min = "";
        public string end_hr = "";
        public string end_min = "";
    }
    public class day
    {
        public bool open = new bool();
        public List<period> timePeriod = new List<period>();
    }
    //static public day[] openTime = new day[7]; // the whole openTime array

    // storeInfo structures
    public class ColumnItem
    {
        public string title = "";
        public string content = "";
    }
    public class storeInfoItem
    {
        public string shopName = "";
        public day[] openTime = new day[7];
        public string shopAddress = "";
        public List<ColumnItem> infoList = new List<ColumnItem>();
    }
    static public storeInfoItem storeInfo = new storeInfoItem(); // the whole storeInfo

    // update time
    static public DateTime updateTime;
    static public bool hasUpdated;


    // make sure only this script can stay on
    private void Awake()
    {
        if (ownerInfo == null)
        {
            ownerInfo = this;
            DontDestroyOnLoad(this);
        }
        else if (this != ownerInfo)
        {
            Destroy(gameObject);
        }

    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 7; ++i)
        {
            storeInfo.openTime[i] = new day();
            storeInfo.openTime[i].open = new bool();
            storeInfo.openTime[i].timePeriod = new List<period>();
        }

        StartCoroutine(loadShopContent());
        hasADF = false;
        hasUpdated = false; // should get from http request
        updateTime = new DateTime(); // should get from http request
        Debug.Log("OwnerInfo.cs start()");
    }

    IEnumerator loadShopContent()
    {
        string toUrl = "http://kevin.imslab.org" + PlayerInfo.port + "/get_shopInfo?shopID=" + OwnerInfo.ownerID;
        UnityWebRequest sending = UnityWebRequest.Get(toUrl);
        yield return sending.Send();
        Debug.Log("load the shopInfo data---");

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            JavaScriptSerializer js = new JavaScriptSerializer();
            storeInfo = js.Deserialize<storeInfoItem>(sending.downloadHandler.text);
            Debug.Log(sending.downloadHandler.text);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
