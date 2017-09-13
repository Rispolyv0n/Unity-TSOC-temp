using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on "panel_settings" in the scene "home"
// control the setting_background expanding and btns showing

public class UnfoldSettings : MonoBehaviour {

    private Button settingBtn;

    public GameObject panelObj;
    public Image bg;
    public GameObject btn_1;
    public GameObject btn_2;
    public GameObject btn_3;

    private bool isOpen;

	// Use this for initialization
	void Start () {
        isOpen = false;
        settingBtn = GetComponent<Button>();
        settingBtn.onClick.AddListener(ifUnfold);
	}

    public void ifUnfold()
    {
        if (isOpen)
        {
            btn_1.SetActive(false);
            btn_2.SetActive(false);
            btn_3.SetActive(false);
            bg.rectTransform.sizeDelta = new Vector2(bg.rectTransform.sizeDelta.x, 25);
            panelObj.SetActive(false);
            isOpen = false;
        }
        else {
            panelObj.SetActive(true);
            bg.rectTransform.sizeDelta = new Vector2(bg.rectTransform.sizeDelta.x, 264);
            btn_1.SetActive(true);
            btn_2.SetActive(true);
            btn_3.SetActive(true);
            isOpen = true;
        }
        
    }

    // Update is called once per frame
    void Update () {
	
	}
}
