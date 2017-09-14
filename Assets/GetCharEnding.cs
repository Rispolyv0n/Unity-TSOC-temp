using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

// attach on the empty object in the finishChar scene
// decide the ending of the char, get the ending info and display

public class GetCharEnding : MonoBehaviour {

    public Image endingImg;
    public Text endingTitle;
    public Text endingContent;

    public Button btn_chooseNextChar;
    public Button btn_toStreet;

    private float value_like;
    private int whichEnding;

	// Use this for initialization
	void Start () {
        value_like = GamingDetect.temp_value_like;

        // decide the ending
        UnityEngine.Random.seed = System.Guid.NewGuid().GetHashCode();
        
        if (value_like < GamingInfo.likePoints_lowerBoundary)
        {
            float getEnding = UnityEngine.Random.Range(0f, 1f);
            if (getEnding < GamingInfo.ratio_low_strange)
            {
                whichEnding = PlayerInfo.ENDING_STR;
            }
            else {
                whichEnding = PlayerInfo.ENDING_BAD;
            }
        }
        else if (value_like >= GamingInfo.likePoints_lowerBoundary && value_like < GamingInfo.likePoints_higherBoundary)
        {
            float getEnding = UnityEngine.Random.Range(0f, 1f);
            if (getEnding < GamingInfo.ratio_mid_bad)
            {
                whichEnding = PlayerInfo.ENDING_BAD;
            }
            else if (getEnding < GamingInfo.ratio_mid_bad + GamingInfo.ratio_mid_strange)
            {
                whichEnding = PlayerInfo.ENDING_STR;
            }
            else {
                whichEnding = PlayerInfo.ENDING_GOOD;
            }
        }
        else {
            float getEnding = UnityEngine.Random.Range(0f, 1f);
            if (getEnding < GamingInfo.ratio_high_strange)
            {
                whichEnding = PlayerInfo.ENDING_STR;
            }
            else
            {
                whichEnding = PlayerInfo.ENDING_GOOD;
            }
        }

        // get ending info
        switch (whichEnding) {
            case PlayerInfo.ENDING_GOOD:
                endingTitle.text = "養育成功";
                endingImg.overrideSprite = GamingInfo.characters[PlayerInfo.currentCharacterID].endingImg_good;
                endingContent.text = GamingInfo.characters[PlayerInfo.currentCharacterID].endingContent_good;
                break;
            case PlayerInfo.ENDING_BAD:
                endingTitle.text = "養育失敗";
                endingImg.overrideSprite = GamingInfo.characters[PlayerInfo.currentCharacterID].endingImg_bad;
                endingContent.text = GamingInfo.characters[PlayerInfo.currentCharacterID].endingContent_bad;
                break;
            case PlayerInfo.ENDING_STR:
                endingTitle.text = "養育意外";
                endingImg.overrideSprite = GamingInfo.characters[PlayerInfo.currentCharacterID].endingImg_str;
                endingContent.text = GamingInfo.characters[PlayerInfo.currentCharacterID].endingContent_str;
                break;
        }

        // save finished char info
        PlayerInfo.characterItem charItem = new PlayerInfo.characterItem();
        charItem.id = PlayerInfo.currentCharacterID;
        charItem.name = PlayerInfo.currentCharacterName;
        charItem.value_strength = PlayerInfo.value_strength;
        charItem.value_intelligence = PlayerInfo.value_intelligence;
        charItem.value_like = value_like;
        charItem.ending = whichEnding;
        charItem.start_time = PlayerInfo.char_startTime;
        charItem.end_time = DateTime.Now;
        PlayerInfo.characterCollection.Add(charItem);

        // reset char info in playerInfo (will be reset again in choosing char scene)
        GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().resetCurrentCharacter(-1);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
