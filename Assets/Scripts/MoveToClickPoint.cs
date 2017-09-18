using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveToClickPoint : MonoBehaviour{

    private Image thisImg;
    private Vector3 targetPoint;

	// Use this for initialization
	void Start () {
        thisImg = GetComponent<Image>();
        targetPoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, 140 * Time.deltaTime);
        if (Input.GetMouseButtonDown(0)) {
            //thisImg.transform.position = Input.mousePosition;
            targetPoint = Input.mousePosition;
            if (targetPoint.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
        
	}
}
