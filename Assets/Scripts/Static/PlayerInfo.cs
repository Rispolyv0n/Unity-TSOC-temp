using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Experimental.Networking;
using System.Web.Script.Serialization;
using UnityEngine.SceneManagement;


public class PlayerInfo : MonoBehaviour
{

    static PlayerInfo playerInfo;

    static public string port;
    static public string whichHttp;

    // personal info
    static public string user_email;
    static public string user_pw;
    static public string user_id;
    static public float totalPlayTime_hr;

    static public int value_money;

    static public bool firstLogIn;
    static public bool firstGoHome;
    static public bool firstGoStreet;
    static public bool justLogOut;

    // the currentCharInfo
    static public int currentCharacterID;
    static public string currentCharacterName;
    static public int value_strength;
    static public int value_intelligence;
    static public int value_like;
    static public DateTime char_startTime;
    static public int value_level;

    // player stock structure
    public struct stockItem
    {
        public int id;
        public int quant;
        public stockItem(int id, int quant)
        {
            this.id = id;
            this.quant = quant;
        }
    }

    // stock
    static public List<stockItem> props_quant;
    static public List<stockItem> clothes_quant;
    static public List<stockItem> furni_quant;

    // furni decoration
    public struct decoInfo
    {
        public int id;
        public int numInUserList;
        public Vector3 pos;
    }
    static public List<decoInfo> decoration;
    public class decoInfoForUpload
    {
        public int id;
        public int index; // numInUserList
        public positionInfo pos = new positionInfo();
        public class positionInfo
        {
            public float x;
            public float y;
            public float z;
        };
    }
    //static public List<GameObject> decoration;

    // favorite shop list
    public class favShop
    {
        public string shopID;
    }
    static public List<favShop> fav_shopID_list;

    // event collection list
    public struct eventItem
    {
        public int id; // the same with corresponding character id
        public int num;
        public eventItem(int id, int num)
        {
            this.id = id;
            this.num = num;
        }
    }
    static public List<eventItem> eventCollection;

    // player character collection structure <modify!!!!!!!>
    public struct characterItem
    {
        public int id;
        public string name;
        public int value_strength;
        public int value_intelligence;
        public int value_like;
        public int ending;
        public string startTime;
        public string endTime;
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

    static public string currentCheckingShopID = "Ris_shop";

    static public string currentCheckingShopName;

    // achievement
    public class achievementItem
    {
        public int id;
        public int level; // 1~3
        public achievementItem() { }
        public achievementItem(int id, int level)
        {
            this.id = id;
            this.level = level;
        }
    }
    static public List<achievementItem> achievementCollection;

    static public int temp_value;

    public class userInfoDownloadContainer
    {
        public string username;
        public string email;
        public int value_strength;
        public int value_intelligence;
        public int value_like;
        public int value_money;
        public int value_playTime_hr;
        public int value_level;
        public string chara_startTime;
        public bool gameMode;
        public bool gameObj;
        public bool infoObj;
        public int current_charID;
        public List<decoInfoForUpload> decoration = new List<decoInfoForUpload>();
        public List<characterItem> chara_coll = new List<characterItem>();
        public List<achievementItem> achievement_coll = new List<achievementItem>();
        public List<eventItem> event_coll = new List<eventItem>();
        public List<favShop> fav_shopID = new List<favShop>();
        public List<stockItem> props_quant = new List<stockItem>();
        public List<stockItem> furni_quant = new List<stockItem>();
    }

    static public bool doneDownloading = false;


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
    void Start()
    {
        Screen.SetResolution(433, 693, false);
        port = ":4001";
        whichHttp = "http";

        justLogOut = false;

        fav_shopID_list = new List<favShop>();
        decoration = new List<decoInfo>();
        props_quant = new List<stockItem>();
        clothes_quant = new List<stockItem>();
        furni_quant = new List<stockItem>();
        eventCollection = new List<eventItem>();
        characterCollection = new List<characterItem>();
        achievementCollection = new List<achievementItem>();

        

        //loadUserPace();


        //resetCurrentCharacter(-1); // modify!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        setUserValueInfo();

        //InvokeRepeating("decreaseLikeValue",0,5);
        InvokeRepeating("calculatePlayTime", 360, 360);

        //setStreetMode();



        Debug.Log("PlayerInfo.Start() done");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    public void setUserId(InputField id)
    {
        user_id = id.text;
        Debug.Log("user id get:" + user_id);
    }

    public void setUserPw(InputField pw)
    {
        user_pw = pw.text;
        Debug.Log("user pw get: " + user_pw);
    }

    static public void setUserValueInfo() // not getting info from http yet
    {
        Debug.Log("setUserValueInfo(initial)");
        // should get from http
        currentCharacterID = -1;
        totalPlayTime_hr = 0;
        value_money = 300;
        value_level = 1;

        // should get from http
        currentCharacterName = "";
        
    }

    // called when done choosing char (send btn in the scene choose_char)
    public void resetCurrentCharacter(int id)
    {
        currentCharacterID = id;
        if (id > -1)
        {
            currentCharacterName = GamingInfo.characters[id].name;
        }
        else
        {
            currentCharacterName = "角色名稱";
        }

        value_strength = 0;
        value_intelligence = 0;
        value_like = 0;
        value_level = 1;
        char_startTime = new DateTime(); // set when done choosing char (in the scene choose_char)
        StartCoroutine(uploadBasicInfo());
    }

    private void calculatePlayTime()
    {
        totalPlayTime_hr += 0.1f;
        StartCoroutine(uploadBasicInfo());
    }

    public void setStreetMode()
    {
        if (firstLogIn)
        {
            streetMode.gameMode = true;
            streetMode.gameObj = true;
            streetMode.infoObj = true;
        }
        else
        {
            // get from http request
        }
    }

    // called when done choosing char (send btn in the scene choose_char)
    public void setCharStartTime()
    {
        char_startTime = DateTime.Now;
    }

    //------------------------------------------------------
    // value control functions

    public void increaseValue_money(int value)
    {
        value_money += value;
        StartCoroutine(uploadBasicInfo());
    }

    public void increaseValue_strength(int value)
    {
        if (isOverLimit(value_strength + value))
        {
            return;
        }
        value_strength += value;
        checkifLvUp();
        checkIfGetACAddicted();
        StartCoroutine(uploadBasicInfo());
    }

    public void increaseValue_intelligence(int value)
    {
        if (isOverLimit(value_intelligence + value))
        {
            return;
        }
        value_intelligence += value;
        checkifLvUp();
        checkIfGetACAddicted();
        StartCoroutine(uploadBasicInfo());
    }

    public void increaseValue_like(int value)
    {
        if (isOverLimit(value_like + value))
        {
            return;
        }
        value_like += value;
        checkifLvUp();
        checkIfGetACAddicted();
        StartCoroutine(uploadBasicInfo());
    }

    public bool isOverLimit(int value)
    {
        switch (value_level)
        {
            case 1:
                if (value > GamingInfo.maxValue_lv1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 2:
                if (value > GamingInfo.maxValue_lv2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 3:
                if (value > GamingInfo.maxValue_lv3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                if (value > GamingInfo.maxValue_lv1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
    }

    public void setCheckingShopID(string id)
    {
        currentCheckingShopID = id;
    }

    public void setCheckingShopName(string name)
    {
        currentCharacterName = name;
    }

    /*
    public void setValue_level(int level) {
        value_level = level;
        StartCoroutine(uploadBasicInfo());
    }
    */
    static public void loadUserPace()
    {
        if (PlayerPrefs.HasKey("firstLogIn") == false)
        {
            firstLogIn = true;
        }
        else
        {
            firstLogIn = Convert.ToBoolean(PlayerPrefs.GetString("firstLogIn", firstLogIn.ToString()));
        }

        if (PlayerPrefs.HasKey("firstGoHome") == false)
        {
            firstGoHome = true;
        }
        else
        {
            firstGoHome = Convert.ToBoolean(PlayerPrefs.GetString("firstGoHome", firstGoHome.ToString()));
        }

        if (PlayerPrefs.HasKey("firstGoStreet") == false)
        {
            firstGoStreet = true;
        }
        else
        {
            firstGoStreet = Convert.ToBoolean(PlayerPrefs.GetString("firstGoStreet", firstGoStreet.ToString()));
        }



    }

    static public void saveCharName()
    {
        PlayerPrefs.SetString("currentCharName", currentCharacterName);
    }

    static public void loadCharName()
    {
        if (PlayerPrefs.HasKey("currentCharName"))
        {
            currentCharacterName = PlayerPrefs.GetString("currentCharName");
        }
        else
        {
            currentCharacterName = GamingInfo.characters[currentCharacterID].name;
        }
    }

    public void saveUserPace()
    {
        PlayerPrefs.SetString("firstLogIn", firstLogIn.ToString());
        PlayerPrefs.SetString("firstGoHome", firstGoHome.ToString());
        PlayerPrefs.SetString("firstGoStreet", firstGoStreet.ToString());
    }

    public bool loadUserAccountSuccess()
    {
        if (PlayerPrefs.HasKey("UserID"))
        {
            user_id = PlayerPrefs.GetString("UserID");
            user_pw = PlayerPrefs.GetString("UserPW");
            return true;
        }
        else
        {
            return false;
        }
    }

    static public void saveUserAccount()
    {
        PlayerPrefs.SetString("UserID", user_id);
        PlayerPrefs.SetString("UserPW", user_pw);
        Debug.Log("save user account done");
        return;
    }

    public void checkifLvUp()
    {
        // for testing : to level 2
        if (value_level == 1 && value_like + value_intelligence + value_strength > GamingInfo.totalPoints_toLv2)
        {
            value_level = 2;
        }
        // for testing : to level 3
        if (value_level == 2 && value_like + value_intelligence + value_strength > GamingInfo.totalPoints_toLv3)
        {
            value_level = 3;
        }
        // for testing : done
        if (value_level == 3 && value_like + value_intelligence + value_strength > GamingInfo.totalPoints_toDone)
        {
            temp_value = value_like;
            value_like = -1;

            GameObject[] objs = GameObject.FindGameObjectsWithTag("furniObj");
            PlayerInfo.decoration.RemoveRange(0, PlayerInfo.decoration.Count);

            foreach (GameObject obj in objs)
            {
                if (obj.GetComponent<DragAlong>().numInUserList < PlayerInfo.decoration.Count)
                {
                    PlayerInfo.decoInfo info = new PlayerInfo.decoInfo();
                    info.id = obj.GetComponent<DragAlong>().id;
                    info.numInUserList = obj.GetComponent<DragAlong>().numInUserList;
                    info.pos = obj.transform.position;
                    PlayerInfo.decoration[obj.GetComponent<DragAlong>().numInUserList] = info;
                }
                else
                {
                    PlayerInfo.decoInfo info = new PlayerInfo.decoInfo();
                    info.id = obj.GetComponent<DragAlong>().id;
                    info.numInUserList = obj.GetComponent<DragAlong>().numInUserList;
                    info.pos = obj.transform.position;
                    PlayerInfo.decoration.Add(info);
                }


            }

            StartCoroutine(PlayerInfo.uploadDecoration());

            SceneManager.LoadScene("finishChar");
        }

    }

    public void checkIfGetACAddicted()
    {
        bool found = false;
        bool needUploadAC = false;
        for (int i = 0; i < achievementCollection.Count; ++i)
        {
            // check ac : addicted
            if (achievementCollection[i].id == 4 && achievementCollection[i].level < 3)
            {
                found = true;
                if (achievementCollection[i].level == 1 && totalPlayTime_hr >= GamingInfo.achievements[4].condition_2)
                {
                    achievementItem new_ac = new achievementItem(4, 2);
                    achievementCollection[i] = new_ac;
                    needUploadAC = true;
                }
                else if (achievementCollection[i].level == 2 && totalPlayTime_hr >= GamingInfo.achievements[4].condition_3)
                {
                    achievementItem new_ac = new achievementItem(4, 3);
                    achievementCollection[i] = new_ac;
                    needUploadAC = true;
                }
                break; // found & done checking
            }
        }

        // add lv1 ac
        if (!found && totalPlayTime_hr >= GamingInfo.achievements[4].condition_1)
        {
            achievementItem new_ac = new achievementItem(4, 1);
            achievementCollection.Add(new_ac);
            needUploadAC = true;
        }

        if (needUploadAC)
        {
            StartCoroutine(uploadACCollection());
        }
    }

    static public IEnumerator uploadBasicInfo()
    {
        string toUrl = whichHttp + "://kevin.imslab.org" + port + "/user_basicInfo_update";
        WWWForm formdata = new WWWForm();
        formdata.AddField("username", user_id);
        formdata.AddField("password", user_pw);
        formdata.AddField("charID", currentCharacterID);
        formdata.AddField("strength", value_strength);
        formdata.AddField("intelligence", value_intelligence);
        formdata.AddField("like", value_like);
        formdata.AddField("money", value_money);
        formdata.AddField("hour", totalPlayTime_hr.ToString()); // ????????????
        formdata.AddField("level", value_level);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("upload basic info~" + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }
    }

    static public IEnumerator uploadFavShopList()
    {
        string toUrl = whichHttp + "://kevin.imslab.org" + port + "/user_favShop_set";

        JavaScriptSerializer js = new JavaScriptSerializer();
        string shopList = js.Serialize(fav_shopID_list);

        Debug.Log("upload fav:" + shopList);

        WWWForm formdata = new WWWForm();
        formdata.AddField("username", user_id);
        formdata.AddField("password", user_pw);
        formdata.AddField("fav_shopID_list", shopList);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("upload fav shop~" + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }
    }

    static public IEnumerator uploadEventCollection()
    {
        string toUrl = whichHttp + "://kevin.imslab.org" + port + "/user_eventColl_set";

        JavaScriptSerializer js = new JavaScriptSerializer();
        string eventList = js.Serialize(eventCollection);

        Debug.Log("upload event~:" + eventList);

        WWWForm formdata = new WWWForm();
        formdata.AddField("username", user_id);
        formdata.AddField("password", user_pw);
        formdata.AddField("event_coll_list", eventList);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("upload event~" + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }

    }

    static public IEnumerator uploadACCollection()
    {
        string toUrl = whichHttp + "://kevin.imslab.org" + port + "/user_achieveColl_set";

        JavaScriptSerializer js = new JavaScriptSerializer();
        string ACList = js.Serialize(achievementCollection);

        Debug.Log("upload ac~:" + ACList);

        WWWForm formdata = new WWWForm();
        formdata.AddField("username", user_id);
        formdata.AddField("password", user_pw);
        formdata.AddField("achieve_coll_list", ACList);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("upload AC~" + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }

    }

    static public IEnumerator uploadCharCollection()
    {
        string toUrl = whichHttp + "://kevin.imslab.org" + port + "/user_charaColl_set";

        JavaScriptSerializer js = new JavaScriptSerializer();
        string charList = js.Serialize(characterCollection);

        Debug.Log("upload char~:" + charList);

        WWWForm formdata = new WWWForm();
        formdata.AddField("username", user_id);
        formdata.AddField("password", user_pw);
        formdata.AddField("chara_coll_list", charList);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("upload char~" + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }

    }

    static public IEnumerator uploadStreetMode()
    {
        string toUrl = whichHttp + "://kevin.imslab.org" + port + "/user_streetView_set";

        int gameMode;
        int gameObj;
        int infoObj;
        if (streetMode.gameMode)
        {
            gameMode = 1;
        }
        else
        {
            gameMode = 0;
        }
        if (streetMode.gameObj)
        {
            gameObj = 1;
        }
        else
        {
            gameObj = 0;
        }
        if (streetMode.infoObj)
        {
            infoObj = 1;
        }
        else
        {
            infoObj = 0;
        }

        WWWForm formdata = new WWWForm();
        formdata.AddField("username", user_id);
        formdata.AddField("password", user_pw);
        formdata.AddField("mode_flag", gameMode);
        formdata.AddField("obj_flag", gameObj);
        formdata.AddField("info_flag", infoObj);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("upload street mode~" + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }
    }

    static public IEnumerator uploadDecoration()
    {
        string toUrl = whichHttp + "://kevin.imslab.org" + port + "/user_decoration_record";

        List<decoInfoForUpload> list = new List<decoInfoForUpload>();
        foreach (decoInfo info in decoration)
        {
            decoInfoForUpload item = new decoInfoForUpload();
            item.id = info.id;
            item.index = info.numInUserList;
            item.pos.x = info.pos.x;
            item.pos.y = info.pos.y;
            item.pos.z = info.pos.z;
            list.Add(item);
        }
        JavaScriptSerializer js = new JavaScriptSerializer();
        string listString = js.Serialize(list);

        WWWForm formdata = new WWWForm();
        formdata.AddField("username", user_id);
        formdata.AddField("password", user_pw);
        formdata.AddField("decoration_record", listString);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("upload decoInfo~" + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }
    }

    static public IEnumerator insertPropsInfo(int itemNum)
    {
        string toUrl = whichHttp + "://kevin.imslab.org" + port + "/user_props_insertORupdate";

        int numInUserList = -1;
        for (int i = 0; i < props_quant.Count; ++i)
        {
            if (props_quant[i].id == itemNum)
            {
                numInUserList = i;
                break;
            }
        }

        stockItem item = new stockItem();
        item.id = itemNum;
        item.quant = props_quant[numInUserList].quant;
        List<stockItem> list = new List<stockItem>();
        list.Add(item);

        JavaScriptSerializer js = new JavaScriptSerializer();
        string itemString = js.Serialize(list);
        Debug.Log("insert prop:" + itemString);


        WWWForm formdata = new WWWForm();
        formdata.AddField("username", user_id);
        formdata.AddField("password", user_pw);
        formdata.AddField("props_quant_array", itemString);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("insert props~" + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }

    }

    static public IEnumerator deletePropsInfo(int itemNum)
    {
        string toUrl = whichHttp + "://kevin.imslab.org" + port + "/user_props_delete";

        int numInUserList = -1;
        for (int i = 0; i < props_quant.Count; ++i)
        {
            if (props_quant[i].id == itemNum)
            {
                numInUserList = i;
                break;
            }
        }

        stockItem item = new stockItem();
        item.id = itemNum;
        item.quant = props_quant[numInUserList].quant;
        List<stockItem> list = new List<stockItem>();
        list.Add(item);

        JavaScriptSerializer js = new JavaScriptSerializer();
        string itemString = js.Serialize(list);
        Debug.Log("delete prop:" + itemString);


        WWWForm formdata = new WWWForm();
        formdata.AddField("username", user_id);
        formdata.AddField("password", user_pw);
        formdata.AddField("props_quant_array", itemString);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("delete props~" + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }

    }

    static public IEnumerator insertFurniInfo(int itemNum)
    {
        string toUrl = whichHttp + "://kevin.imslab.org" + port + "/user_furni_insertORupdate";

        int numInUserList = -1;
        for (int i = 0; i < furni_quant.Count; ++i)
        {
            if (furni_quant[i].id == itemNum)
            {
                numInUserList = i;
                break;
            }
        }

        stockItem item = new stockItem();
        item.id = itemNum;
        item.quant = furni_quant[numInUserList].quant;
        List<stockItem> list = new List<stockItem>();
        list.Add(item);

        JavaScriptSerializer js = new JavaScriptSerializer();
        string itemString = js.Serialize(list);
        Debug.Log("insert furni:" + itemString);


        WWWForm formdata = new WWWForm();
        formdata.AddField("username", user_id);
        formdata.AddField("password", user_pw);
        formdata.AddField("furni_quant_array", itemString);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("insert furni~" + sending.downloadHandler.text);
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }

    }

    static public IEnumerator downloadUserInfo()
    {
        string toUrl = whichHttp + "://kevin.imslab.org" + port + "/get_userInfo?username=" + user_id;
        UnityWebRequest sending = UnityWebRequest.Get(toUrl);
        yield return sending.Send();
        Debug.Log("load the userInfo data---");

        userInfoDownloadContainer theInfo = new userInfoDownloadContainer();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            JavaScriptSerializer js = new JavaScriptSerializer();
            theInfo = js.Deserialize<userInfoDownloadContainer>(sending.downloadHandler.text);
            Debug.Log(sending.downloadHandler.text);

            // put the info into right places
            user_email = theInfo.email;
            value_strength = theInfo.value_strength;
            value_intelligence = theInfo.value_intelligence;
            value_like = theInfo.value_like;
            value_money = theInfo.value_money;
            totalPlayTime_hr = theInfo.value_playTime_hr;
            value_level = theInfo.value_level;

            //string temp_date = theInfo.chara_startTime;
            //IFormatProvider culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            //char_startTime = DateTime.Parse(temp_date, culture, System.Globalization.DateTimeStyles.AssumeLocal);
            streetMode.gameMode = theInfo.gameMode;
            streetMode.gameObj = theInfo.gameObj;
            streetMode.infoObj = theInfo.infoObj;
            currentCharacterID = theInfo.current_charID;
            //
            List<decoInfoForUpload> temp_decoInfo = new List<decoInfoForUpload>();
            temp_decoInfo = theInfo.decoration;
            foreach (decoInfoForUpload info in temp_decoInfo)
            {
                decoInfo item = new decoInfo();
                item.id = info.id;
                item.numInUserList = info.index;
                item.pos = new Vector3(info.pos.x, info.pos.y, info.pos.z);
                decoration.Add(item);
            }
            characterCollection = theInfo.chara_coll;
            achievementCollection = theInfo.achievement_coll;
            eventCollection = theInfo.event_coll;
            fav_shopID_list = theInfo.fav_shopID;
            props_quant = theInfo.props_quant;
            furni_quant = theInfo.furni_quant;

            if (value_level == 0)
            { // first log in
                setUserValueInfo();
            }
            else {
                loadCharName();
                loadUserPace();
            }

            doneDownloading = true;

            Debug.Log("mail:" + user_email);
            Debug.Log("value_str:" + value_strength);
            Debug.Log("value_intelli:" + value_intelligence);
            Debug.Log("value_like:" + value_like);
            Debug.Log("money:" + value_money);
            Debug.Log("playTimeHr:" + totalPlayTime_hr);
            Debug.Log("level:" + value_level);
            Debug.Log("charStartTime:" + char_startTime.ToString());
            Debug.Log("mode/gamePanel:" + streetMode.gameMode); //
            Debug.Log("mode/gameObj:" + streetMode.gameObj); //
            Debug.Log("mode/infoObj:" + streetMode.infoObj); //
            Debug.Log("currentCharID:" + currentCharacterID);

        }
    }


}


