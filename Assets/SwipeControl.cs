using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    private Vector3 frontObjPos; // for adjusting achievements' position

    public Material forBackObj;
    public Material defaultMaterial;

    public Button right;
    public Button left;

    private int adjustState; // 0 for middle front adjust, -1 for left-swipe, 1 for right-swipe

	// Use this for initialization
	void Start () {
        ac_num = 9;
        ac_pos = new Vector3[ac_num];
        ac_obj = new GameObject[ac_num];

        obj_x = new float[ac_num];
        obj_z = new float[ac_num];

        movingSpeed = 6;
        timeCounter = 0;
        circleWidth = 15;
        frontObjPos = new Vector3(0,0,-15);

        adjustState = 0;

        ac_obj = GameObject.FindGameObjectsWithTag("ac");
        for (int i = 0; i < ac_num; ++i) {
            ac_pos[i] = new Vector3((-1)*Mathf.Sin(2*Mathf.PI/ac_num*i)*circleWidth, 0f, (-1)*Mathf.Cos(2 * Mathf.PI / ac_num * i)*circleWidth);
            Debug.Log(ac_pos[i]);
            ac_obj[i].transform.position = ac_pos[i];
            obj_x[i] = 2 * Mathf.PI / ac_num * i;
            obj_z[i] = 2 * Mathf.PI / ac_num * i;
        }

        right.onClick.AddListener(moveRight);
        left.onClick.AddListener(moveLeft);

	}
	
	// Update is called once per frame
	void Update () {

        // for achievement behind to become darker
        foreach (GameObject obj in ac_obj) {
            if (obj.transform.position.z > -10.5)
            {
                obj.GetComponent<SpriteRenderer>().material = forBackObj;
            }
            else {
                obj.GetComponent<SpriteRenderer>().material = defaultMaterial;
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
