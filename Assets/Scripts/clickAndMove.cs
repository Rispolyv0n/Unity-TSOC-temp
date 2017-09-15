using UnityEngine;
using System.Collections;


public class clickAndMove : MonoBehaviour {

    public float upperBound;
    public float lowerBound;
    public float rightBound;
    public float leftBound;

    private Vector3 targetPoint;
    private int scalingP = 1;
    // Use this for initialization
    void Start () {
        targetPoint = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        /*
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, 140*Time.deltaTime);
        if (Input.GetMouseButtonDown(0)) {
            targetPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,600));
            if (targetPoint.y > 474) {
                targetPoint.y = 474;
            }
            targetPoint.z = transform.position.z;
            if (targetPoint.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
        */
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, 140 * Time.deltaTime);
        if (Input.GetMouseButtonDown(0))
        {
            //thisImg.transform.position = Input.mousePosition;
            targetPoint = Input.mousePosition;
            if (targetPoint.y > upperBound)
            {
                targetPoint.y = upperBound;
            }
            if (targetPoint.y < lowerBound)
            {
                targetPoint.y = lowerBound;
            }
            if (targetPoint.x > rightBound)
            {
                targetPoint.x = rightBound;
            }
            if (targetPoint.x < leftBound)
            {
                targetPoint.x = leftBound;
            }
            if (targetPoint.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
    }
}
