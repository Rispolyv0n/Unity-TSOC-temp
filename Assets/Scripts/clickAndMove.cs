using UnityEngine;
using System.Collections;


public class clickAndMove : MonoBehaviour {

    public Vector3 targetPoint;
    private int scalingP = 1;
    // Use this for initialization
    private void Awake()
    {
        targetPoint = transform.position;
    }
    void Start () {
        targetPoint = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
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
	}
}
