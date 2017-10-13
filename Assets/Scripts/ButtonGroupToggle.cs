using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lean.Touch;

public class ButtonGroupToggle : MonoBehaviour {

    [HideInInspector]    
    public GameObject theStoreInfo;

    public Button btn_move;
    public Button btn_spin;
    public Button btn_resize;

    public Sprite select;
    public Sprite unselect;

	// Use this for initialization
	void Start () {

        theStoreInfo = GameObject.FindGameObjectWithTag("storeInfoObj");

        btn_move.onClick.AddListener(moveMode);
        btn_spin.onClick.AddListener(spinMode);
        btn_resize.onClick.AddListener(resizeMode);
        moveMode();

    }

    public void moveMode() {

        Debug.Log("press move!!!");

        theStoreInfo = GameObject.FindGameObjectWithTag("storeInfoObj");

        btn_move.GetComponent<Image>().overrideSprite = select;
        btn_spin.GetComponent<Image>().overrideSprite = unselect;
        btn_resize.GetComponent<Image>().overrideSprite = unselect;

        var scriptTranslate = theStoreInfo.GetComponent<LeanTranslate>();
        scriptTranslate.enabled = true;

        var scriptScale = theStoreInfo.GetComponent<LeanScale>();
        scriptScale.enabled = false;

        var scriptRotate = theStoreInfo.GetComponent<rotateMarker>();
        scriptRotate.enabled = false;

    }

    void spinMode() {

        Debug.Log("press rotate!!!");

        theStoreInfo = GameObject.FindGameObjectWithTag("storeInfoObj");

        btn_move.GetComponent<Image>().overrideSprite = unselect;
        btn_spin.GetComponent<Image>().overrideSprite = select;
        btn_resize.GetComponent<Image>().overrideSprite = unselect;

        var scriptTranslate = theStoreInfo.GetComponent<LeanTranslate>();
        scriptTranslate.enabled = false;

        var scriptScale = theStoreInfo.GetComponent<LeanScale>();
        scriptScale.enabled = false;

        var scriptRotate = theStoreInfo.GetComponent<rotateMarker>();
        scriptRotate.enabled = true;
    }

    void resizeMode() {

        Debug.Log("press resize!!!");

        theStoreInfo = GameObject.FindGameObjectWithTag("storeInfoObj");

        btn_move.GetComponent<Image>().overrideSprite = unselect;
        btn_spin.GetComponent<Image>().overrideSprite = unselect;
        btn_resize.GetComponent<Image>().overrideSprite = select;

        var scriptTranslate = theStoreInfo.GetComponent<LeanTranslate>();
        scriptTranslate.enabled = false;

        var scriptScale = theStoreInfo.GetComponent<LeanScale>();
        scriptScale.enabled = true;

        var scriptRotate = theStoreInfo.GetComponent<rotateMarker>();
        scriptRotate.enabled = false;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
