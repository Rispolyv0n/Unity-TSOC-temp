using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on the empty object in the scene "achievement"
// set every ac's position and img, decide if the user can see the ac
// control the swiping, get and display the info of each ac in the front

public class SwipeControl : MonoBehaviour {

    private int ac_num; // numbers of achievements
    private Vector3[] ac_pos;
    private GameObject[] ac_obj;

    private Vector3 mouseDownPos;
    private Vector3 mouseUpPos;

    private bool swipeDir; // true for right, false for left;

    private float movingSpeed;
    private float timeCounter;
    private float circleWidth;

    private float[] obj_x;
    private float[] obj_z;

    private bool[] hasIt;

    private Vector3 frontObjPos; // for adjusting achievements' position

    public Material forBackObj;
    public Material defaultMaterial;

    public Button right;
    public Button left;

    public GameObject camera_obj;

    private float distance_cameraNobj;
    private int adjustState; // 0 for middle front adjust, -1 for left-swipe, 1 for right-swipe

    public Text ac_title;
    public Text ac_contentTitle;
    public Text ac_content;
    public Text ac_couponTitle;
    public Text ac_coupon;
    public GameObject text_lock;
    public GameObject text_info;

	// Use this for initialization
	void Start () {
        ac_num = 5;
        ac_pos = new Vector3[ac_num];
        ac_obj = new GameObject[ac_num];

        obj_x = new float[ac_num];
        obj_z = new float[ac_num];

        hasIt = new bool[ac_num];

        distance_cameraNobj = 17; // original 15
        movingSpeed = 4;
        timeCounter = 0;
        circleWidth = ac_num/3*5;
        frontObjPos = new Vector3(0,0,-circleWidth);

        camera_obj.transform.position = new Vector3(camera_obj.transform.position.x, camera_obj.transform.position.y, -circleWidth-distance_cameraNobj);

        adjustState = 0;

        ac_obj = GameObject.FindGameObjectsWithTag("ac");
        for (int i = 0; i < ac_num; ++i) {
            // initial setting
            ac_pos[i] = new Vector3((-1)*Mathf.Sin(2*Mathf.PI/ac_num*i)*circleWidth, 0f, (-1)*Mathf.Cos(2 * Mathf.PI / ac_num * i)*circleWidth);
            ac_obj[i].transform.position = ac_pos[i];
            obj_x[i] = 2 * Mathf.PI / ac_num * i;
            obj_z[i] = 2 * Mathf.PI / ac_num * i;

            // set img of each achievement, check if user has got the achievement, and decide which img to render
            
            hasIt[i] = false;
            foreach (PlayerInfo.achievementItem item in PlayerInfo.achievementCollection) {
                if (item.id == i)
                {
                    switch (item.level)
                    {
                        case 1:
                            ac_obj[i].GetComponent<Image>().overrideSprite = GamingInfo.achievements[i].img_1;
                            //ac_obj[i].GetComponent<SpriteRenderer>().sprite = GamingInfo.achievements[i].img_1;
                            break;
                        case 2:
                            ac_obj[i].GetComponent<Image>().overrideSprite = GamingInfo.achievements[i].img_2;
                            //ac_obj[i].GetComponent<SpriteRenderer>().sprite = GamingInfo.achievements[i].img_2;
                            break;
                        case 3:
                            ac_obj[i].GetComponent<Image>().overrideSprite = GamingInfo.achievements[i].img_3;
                            //ac_obj[i].GetComponent<SpriteRenderer>().sprite = GamingInfo.achievements[i].img_3;
                            break;
                    }
                    hasIt[i] = true;
                    break;
                }
            }
            if (!hasIt[i]) {
                ac_obj[i].GetComponent<Image>().overrideSprite = GamingInfo.achievements[i].img_1;
                //ac_obj[i].GetComponent<SpriteRenderer>().sprite = GamingInfo.achievements[i].img_1;
            } 
        }
        adjustPosition(0);

        right.onClick.AddListener(moveRight);
        left.onClick.AddListener(moveLeft);

	}
	
	// Update is called once per frame
	void Update () {
        
        // for achievement behind to become darker
        for (int i=0;i<ac_num;++i) {
            if (ac_obj[i].transform.position.z > camera_obj.transform.position.z + distance_cameraNobj + 1)
            {
                //ac_obj[i].GetComponent<SpriteRenderer>().material = forBackObj;
            }
            else if (hasIt[i])
            {
                //ac_obj[i].GetComponent<SpriteRenderer>().material = defaultMaterial;
            }
            else {
                //ac_obj[i].GetComponent<SpriteRenderer>().material = forBackObj;
            }
        }

        // when mouse pressed, move right or move left
        if (Input.GetMouseButton(0)) {
            timeCounter = Time.deltaTime * movingSpeed * Input.GetAxis("Mouse X")*(-1);
            
            for (int i = 0; i < ac_num; ++i) {
                obj_x[i] += timeCounter;
                obj_z[i] += timeCounter;
                ac_obj[i].transform.position = new Vector3((-1) * Mathf.Sin(obj_x[i]) * circleWidth, 0f, (-1) * Mathf.Cos(obj_z[i]) * circleWidth);
            }
        }

        // when mouse released, adjust achievements' position, let the nearest one is in the middle front
        if (Input.GetMouseButtonUp(0)) {
            adjustPosition(adjustState);
        }
        
    }


    // for the front move left&right btns
    void moveRight() {
        adjustState = 1;
    }
    void moveLeft() {
        adjustState = -1;
    }


    void adjustPosition(int state) {
        float thisObjDistance = 0;
        float minDistance = circleWidth + 1;
        int theClosestObjNum = 0;

        // calculate the nearest achievement from the front
        for (int i = 0; i < ac_num; ++i)
        {
            thisObjDistance = Vector3.Distance(frontObjPos, ac_obj[i].transform.position);
            if (thisObjDistance < minDistance)
            {
                minDistance = thisObjDistance;
                theClosestObjNum = i;
            }
        }
        
        // check if btn are clicked
        if (state > 0)
        {
            --theClosestObjNum;
            if (theClosestObjNum < 0) theClosestObjNum = ac_num - 1;
        }
        else if (state < 0)
        {
            ++theClosestObjNum;
            if (theClosestObjNum > ac_num - 1) theClosestObjNum = 0;
        }

        // display info
        if (hasIt[theClosestObjNum])
        {
            text_lock.SetActive(false);
            text_info.SetActive(true);

            ac_title.text = GamingInfo.achievements[theClosestObjNum].title;
            ac_contentTitle.text = "解鎖成就內容\n";
            ac_couponTitle.text = "解鎖商家優惠\n";
            int level = 0;
            foreach (PlayerInfo.achievementItem item in PlayerInfo.achievementCollection)
            {
                if (item.id == theClosestObjNum)
                {
                    level = item.level;
                    break;
                }
            }
            switch (level)
            {
                case 1:
                    ac_content.text = GamingInfo.achievements[theClosestObjNum].content_1;
                    ac_coupon.text = GamingInfo.achievements[theClosestObjNum].coupon_1;
                    break;
                case 2:
                    ac_content.text = GamingInfo.achievements[theClosestObjNum].content_2;
                    ac_coupon.text = GamingInfo.achievements[theClosestObjNum].coupon_2;
                    break;
                case 3:
                    ac_content.text = GamingInfo.achievements[theClosestObjNum].content_3;
                    ac_coupon.text = GamingInfo.achievements[theClosestObjNum].coupon_3;
                    break;
            }

        }
        else
        {
            text_lock.SetActive(true);
            text_info.SetActive(false);
        }

        // set other achievements' position
        int correspondPos;
        for (int i = 0; i < ac_num; ++i)
        {
            if (i - theClosestObjNum < 0)
            {
                correspondPos = i - theClosestObjNum + ac_num;
            }
            else
            {
                correspondPos = i - theClosestObjNum;
            }

            ac_obj[i].transform.position = ac_pos[correspondPos];
            obj_x[i] = 2 * Mathf.PI / ac_num * correspondPos;
            obj_z[i] = 2 * Mathf.PI / ac_num * correspondPos;
        }

        adjustState = 0;
        
    }
}
