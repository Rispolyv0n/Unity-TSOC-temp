using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

// attach on the empty object in the finishChar scene
// decide the ending of the char, get the ending info and display

public class GetCharEnding : MonoBehaviour
{

    public Image endingImg;
    public Text endingTitle;
    public Text endingContent;

    public Button btn_chooseNextChar;
    public Button btn_toStreet;

    private int value_like;
    private int whichEnding;

    // Use this for initialization
    void Start()
    {
        value_like = PlayerInfo.temp_value;

        // decide the ending
        UnityEngine.Random.seed = System.Guid.NewGuid().GetHashCode();

        if (value_like < GamingInfo.likePoints_lowerBoundary)
        {
            float getEnding = UnityEngine.Random.Range(0f, 1f);
            if (getEnding < GamingInfo.ratio_low_strange)
            {
                whichEnding = PlayerInfo.ENDING_STR;
            }
            else
            {
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
            else
            {
                whichEnding = PlayerInfo.ENDING_GOOD;
            }
        }
        else
        {
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
        switch (whichEnding)
        {
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
        charItem.startTime = PlayerInfo.char_startTime.ToString(); // check!!!!!!
        charItem.endTime = DateTime.Now.ToString(); // check!!!!!!!!
        PlayerInfo.characterCollection.Add(charItem);

        // upload character collection
        StartCoroutine(PlayerInfo.uploadCharCollection());

        // check achievements (category:0 - characters collection)
        // calculate how many characters of the kind the player had
        int charNum = 0;
        foreach (PlayerInfo.characterItem item in PlayerInfo.characterCollection)
        {
            if (item.id == PlayerInfo.currentCharacterID)
            {
                ++charNum;
            }
        }
        // check if the player already had the kind of character achievement
        int indexOfAc = -1; // index in player's achievement list
        bool needUploadAC = false;
        foreach (PlayerInfo.achievementItem ac in PlayerInfo.achievementCollection)
        {
            if (GamingInfo.achievements[ac.id].category == 0 && GamingInfo.achievements[ac.id].relative_id == PlayerInfo.currentCharacterID)
            {
                indexOfAc = PlayerInfo.achievementCollection.IndexOf(ac);
                break;
            }
        }
        if (indexOfAc > -1)
        {
            switch (PlayerInfo.achievementCollection[indexOfAc].level)
            {
                case 1:
                    if (charNum >= GamingInfo.achievements[PlayerInfo.currentCharacterID].condition_2)
                    {
                        PlayerInfo.achievementItem new_ac = new PlayerInfo.achievementItem(PlayerInfo.achievementCollection[indexOfAc].id, 2);
                        PlayerInfo.achievementCollection[indexOfAc] = new_ac;
                        needUploadAC = true;
                    }
                    break;
                case 2:
                    if (charNum >= GamingInfo.achievements[PlayerInfo.currentCharacterID].condition_3)
                    {
                        PlayerInfo.achievementItem new_ac = new PlayerInfo.achievementItem(PlayerInfo.achievementCollection[indexOfAc].id, 3);
                        PlayerInfo.achievementCollection[indexOfAc] = new_ac;
                        needUploadAC = true;
                    }
                    break;
                case 3:
                    break;
            }

        }
        else
        {
            // find the corresponding id of the char and the achievement
            int theID = -1;
            foreach (GamingInfo.achievementInfo item in GamingInfo.achievements)
            {
                if (item.category == 0 && item.relative_id == PlayerInfo.currentCharacterID)
                {
                    theID = item.id;
                    break;
                }
            }
            // add new ac to the player's ac list
            PlayerInfo.achievementItem new_ac = new PlayerInfo.achievementItem(theID, 1);
            PlayerInfo.achievementCollection.Add(new_ac);
            needUploadAC = true;
        }

        if (needUploadAC)
        {
            StartCoroutine(PlayerInfo.uploadACCollection());
        }

        // reset char info in playerInfo (will be reset again in choosing char scene)
        GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().resetCurrentCharacter(-1);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
