using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lean.Touch;

public class ButtonGroupToggle : MonoBehaviour {

    [HideInInspector]    
    public GameObject cube_storeInfo;

    public Button btn_move;
    public Button btn_spin;
    public Button btn_resize;

    public Sprite select;
    public Sprite unselect;

	// Use this for initialization
	void Start () {

        cube_storeInfo = GameObject.FindGameObjectWithTag("cube_storeInfo");

        btn_move.onClick.AddListener(moveMode);
        btn_spin.onClick.AddListener(spinMode);
        btn_resize.onClick.AddListener(resizeMode);
        moveMode();

    }

    void moveMode() {
        
        cube_storeInfo = GameObject.FindGameObjectWithTag("cube_storeInfo");

        btn_move.GetComponent<Image>().overrideSprite = select;
        btn_spin.GetComponent<Image>().overrideSprite = unselect;
        btn_resize.GetComponent<Image>().overrideSprite = unselect;

        var scriptTranslate = cube_storeInfo.GetComponent<LeanTranslate>();
        scriptTranslate.enabled = true;

        var scriptScale = cube_storeInfo.GetComponent<LeanScale>();
        scriptScale.enabled = false;

        var scriptRotate = cube_storeInfo.GetComponent<rotateMarker>();
        scriptRotate.enabled = false;
        
    }

    void spinMode() {
        
        cube_storeInfo = GameObject.FindGameObjectWithTag("cube_storeInfo");

        btn_move.GetComponent<Image>().overrideSprite = unselect;
        btn_spin.GetComponent<Image>().overrideSprite = select;
        btn_resize.GetComponent<Image>().overrideSprite = unselect;

        var scriptTranslate = cube_storeInfo.GetComponent<LeanTranslate>();
        scriptTranslate.enabled = false;

        var scriptScale = cube_storeInfo.GetComponent<LeanScale>();
        scriptScale.enabled = false;

        var scriptRotate = cube_storeInfo.GetComponent<rotateMarker>();
        scriptRotate.enabled = true;
    }

    void resizeMode() {
        
        cube_storeInfo = GameObject.FindGameObjectWithTag("cube_storeInfo");

        btn_move.GetComponent<Image>().overrideSprite = unselect;
        btn_spin.GetComponent<Image>().overrideSprite = unselect;
        btn_resize.GetComponent<Image>().overrideSprite = select;

        var scriptTranslate = cube_storeInfo.GetComponent<LeanTranslate>();
        scriptTranslate.enabled = false;

        var scriptScale = cube_storeInfo.GetComponent<LeanScale>();
        scriptScale.enabled = true;

        var scriptRotate = cube_storeInfo.GetComponent<rotateMarker>();
        scriptRotate.enabled = false;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
