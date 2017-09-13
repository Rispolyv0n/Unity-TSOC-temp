using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on prefab "Image_charRecord"
// set btn function on each prefab & record text
// set storyPanel info

public class CharRecord : MonoBehaviour {

    public int charID;
    public int num_goodEnding;
    public int num_badEnding;
    public int num_strEnding;
    public Button btn_story;
    public Button btn_char;
    public Text char_record;

    private int totalNum;

    // obj in story Panel
    private GameObject storyPanel;
    private Image charImg;
    private Text charStory;

	// Use this for initialization
	void Start () {
        totalNum = num_goodEnding + num_badEnding + num_strEnding;
        btn_story.onClick.AddListener(openStoryPanel);
        btn_char.onClick.AddListener(openCharRecordScene);
        char_record.text = "共養成" + totalNum + "隻。快樂結局:" + num_goodEnding + "/ 難過結局:" + num_badEnding + "/??結局:" + num_strEnding;

    }

    void openStoryPanel() {
        storyPanel = this.transform.parent.parent.parent.parent.parent.GetChild(1).gameObject;
        storyPanel.SetActive(true);
        charImg = GameObject.FindGameObjectWithTag("img_char").GetComponent<Image>();
        charStory = GameObject.FindGameObjectWithTag("text_story").GetComponent<Text>();
        charImg.overrideSprite = GamingInfo.characters[charID].imgForHome;
        charStory.text = GamingInfo.characters[charID].info;
        // set story panel img & story
    }

    void openCharRecordScene() {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
