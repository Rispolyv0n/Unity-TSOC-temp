using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lean.Touch;

public class ButtonGroupToggle : MonoBehaviour {

    [HideInInspector]    
    public GameObject storeInfo;

    public Button btn_move;
    public Button btn_spin;
    public Button btn_resize;

    public Sprite select;
    public Sprite unselect;

	// Use this for initialization
	void Start () {

        storeInfo = GameObject.FindGameObjectWithTag("storeInfoObj");

        btn_move.onClick.AddListener(moveMode);
        btn_spin.onClick.AddListener(spinMode);
        btn_resize.onClick.AddListener(resizeMode);
        moveMode();

    }

    void moveMode() {
        
        storeInfo = GameObject.FindGameObjectWithTag("storeInfoObj");

        btn_move.GetComponent<Image>().overrideSprite = select;
        btn_spin.GetComponent<Image>().overrideSprite = unselect;
        btn_resize.GetComponent<Image>().overrideSprite = unselect;

        var scriptTranslate = storeInfo.GetComponent<LeanTranslate>();
        scriptTranslate.enabled = true;

        var scriptScale = storeInfo.GetComponent<LeanScale>();
        scriptScale.enabled = false;

        var scriptRotate = storeInfo.GetComponent<rotateMarker>();
        scriptRotate.enabled = false;
        
    }

    void spinMode() {
        
        storeInfo = GameObject.FindGameObjectWithTag("storeInfoObj");

        btn_move.GetComponent<Image>().overrideSprite = unselect;
        btn_spin.GetComponent<Image>().overrideSprite = select;
        btn_resize.GetComponent<Image>().overrideSprite = unselect;

        var scriptTranslate = storeInfo.GetComponent<LeanTranslate>();
        scriptTranslate.enabled = false;

        var scriptScale = storeInfo.GetComponent<LeanScale>();
        scriptScale.enabled = false;

        var scriptRotate = storeInfo.GetComponent<rotateMarker>();
        scriptRotate.enabled = true;
    }

    void resizeMode() {
        
        storeInfo = GameObject.FindGameObjectWithTag("storeInfoObj");

        btn_move.GetComponent<Image>().overrideSprite = unselect;
        btn_spin.GetComponent<Image>().overrideSprite = unselect;
        btn_resize.GetComponent<Image>().overrideSprite = select;

        var scriptTranslate = storeInfo.GetComponent<LeanTranslate>();
        scriptTranslate.enabled = false;

        var scriptScale = storeInfo.GetComponent<LeanScale>();
        scriptScale.enabled = true;

        var scriptRotate = storeInfo.GetComponent<rotateMarker>();
        scriptRotate.enabled = false;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
