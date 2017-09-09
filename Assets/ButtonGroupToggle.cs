using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonGroupToggle : MonoBehaviour {

    public Button btn_move;
    public Button btn_spin;
    public Button btn_resize;

    public Sprite select;
    public Sprite unselect;

	// Use this for initialization
	void Start () {
        btn_move.onClick.AddListener(moveMode);
        btn_spin.onClick.AddListener(spinMode);
        btn_resize.onClick.AddListener(resizeMode);
        moveMode();

    }

    void moveMode() {
        btn_move.GetComponent<Image>().overrideSprite = select;
        btn_spin.GetComponent<Image>().overrideSprite = unselect;
        btn_resize.GetComponent<Image>().overrideSprite = unselect;
    }

    void spinMode() {
        btn_move.GetComponent<Image>().overrideSprite = unselect;
        btn_spin.GetComponent<Image>().overrideSprite = select;
        btn_resize.GetComponent<Image>().overrideSprite = unselect;
    }

    void resizeMode() {
        btn_move.GetComponent<Image>().overrideSprite = unselect;
        btn_spin.GetComponent<Image>().overrideSprite = unselect;
        btn_resize.GetComponent<Image>().overrideSprite = select;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
