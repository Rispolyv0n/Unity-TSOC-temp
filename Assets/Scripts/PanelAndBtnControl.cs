using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// tell if the user has gone home, decide whether to show the instruction

public class PanelAndBtnControl : MonoBehaviour {

    public GameObject panel_one;
    public GameObject panel_three;

    public Button btn_one;
    public Button btn_three;

    public Sprite img_one_A;
    public Sprite img_one_B;
    public Sprite img_three_A;
    public Sprite img_three_B;

    // Use this for initialization
    void Start () {
        if (PlayerInfo.firstGoHome)
        {
            Debug.Log("in shop:"+PlayerInfo.firstGoHome);
            SceneManager.LoadScene("instruction_home");
        }else if (PlayerInfo.currentCharacterID < 0)
        {
            SceneManager.LoadScene("choose_char");
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void viewPanelOne() {
        panel_one.SetActive(true);
        panel_three.SetActive(false);

        btn_one.image.overrideSprite = img_one_A;
        btn_three.image.overrideSprite = img_three_B;
    }
    
    public void viewPanelThree() {
        panel_one.SetActive(false);
        panel_three.SetActive(true);

        btn_one.image.overrideSprite = img_one_B;
        btn_three.image.overrideSprite = img_three_A;
    }
}
