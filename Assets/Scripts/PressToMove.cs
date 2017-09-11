using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PressToMove : MonoBehaviour {

    private Image objToMove;
    public Image imgFront;
    public Sprite img;
    private float speed;

	// Use this for initialization
	void Start () {
        objToMove = GetComponent<Image>();
        speed = 7;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0)) {
            imgFront.overrideSprite = img;
            objToMove.transform.localPosition -= new Vector3(0f,speed,0f);
        }
	}
}
