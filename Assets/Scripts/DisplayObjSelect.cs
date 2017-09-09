using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayObjSelect : MonoBehaviour {

    private Button thisBtn;
    public Image bg;
    public GameObject scrollViewObj;
    private bool open;

	// Use this for initialization
	void Start () {
        open = false;
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(unfoldOrHide);
	}

    void unfoldOrHide() {
        if (open)
        {
            // to hide
            scrollViewObj.SetActive(false);
            bg.rectTransform.sizeDelta = new Vector2(bg.rectTransform.sizeDelta.x,180);
            open = false;
        }
        else {
            // to unfold
            bg.rectTransform.sizeDelta = new Vector2(bg.rectTransform.sizeDelta.x,677);
            scrollViewObj.SetActive(true);
            open = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
