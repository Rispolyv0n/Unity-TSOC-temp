using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class RopeNCurtain : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler{

    private Button ropeBtn;
    public GameObject leftCurtain;
    public GameObject rightCurtain;
    public GameObject pullText;

    private bool dragging;
    private bool dragNrelease;

    private float draggingSpeed;
    private float risingSpeed;
    private float curtainSpeed;

    private float buffDistance;
    private float movingDistance;
    private Vector3 destinationPoint;

    private Vector3 leftCurtainDestinationPoint;
    private Vector3 rightCurtainDestinationPoint;
    

    // Use this for initialization
    void Start () {
        dragging = false;
        dragNrelease = false;
        //openCurtain = false;
        draggingSpeed = 100;
        risingSpeed = 2000;
        curtainSpeed = 500;
        buffDistance = 0;
        movingDistance = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y + buffDistance;
        destinationPoint = new Vector3(transform.position.x, transform.position.y + movingDistance, transform.position.z);
        leftCurtainDestinationPoint = new Vector3(leftCurtain.transform.position.x-leftCurtain.GetComponent<RectTransform>().sizeDelta.x-buffDistance, leftCurtain.transform.position.y, leftCurtain.transform.position.z);
        rightCurtainDestinationPoint = new Vector3(rightCurtain.transform.position.x + rightCurtain.GetComponent<RectTransform>().sizeDelta.x + buffDistance, rightCurtain.transform.position.y, rightCurtain.transform.position.z);
    }

    
	
	// Update is called once per frame
	void Update () {
        if (dragging)
        {
            transform.position += new Vector3(0, Input.GetAxis("Mouse Y") * draggingSpeed * Time.deltaTime, 0);
        }
        else if (dragNrelease) {
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint, risingSpeed*Time.deltaTime);
            leftCurtain.transform.position = Vector3.MoveTowards(leftCurtain.transform.position, leftCurtainDestinationPoint, curtainSpeed*Time.deltaTime);
            rightCurtain.transform.position = Vector3.MoveTowards(rightCurtain.transform.position, rightCurtainDestinationPoint, curtainSpeed * Time.deltaTime);
            
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
        pullText.SetActive(false);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        dragNrelease = true;
    }
}
