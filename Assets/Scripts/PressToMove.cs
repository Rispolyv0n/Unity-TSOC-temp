using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PressToMove : MonoBehaviour {

    private Image objToMove;
    public Image imgFront;
    public Sprite img;
    public Image img_bearFace;
    public Sprite sprite_bearFace;
    private int speed;

	// Use this for initialization
	void Start () {
        Random.seed = System.Guid.NewGuid().GetHashCode();
        objToMove = GetComponent<Image>();
        speed = Random.Range(-2, 3); // -2, -1, 0, 1, 2
        speed *= 2; // -4, -2, 0, 2, 4
        speed += 7; // speed range = 3, 5, 7, 9, 11
        Debug.Log(speed);
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
