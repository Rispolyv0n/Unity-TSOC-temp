using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PlayerInfo : MonoBehaviour {

    static PlayerInfo playerInfo;

    // personal info
    static public string user_email;
    static public string user_pw;
    static public string user_id;
    //static public DateTime user_startTime;
    static public bool firstLogIn;
    static public bool firstGoHome;
    static public bool firstGoStreet;

    // the upInfo
    static public int currentCharacterID;
    static public float value_strength;
    static public float value_intelligence;
    static public float value_like;
    static public int value_money;
    static public float value_playTime_hr; //
    static public int value_playTime_day; //
    static public int value_level;

    // player stock structure
    public struct stockItem {
        public int id;
        public int quant;
        public stockItem(int id, int quant)
        {
            this.id = id;
            this.quant= quant;
        }
    }
    
    // stock
    static public List<stockItem> props_quant;
    static public List<stockItem> clothes_quant;
    static public List<stockItem> furni_quant;

    // furni decoration
    public struct decoInfo {
        public int id;
        public int numInUserList;
        public Sprite img;
        public Vector3 pos;
    }
    static public List<decoInfo> decoration;
    //static public List<GameObject> decoration;
    
    // favorite shop list
    static public List<string> fav_shopID_list;

    // event collection list
    public struct eventItem {
        public int num;
        public int time;
    }
    static public List<eventItem> eventCollection;

    // player character collection structure <modify!!!!!!!>
    public struct characterItem {
        public int id;
        public int time;
        public int state;
        public float value_playTime_hr; //
        public int value_playTime_day; //
    }
    static public List<characterItem> characterCollection;

    // street mode
    public struct streetModeStruct
    {
        public bool gameObj;
        public bool infoObj;
        public bool gameMode;
    }
    static public streetModeStruct streetMode;

    static public string currentCheckingShopID;


    // make sure only this script can stay on
    private void Awake()
    {

        if (playerInfo == null)
        {
            playerInfo = this;
            DontDestroyOnLoad(this);
        }
        else if (this != playerInfo)
        {
            Destroy(gameObject);
        }

    }





    // Use this for initialization
    void Start () {
        

        fav_shopID_list = new List<string>();
        decoration = new List<decoInfo>();
        
        
        props_quant = new List<stockItem>();
        clothes_quant = new List<stockItem>();
        furni_quant = new List<stockItem>();

        eventCollection = new List<eventItem>();
        characterCollection = new List<characterItem>();

        resetCurrentCharacter(-1);
        //setUserValueInfo();

        InvokeRepeating("decreaseLikeValue",0,5);
        InvokeRepeating("calculatePlayTime",360,360);

        setFirstLogIn();
        setStreetMode();
        currentCheckingShopID = "000";

        Debug.Log("PlayerInfo.Start() done");
    }
	



	// Update is called once per frame
	void Update () {

    }


    public void setUserId(InputField id) {
        user_id = id.text;
        Debug.Log("user id get:" + user_id);
    }

    public void setUserAccount(InputField email) {
        user_email = email.text;
        Debug.Log("user mail get: "+user_email);
    }

    public void setUserPw(InputField pw) {
        user_pw = pw.text;
        Debug.Log("user pw get: " + user_pw);
    }

    private void setFirstLogIn() {
        firstLogIn = true; // get from http
        if (firstLogIn)
        {
            firstGoHome = true;// get from http
            firstGoStreet = true;
        }
        else {
            firstGoHome = false;
            firstGoStreet = false;
            // get from http
        }

    }

    private void decreaseLikeValue() {
        value_like -= 3;
    }

    private void setUserValueInfo() {
        // should get from http
        currentCharacterID = 0;

        value_strength = 10.0f;
        value_intelligence = 50.0f;
        value_like = 100.0f;
        value_money = 500;
        value_playTime_hr = 0;
        value_playTime_day = 0;
        value_level = 1;

        // should get from http----------up
        eventItem event_item = new eventItem();
        event_item.num = 0;
        event_item.time = 2;
        eventCollection.Add(event_item);

        eventItem event_item2 = new eventItem();
        event_item2.num = 2;
        event_item2.time = 4;
        eventCollection.Add(event_item2);

        characterItem char_item = new characterItem();
        char_item.id = 0;
        char_item.time = 3;
        char_item.state = 1;
        char_item.value_playTime_day = 2;
        char_item.value_playTime_hr = 12.5f;
        // should get from http----------down

    }

    public void resetCurrentCharacter(int id) {
        currentCharacterID=id;
        value_strength=0;
        value_intelligence=0;
        value_like=80;
        value_money=550;
        value_playTime_hr=0; //
        value_playTime_day=0; //
        value_level=1;

        eventItem event_item = new eventItem();
        event_item.num = 0;
        event_item.time = 2;
        eventCollection.Add(event_item);

        eventItem event_item2 = new eventItem();
        event_item2.num = 2;
        event_item2.time = 4;
        eventCollection.Add(event_item2);

        fav_shopID_list.RemoveRange(0,fav_shopID_list.Count);
        fav_shopID_list.Add("001");
        fav_shopID_list.Add("002");
    }

    private void calculatePlayTime() {
        value_playTime_hr += 0.1f;
        if (value_playTime_hr >= 24) {
            value_playTime_day++;
            value_playTime_hr -= 24;
        }
    }

    public void setStreetMode() {
        
        if (firstLogIn)
        {
            streetMode.gameMode = true;
            streetMode.gameObj = true;
            streetMode.infoObj = true;
        }
        else {
            // get from http request
        }
    }
}


