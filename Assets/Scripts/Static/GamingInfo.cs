using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GamingInfo : MonoBehaviour {

    static GamingInfo gameInfoScript;



    // for shop return button control
    static public bool fromHomeStreet;

    // size of the three categories of shop items
    static public int shop_props_size;
    static public int shop_clothes_size;
    static public int shop_furni_size;

    public const int shop_props_ctgrNum = 0;
    public const int shop_clothes_ctgrNum = 1;
    public const int shop_furni_ctgrNum = 2;

    // one shop item structure
    public struct goods
    {
        public int id; // no need??
        public int category; // no need??
        public int price;
        public string name;
        public string info;
        public Sprite img;
    }

    // the three categories of shop-items-list
    static public goods[] props_info;
    static public goods[] clothes_info;
    static public goods[] furni_info;

    // shop info------------

    // openTime structures
    public struct period
    {
        public string begin_hr;
        public string begin_min;
        public string end_hr;
        public string end_min;
    }
    public struct day
    {
        public bool open;
        public List<period> timePeriod;
    }

    // storeInfo structures
    public struct ColumnItem
    {
        public string title;
        public string content;
    }
    public struct storeInfoItem
    {
        public string shopName;
        public day[] openTime;
        public string shopAddress;
        public List<ColumnItem> infoList;
    }
    public struct storeInfoWithID {
        public string id;
        public storeInfoItem info;
    }
    static public List<storeInfoWithID> storeInfo; // the whole storeInfo

    // shop comments structure
    public struct shopComments {
        // speaker info structure
        public DateTime time;
        public string content;
        // picture
        // like & hate rate
    }
    

    // event structure, size, array
    public struct eventInfo {
        public int num;
        public string title;
        public string content;
        public Sprite img;
    }
    static public int eventNum;
    static public eventInfo[] events;

    // choosing character
    public struct characterInfo {
        public int id;
        public string name;
        public string info;
        public Sprite imgForChoosing;
        public Sprite imgForHome;
        public Sprite imgForStreet;
        public Sprite imgForCollect;
    }
    static public characterInfo[] characters;
    static public int characterNum;





    // make sure only this script can stay on
    private void Awake()
    {

        if (gameInfoScript == null)
        {
            gameInfoScript = this;
            DontDestroyOnLoad(this);
        }
        else if (this != gameInfoScript)
        {
            Destroy(gameObject);
        }

        shop_props_size = 9;
        shop_clothes_size = 9;
        shop_furni_size = 9;

        eventNum = 2;
        characterNum = 2;
    }

    // Use this for initialization
    void Start () {
        fromHomeStreet = true; // false for shop, true for home

        

        props_info = new goods[shop_props_size];
        furni_info = new goods[shop_furni_size];

        setPropsInfo();
        setFurniInfo();

        events = new eventInfo[eventNum];
        setEventsInfo();

        characters = new characterInfo[characterNum];
        setCharactersInfo();

        // for example test
        setShopInfo();


    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void setPropsInfo() {
        for (int i = 0; i < shop_props_size; ++i) {
            props_info[i].id = i;
            props_info[i].category = shop_props_ctgrNum;
        }

        props_info[0].id = 0;
        props_info[0].name = "螢光菠菜汁";
        props_info[0].price = 50;
        props_info[0].info = "增加體力值，顯然不怎麼好喝。";
        props_info[0].img = Resources.Load<Sprite>("ImageSource/ShopItemImg/kusuli_strength");

        props_info[1].id = 1;
        props_info[1].name = "聰明水";
        props_info[1].price = 50;
        props_info[1].info = "增加智力值，看起來像普通的水，但你心中某處知道一定不是。";
        props_info[1].img = Resources.Load<Sprite>("ImageSource/ShopItemImg/kusuli_intelli");

        props_info[2].id = 2;
        props_info[2].name = "迷幻藥水";
        props_info[2].price = 100;
        props_info[2].info = "增加好感度，為了好感，就算是這種顏色我也...。";
        props_info[2].img = Resources.Load<Sprite>("ImageSource/ShopItemImg/kusuli_like");

        //Debug.Log("setPropsInfo done");
    }
   
    private void setFurniInfo()
    {
        for (int i = 0; i < shop_furni_size; ++i)
        {
            furni_info[i].id = i;
            furni_info[i].category = shop_furni_ctgrNum;
        }

        furni_info[0].id = 0;
        furni_info[0].name = "直立電扇";
        furni_info[0].price = 200;
        furni_info[0].info = "有了他，你再也不用頂著一身汗坐在房間中。";
        furni_info[0].img = Resources.Load<Sprite>("ImageSource/ShopItemImg/fan");

        furni_info[1].id = 1;
        furni_info[1].name = "冷氣";
        furni_info[1].price = 300;
        furni_info[1].info = "電費的主要來源，大家要愛地球^^";
        furni_info[1].img = Resources.Load<Sprite>("ImageSource/ShopItemImg/conditioner");

        //Debug.Log("setFurniInfo done");
    }

    // for testing example
    private void setShopInfo() {
        storeInfo = new List<storeInfoWithID>();

        // ------------------1

        storeInfoWithID info1 = new storeInfoWithID();

        storeInfoItem item1 = new storeInfoItem();
        item1.shopName = "私藏";
        item1.shopAddress = "育樂街的小赤佬旁邊";
        item1.infoList = new List<ColumnItem>();
        item1.openTime = new day[7];

        ColumnItem column1 = new ColumnItem();
        column1.title = "店家介紹";
        column1.content = "好喝手搖飲料店 店員解接很可愛 歡迎大家~~";
        item1.infoList.Add(column1);

        ColumnItem column3 = new ColumnItem();
        column3.title = "推薦飲品";
        column3.content = "奶霜系列超讚還有薰衣草綠茶!!";
        item1.infoList.Add(column3);

        ColumnItem column2 = new ColumnItem();
        column2.title = "店家聯絡方式";
        column2.content = "走進去點飲料~~";
        item1.infoList.Add(column2);
        
        item1.openTime[1].open = true;
        period period1_1 = new period();
        period1_1.begin_hr = "8";
        period1_1.begin_min = "30";
        period1_1.end_hr = "21";
        period1_1.end_min = "30";
        period period1_2 = new period();
        period1_2.begin_hr = "3";
        period1_2.begin_min = "30";
        period1_2.end_hr = "5";
        period1_2.end_min = "30";
        item1.openTime[1].timePeriod = new List<period>();
        item1.openTime[1].timePeriod.Add(period1_1);
        item1.openTime[1].timePeriod.Add(period1_2);

        info1.id = "001";
        info1.info = item1;

        storeInfo.Add(info1);

        // -----------------2

        storeInfoWithID info2 = new storeInfoWithID();

        storeInfoItem item2 = new storeInfoItem();
        item2.shopName = "南園街鍋貼";
        item2.shopAddress = "林森路ㄉ一條小巷彎進去就是南園街ㄌ";
        item2.infoList = new List<ColumnItem>();
        item2.openTime = new day[7];

        ColumnItem column2_1 = new ColumnItem();
        column2_1.title = "店家介紹";
        column2_1.content = "超大顆鍋貼 便當盒都蓋不起來\n會滿出來的鍋貼啊!!!!\n超級好吃讓人中毒\n就算每次買都要等15分鐘up也願意";
        item2.infoList.Add(column2_1);

        item2.openTime[1].open = true;
        period period2_1 = new period();
        period2_1.begin_hr = "10";
        period2_1.begin_min = "00";
        period2_1.end_hr = "16";
        period2_1.end_min = "00";
        
        item2.openTime[1].timePeriod = new List<period>();
        item2.openTime[1].timePeriod.Add(period2_1);

        info2.id = "002";
        info2.info = item2;

        storeInfo.Add(info2);

    }

    private void setEventsInfo() {
        events[0].num = 0;
        events[0].title = "黑熊君踩到面膜跌倒";
        events[0].content = "啊啊，好想變白啊... 蒐集完面膜以後，要開始積極的每日敷臉、敷手、敷身體、敷腳... 得做好全身美白才行!! 面膜面膜...面膜在哪裡呢...?? !!!!!!??? !!!!!! (碰!!!!) ....... 哎呀不小心踩到面膜滑倒了... 腳底也會美白到呢...";
        events[0].img = new Sprite();
        events[0].img = Resources.Load<Sprite>("ImageSource/EventsImg/bear_slip");
        if (events[0].img == null) { Debug.Log("no img"); }

        events[1].num = 1;
        events[1].title = "尋找鳳梨的路上突然被抓走";
        events[1].content = "咦咦咦咦???這個大叔是誰??? 為甚麼突然把我抓走??!!!! 什...什麼? 海鮮市場??? 不對啊!! 我已經熟了啊啊啊!!!!! 雖然其實我很好吃但是也不要吃我啊!!!";
        events[1].img = new Sprite();
        events[1].img = Resources.Load<Sprite>("ImageSource/EventsImg/shrimp_caught");
        if (events[1].img == null) { Debug.Log("no img"); }
        /*
        events[2].num = 2;
        events[2].title = "黑熊君踩到面膜跌倒2";
        events[2].content = "啊啊，好想變白啊... 蒐集完面膜以後，要開始積極的每日敷臉、敷手、敷身體、敷腳... 得做好全身美白才行!! 面膜面膜...面膜在哪裡呢...?? !!!!!!??? !!!!!! (碰!!!!) ....... 哎呀不小心踩到面膜滑倒了... 腳底也會美白到呢...";
        events[2].img = new Sprite();
        events[2].img = Resources.Load<Sprite>("ImageSource/EventsImg/bear_slip");
        if (events[2].img == null) { Debug.Log("no img"); }

        events[3].num = 3;
        events[3].title = "黑熊君踩到面膜跌倒3";
        events[3].content = "啊啊，好想變白啊... 蒐集完面膜以後，要開始積極的每日敷臉、敷手、敷身體、敷腳... 得做好全身美白才行!! 面膜面膜...面膜在哪裡呢...?? !!!!!!??? !!!!!! (碰!!!!) ....... 哎呀不小心踩到面膜滑倒了... 腳底也會美白到呢...";
        events[3].img = new Sprite();
        events[3].img = Resources.Load<Sprite>("ImageSource/EventsImg/bear_slip");
        if (events[3].img == null) { Debug.Log("no img"); }
        */
    }

    private void setCharactersInfo() {
        characters[0].id = 0;
        characters[0].name = "台灣黑熊";
        characters[0].info = "總而言之是一隻想變白的台灣黑熊。他沮喪的臉清楚的寫著天生黑肉底的鬱悶。\"啊啊，為甚麼白的只有鼻子附近和胸前的V呢...? \"他總是這樣問天問大地。敷面膜前都會把鼻子附近剪一個圓形的洞。喜歡邊敷臉邊在自家二樓室外泳池泡澡。";
        characters[0].imgForChoosing = Resources.Load<Sprite>("ImageSource/CharacterImg/TWBlackBear/Img_char_bear");
        characters[0].imgForHome = Resources.Load<Sprite>("ImageSource/CharacterImg/TWBlackBear/Img_char_bear0");
        characters[0].imgForStreet = Resources.Load<Sprite>("ImageSource/CharacterImg/TWBlackBear/Img_char_bear1");
        characters[0].imgForCollect = Resources.Load<Sprite>("ImageSource/CharacterImg/TWBlackBear/mask");

        characters[1].id = 1;
        characters[1].name = "蝦仁";
        characters[1].info = "是...是蝦仁?讓你納悶的角色。你開始懷疑他的身家由來，但他堅定的意志讓你只能答應幫他的忙。晶瑩剔透的蝦仁，看起來是高級的那種蝦仁，走在路上其實一直都被主婦/主夫們竊視著，真希望他能順利找到鳳梨啊...";
        characters[1].imgForChoosing = Resources.Load<Sprite>("ImageSource/CharacterImg/Shrimp/shrimp_choose");
        characters[1].imgForHome = Resources.Load<Sprite>("ImageSource/CharacterImg/Shrimp/shrimp_home");
        characters[1].imgForStreet = Resources.Load<Sprite>("ImageSource/CharacterImg/Shrimp/shrimp_street");
        characters[1].imgForCollect = Resources.Load<Sprite>("ImageSource/CharacterImg/Shrimp/pineapple");
    }


    
}
