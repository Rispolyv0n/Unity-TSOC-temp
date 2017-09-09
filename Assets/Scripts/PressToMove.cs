using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PressToMove : MonoBehaviour {

    private Image objToMove;
    public Image imgFront;
    public Sprite img;
    public Image img_bearFace;
    public Sprite sprite_bearFace;
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
            img_bearFace.overrideSprite = sprite_bearFace;
            objToMove.transform.position -= new Vector3(0f,speed,0f);
        }
	}
}
