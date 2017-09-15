using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PlayerInfo : MonoBehaviour {

    static PlayerInfo playerInfo;

    static public string port;

    // personal info
    static public string user_email;
    static public string user_pw;
    static public string user_id;
    static public int totalPlayTime_day;
    static public float totalPlayTime_hr;

    static public int value_money;

    static public bool firstLogIn;
    static public bool firstGoHome;
    static public bool firstGoStreet;

    // the currentCharInfo
    static public int currentCharacterID;
    static public string currentCharacterName;
    static public float value_strength;
    static public float value_intelligence;
    static public float value_like;
    static public DateTime char_startTime;
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
        public Vector3 pos;
    }
    static public List<decoInfo> decoration;
    //static public List<GameObject> decoration;
    
    // favorite shop list
    static public List<string> fav_shopID_list;

    // event collection list
    public struct eventItem {
        public int num; // the same with corresponding character id
        public int time;
        public eventItem(int num, int time) {
            this.num = num;
            this.time = time;
        }
    }
    static public List<eventItem> eventCollection;

    // player character collection structure <modify!!!!!!!>
    public struct characterItem {
        public int id;
        public string name;
        public float value_strength;
        public float value_intelligence;
        public float value_like;
        public int ending;
        public DateTime start_time;
        public DateTime end_time;
    }
    static public List<characterItem> characterCollection;
    public const int ENDING_GOOD = 0;
    public const int ENDING_BAD = 1;
    public const int ENDING_STR = 2;

    // street mode
    public struct streetModeStruct
    {
        public bool gameObj;
        public bool infoObj;
        public bool gameMode;
    }
    static public streetModeStruct streetMode;

    static public string currentCheckingShopID;

    // achievement
    public struct achievementItem {
        public int id;
        public int level; // 1~3
        public achievementItem(int id, int level) {
            this.id = id;
            this.level = level;
        }
    }
    static public List<achievementItem> achievementCollection;




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
        port = ":4000";

        fav_shopID_list = new List<string>();
        decoration = new List<decoInfo>();
        
        
        props_quant = new List<stockItem>();
        clothes_quant = new List<stockItem>();
        furni_quant = new List<stockItem>();

        eventCollection = new List<eventItem>();
        characterCollection = new List<characterItem>();
        achievementCollection = new List<achievementItem>();

        //resetCurrentCharacter(); // modify!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        setUserValueInfo();

        //InvokeRepeating("decreaseLikeValue",0,5);
        InvokeRepeating("calculatePlayTime",360,360);

        setFirstLogIn();
        setStreetMode();
        currentCheckingShopID = "000"; // modify!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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
        firstLogIn = false; // get from http
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

    /*
    private void decreaseLikeValue() {
        value_like -= 3;
    }
    */

    private void setUserValueInfo() {
        // should get from http
        currentCharacterID = 1;
        totalPlayTime_day = 12;
        totalPlayTime_hr = 0;
        value_strength = 250f;
        value_intelligence = 240f;
        value_like = 200f;
        value_money = 600;
        value_level = 3;
        char_startTime = new DateTime(2017, 9, 13, 0, 0, 0);

        // should get from http----------up
        /*
        eventItem event_item = new eventItem();
        event_item.num = 0;
        event_item.time = 2;
        eventCollection.Add(event_item);

        eventItem event_item2 = new eventItem();
        event_item2.num = 2;
        event_item2.time = 4;
        eventCollection.Add(event_item2);
        */

        achievementItem ac_item = new achievementItem();
        ac_item.id = 0;
        ac_item.level = 1;

        achievementItem ac_item2 = new achievementItem();
        ac_item2.id = 2;
        ac_item2.level = 3;
        /*
        achievementCollection.Add(ac_item);
        achievementCollection.Add(ac_item2);
        */
        fav_shopID_list.RemoveRange(0, fav_shopID_list.Count);
        fav_shopID_list.Add("001");
        fav_shopID_list.Add("002");

        characterItem char_item = new characterItem();
        char_item.id = 0;
        char_item.name = "偶取ㄉ熊熊降";
        char_item.value_strength = 20;
        char_item.value_intelligence = 80;
        char_item.value_like = 150;
        char_item.ending = ENDING_GOOD;
        char_item.start_time = new DateTime(2017, 9, 12, 1, 2, 3);
        char_item.end_time = new DateTime(2017, 9, 15, 5, 7, 9);

        characterItem char_item2 = new characterItem();
        char_item2.id = 0;
        char_item2.name = "偶取ㄉ熊熊降2";
        char_item2.value_strength = 30;
        char_item2.value_intelligence = 90;
        char_item2.value_like = 160;
        char_item2.ending = ENDING_STR;
        char_item2.start_time = new DateTime(2017, 9, 13, 2, 3, 4);
        char_item2.end_time = new DateTime(2017, 9, 15, 5, 7, 9);

        characterItem char_item3 = new characterItem();
        char_item3.id = 1;
        char_item3.name = "神奇蝦仁";
        char_item3.value_strength = 30;
        char_item3.value_intelligence = 90;
        char_item3.value_like = 160;
        char_item3.ending = ENDING_STR;
        char_item3.start_time = new DateTime(2017, 9, 13, 2, 3, 4);
        char_item3.end_time = new DateTime(2017, 9, 15, 5, 7, 9);

        characterItem char_item4 = new characterItem();
        char_item4.id = 0;
        char_item4.name = "偶取ㄉ熊熊降3";
        char_item4.value_strength = 30;
        char_item4.value_intelligence = 90;
        char_item4.value_like = 160;
        char_item4.ending = ENDING_STR;
        char_item4.start_time = new DateTime(2017, 9, 13, 2, 3, 4);
        char_item4.end_time = new DateTime(2017, 9, 15, 5, 7, 9);

        
        characterCollection.Add(char_item);
        characterCollection.Add(char_item2);
        characterCollection.Add(char_item3);
        characterCollection.Add(char_item4);
        
        // should get from http----------down

    }

    // called when done choosing char (send btn in the scene choose_char)
    public void resetCurrentCharacter(int id) {
        currentCharacterID=id;
        if (id > -1)
        {
            currentCharacterName = GamingInfo.characters[id].name;
        }
        else {
            currentCharacterName = "角色名稱";
        }
        
        value_strength=0;
        value_intelligence=0;
        value_like=0;
        value_level=1;
        char_startTime = new DateTime(); // set when done choosing char (in the scene choose_char)
    }

    private void calculatePlayTime() {
        totalPlayTime_hr += 0.1f;
        if (totalPlayTime_hr >= 24) {
            totalPlayTime_day++;
            totalPlayTime_hr -= 24;
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

    // called when done choosing char (send btn in the scene choose_char)
    public void setCharStartTime() {
        char_startTime = DateTime.Now;
    }
    
}


