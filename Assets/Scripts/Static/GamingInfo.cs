using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GamingInfo : MonoBehaviour
{

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


    /*
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
*/

    public class period
    {
        public string begin_hr;
        public string begin_min;
        public string end_hr;
        public string end_min;
    }

    public class day
    {
        public bool open = new bool();
        public List<period> timePeriod = new List<period>();
    }

    public class column
    {
        public string title;
        public string content;
    }

    public class oneInfo
    {
        public string _id;
        public string shopName;
        public string shopAddress;
        public List<column> infoList = new List<column>();
        public day[] openTime = new day[7];
    }


    // shop comments structure
    public struct shopComments
    {
        // speaker info structure
        public DateTime time;
        public string content;
        // picture
        // like & hate rate
    }


    // event structure, size, array
    public struct eventInfo
    {
        public int num;
        public string title;
        public string content;
        public Sprite img;
    }
    static public int eventNum;
    static public eventInfo[] events;

    // choosing character
    public struct characterInfo
    {
        public int id;
        public string name;
        public string info;
        public Sprite imgForChoosing;
        public Sprite imgForHome;
        public Sprite imgForStreet;
        public Sprite imgForCollect;
        public string endingContent_good;
        public string endingContent_bad;
        public string endingContent_str;
        public Sprite endingImg_good;
        public Sprite endingImg_bad;
        public Sprite endingImg_str;
    }
    static public characterInfo[] characters;
    static public int characterNum;

    // achievements
    public struct achievementInfo
    {
        public int id;
        public int category; // 0 for char, 1 for event, 2 for others
        public int relative_id;
        public int condition_1;
        public int condition_2;
        public int condition_3;
        public string title;
        public string content_1;
        public string content_2;
        public string content_3;
        public Sprite img_1;
        public Sprite img_2;
        public Sprite img_3;
        public string coupon_1;
        public string coupon_2;
        public string coupon_3;
    }
    static public achievementInfo[] achievements;
    static public int achievementNum;

    // level up maxValue of three values
    static public float maxValue_lv1;
    static public float maxValue_lv2;
    static public float maxValue_lv3;

    // level up threshold
    static public int totalPoints_toLv2;
    static public int totalPoints_toLv3;
    static public int totalPoints_toDone;

    // ratio of different endings
    static public int likePoints_lowerBoundary;
    static public int likePoints_higherBoundary;
    static public float ratio_low_bad;
    static public float ratio_low_strange;
    static public float ratio_mid_bad;
    static public float ratio_mid_strange;
    static public float ratio_mid_good;
    static public float ratio_high_strange;
    static public float ratio_high_good;


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
        achievementNum = 5;

        // level up maxValue
        maxValue_lv1 = 100;
        maxValue_lv2 = 200;
        maxValue_lv3 = 300;

        // level up theshold
        totalPoints_toLv2 = 200;
        totalPoints_toLv3 = 400;
        totalPoints_toDone = 700;

        // ratio of different endings
        likePoints_lowerBoundary = 120;
        likePoints_higherBoundary = 220;
        ratio_low_bad = 0.7f;
        ratio_low_strange = 0.3f;
        ratio_mid_bad = 0.2f;
        ratio_mid_strange = 0.4f;
        ratio_mid_good = 0.4f;
        ratio_high_strange = 0.2f;
        ratio_high_good = 0.8f;
    }

    // Use this for initialization
    void Start()
    {
        fromHomeStreet = true; // false for shop, true for home



        props_info = new goods[shop_props_size];
        furni_info = new goods[shop_furni_size];

        setPropsInfo();
        setFurniInfo();

        events = new eventInfo[eventNum];
        setEventsInfo();

        characters = new characterInfo[characterNum];
        setCharactersInfo();

        achievements = new achievementInfo[achievementNum];
        setAchievementsInfo();

        // for example test
        //setShopInfo();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setPropsInfo()
    {
        for (int i = 0; i < shop_props_size; ++i)
        {
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
        furni_info[1].info = "電費的主要來源，大家要愛地球^^喔咿喔咿喔咿喔咿喔咿喔咿喔";
        furni_info[1].img = Resources.Load<Sprite>("ImageSource/ShopItemImg/conditioner");

        //Debug.Log("setFurniInfo done");
    }

    /*
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
    */

    private void setEventsInfo()
    {
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

    private void setCharactersInfo()
    {
        characters[0].id = 0;
        characters[0].name = "台灣黑熊";
        characters[0].info = "總而言之是一隻想變白的台灣黑熊。他沮喪的臉清楚的寫著天生黑肉底的鬱悶。\"啊啊，為甚麼白的只有鼻子附近和胸前的V呢...? \"他總是這樣問天問大地。敷面膜前都會把鼻子附近剪一個圓形的洞。喜歡邊敷臉邊在自家二樓室外泳池泡澡。";
        characters[0].imgForChoosing = Resources.Load<Sprite>("ImageSource/CharacterImg/TWBlackBear/Img_char_bear");
        characters[0].imgForHome = Resources.Load<Sprite>("ImageSource/CharacterImg/TWBlackBear/Img_char_bear0");
        characters[0].imgForStreet = Resources.Load<Sprite>("ImageSource/CharacterImg/TWBlackBear/Img_char_bear1");
        characters[0].imgForCollect = Resources.Load<Sprite>("ImageSource/CharacterImg/TWBlackBear/mask");
        characters[0].endingContent_good = "成功美白! 雖然這種程度真的是面膜敷得出來的嗎...根本已經是北極熊了吧... 總之他看起來很滿意呢";
        characters[0].endingContent_bad = "在街上到處走來走去找面膜結果曬得更黑了啊... 居然比鼻子的顏色還要黑了... 不要沮喪啊ˊ__ˋ";
        characters[0].endingContent_str = "啊啊，不要誤會，我不是你們的吉祥物... 等等...等等啊!! 啊...要發完傳單才能走嗎... 請問可以站在有陰影的地方嗎...";
        characters[0].endingImg_good = Resources.Load<Sprite>("ImageSource/CharacterImg/TWBlackBear/img_goodEnding");
        characters[0].endingImg_bad = Resources.Load<Sprite>("ImageSource/CharacterImg/TWBlackBear/img_badEnding");
        characters[0].endingImg_str = Resources.Load<Sprite>("ImageSource/CharacterImg/TWBlackBear/img_strEnding");

        characters[1].id = 1;
        characters[1].name = "蝦仁";
        characters[1].info = "是...是蝦仁?讓你納悶的角色。你開始懷疑他的身家由來，但他堅定的意志讓你只能答應幫他的忙。晶瑩剔透的蝦仁，看起來是高級的那種蝦仁，走在路上其實一直都被主婦/主夫們竊視著，真希望他能順利找到鳳梨啊...";
        characters[1].imgForChoosing = Resources.Load<Sprite>("ImageSource/CharacterImg/Shrimp/shrimp_choose");
        characters[1].imgForHome = Resources.Load<Sprite>("ImageSource/CharacterImg/Shrimp/shrimp_home");
        characters[1].imgForStreet = Resources.Load<Sprite>("ImageSource/CharacterImg/Shrimp/shrimp_street");
        characters[1].imgForCollect = Resources.Load<Sprite>("ImageSource/CharacterImg/Shrimp/pineapple");
        characters[1].endingContent_good = "高級料理! 總算找到鳳梨了，我們就是該被做成一盤鳳梨蝦仁啊!";
        characters[1].endingContent_bad = "奇...奇怪... 被丟到水裡了?? ...被勾起來了!!??? 不要把我放進釣蝦場啊!!!!";
        characters[1].endingContent_str = "今日特餐是義大利料理，那麼就為您獻上一份夏威夷披薩佐蝦仁!";
        characters[1].endingImg_good = Resources.Load<Sprite>("ImageSource/CharacterImg/Shrimp/img_goodEnding");
        characters[1].endingImg_bad = Resources.Load<Sprite>("ImageSource/CharacterImg/Shrimp/img_badEnding");
        characters[1].endingImg_str = Resources.Load<Sprite>("ImageSource/CharacterImg/Shrimp/img_strEnding");
    }

    private void setAchievementsInfo()
    {
        achievements[0].id = 0;
        achievements[0].category = 0;
        achievements[0].relative_id = 0;
        achievements[0].condition_1 = 1;
        achievements[0].condition_2 = 5;
        achievements[0].condition_3 = 10;
        achievements[0].title = "黑熊蒐集家";
        achievements[0].content_1 = "蒐集1隻台灣黑熊。";
        achievements[0].content_2 = "蒐集5隻台灣黑熊。";
        achievements[0].content_3 = "蒐集10隻台灣黑熊。";
        achievements[0].coupon_1 = "店家A優惠_1。";
        achievements[0].coupon_2 = "店家A優惠_2。";
        achievements[0].coupon_3 = "店家A優惠_3。";
        achievements[0].img_1 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_bear_1");
        achievements[0].img_2 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_bear_2");
        achievements[0].img_3 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_bear_3");

        achievements[1].id = 1;
        achievements[1].category = 0;
        achievements[1].relative_id = 1;
        achievements[1].condition_1 = 1;
        achievements[1].condition_2 = 5;
        achievements[1].condition_3 = 10;
        achievements[1].title = "鳳梨蝦仁愛好者";
        achievements[1].content_1 = "蒐集1隻蝦仁。";
        achievements[1].content_2 = "蒐集5隻蝦仁。";
        achievements[1].content_3 = "蒐集10隻蝦仁。";
        achievements[1].coupon_1 = "店家B優惠_1。";
        achievements[1].coupon_2 = "店家B優惠_2。";
        achievements[1].coupon_3 = "店家B優惠_3。";
        achievements[1].img_1 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_shrimp_1");
        achievements[1].img_2 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_shrimp_2");
        achievements[1].img_3 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_shrimp_3");

        achievements[2].id = 2;
        achievements[2].category = 1;
        achievements[2].relative_id = 0;
        achievements[2].condition_1 = 1;
        achievements[2].condition_2 = 10;
        achievements[2].condition_3 = 20;
        achievements[2].title = "踩面膜達人";
        achievements[2].content_1 = "蒐集1個\"黑熊君踩到面膜跌倒\"事件。";
        achievements[2].content_2 = "蒐集10個\"黑熊君踩到面膜跌倒\"事件。";
        achievements[2].content_3 = "蒐集20個\"黑熊君踩到面膜跌倒\"事件。";
        achievements[2].coupon_1 = "店家C優惠_1。";
        achievements[2].coupon_2 = "店家C優惠_2。";
        achievements[2].coupon_3 = "店家C優惠_3。";
        achievements[2].img_1 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_stampOnMask_1");
        achievements[2].img_2 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_stampOnMask_2");
        achievements[2].img_3 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_stampOnMask_3");

        achievements[3].id = 3;
        achievements[3].category = 1;
        achievements[3].relative_id = 1;
        achievements[3].condition_1 = 1;
        achievements[3].condition_2 = 10;
        achievements[3].condition_3 = 20;
        achievements[3].title = "被抓走達人";
        achievements[3].content_1 = "蒐集1個\"尋找鳳梨的路上突然被抓走\"事件。";
        achievements[3].content_2 = "蒐集10個\"尋找鳳梨的路上突然被抓走\"事件。";
        achievements[3].content_3 = "蒐集20個\"尋找鳳梨的路上突然被抓走\"事件。";
        achievements[3].coupon_1 = "店家D優惠_1。";
        achievements[3].coupon_2 = "店家D優惠_2。";
        achievements[3].coupon_3 = "店家D優惠_3。";
        achievements[3].img_1 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_getCaught_1");
        achievements[3].img_2 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_getCaught_2");
        achievements[3].img_3 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_getCaught_3");

        achievements[4].id = 4;
        achievements[4].category = 2;
        achievements[4].relative_id = 0;
        achievements[4].condition_1 = 24;
        achievements[4].condition_2 = 168;
        achievements[4].condition_3 = 336;
        achievements[4].title = "成癮者";
        achievements[4].content_1 = "遊戲時間滿24小時。";
        achievements[4].content_2 = "遊戲時間滿168小時。";
        achievements[4].content_3 = "遊戲時間滿336小時。";
        achievements[4].coupon_1 = "店家E優惠_1。";
        achievements[4].coupon_2 = "店家E優惠_2。";
        achievements[4].coupon_3 = "店家E優惠_3。";
        achievements[4].img_1 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_addicted_1");
        achievements[4].img_2 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_addicted_2");
        achievements[4].img_3 = Resources.Load<Sprite>("ImageSource/BackgroundImage/Achievement/ac_addicted_3");
    }



}
