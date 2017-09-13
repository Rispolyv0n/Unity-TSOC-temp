using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class OwnerInfo : MonoBehaviour {

    static OwnerInfo ownerInfo;

    static public bool hasADF;

    // openTime structures
    public struct period {
        public string begin_hr;
        public string begin_min;
        public string end_hr;
        public string end_min;
    }
    public struct day {
        public bool open;
        public List<period> timePeriod;
    }
    //static public day[] openTime = new day[7]; // the whole openTime array

    // storeInfo structures
    public struct ColumnItem {
        public string title;
        public string content;
    }
    public struct storeInfoItem {
        public string shopName;
        public day[] openTime;
        public string shopAddress;
        public List<ColumnItem> infoList;
    }
    static public storeInfoItem storeInfo; // the whole storeInfo

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
    void Start () {
        storeInfo.openTime = new day[7]; // storeInfo initialization : openTime array
        hasADF = false;
        hasUpdated = false; // should get from http request
        updateTime = new DateTime(); // should get from http request
        Debug.Log("OwnerInfo.cs start()");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
