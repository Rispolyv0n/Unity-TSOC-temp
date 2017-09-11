using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DetectMouseUp : MonoBehaviour {

    public GameObject ObjToShow;
    private GameObject thisDetectingObj;

    public Image imgFront;
    public Sprite img;

    // Use this for initialization
    void Start () {
        thisDetectingObj = GetComponent<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0)) {
            imgFront.overrideSprite = img;
            ObjToShow.SetActive(true);
            //Destroy(this);
        }
	}
}
