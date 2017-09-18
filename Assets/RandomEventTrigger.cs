using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class RandomEventTrigger : MonoBehaviour {

    public GameObject eventPanel;
    public int minHappenSecond;
    public int maxHappenSecond;

    // Use this for initialization
    void Start() {

        //minHappenSecond = 150;
        //maxHappenSecond = 600;
        if (PlayerInfo.currentCharacterID >= 0) {
            Random.seed = System.Guid.NewGuid().GetHashCode();
            float happen = Random.Range(0f, 1f);
            float happenChance = 0.5f;
            float happenTime = Random.Range(minHappenSecond, maxHappenSecond);

            if (happen <= happenChance)
            {
                Debug.Log("event happen time:" + happenTime);
                Invoke("triggerEvent", happenTime);
            }
            else
            {
                Debug.Log("event won't happen");
            }

        }
        else
        {
            Debug.Log("no char, event won't happen");
        }

    }

    void triggerEvent() {
        eventPanel.transform.Find("Text_event").GetComponent<Text>().text = GamingInfo.events[PlayerInfo.currentCharacterID].title;
        eventPanel.transform.Find("Image").GetComponent<Image>().overrideSprite = GamingInfo.events[PlayerInfo.currentCharacterID].img;
        eventPanel.SetActive(true);
        // do the event collecting
        bool hasEvent = false;
        int eventIndex = -1; // index in player's event collection list
        for (int i = 0; i < PlayerInfo.eventCollection.Count; ++i) {
            if (PlayerInfo.eventCollection[i].num == PlayerInfo.currentCharacterID) {
                hasEvent = true;
                eventIndex = i;
                break;
            }
        }
        if (hasEvent)
        {
            PlayerInfo.eventCollection[eventIndex] = new PlayerInfo.eventItem(PlayerInfo.eventCollection[eventIndex].num, PlayerInfo.eventCollection[eventIndex].num + 1);
        }
        else {
            PlayerInfo.eventItem item = new PlayerInfo.eventItem();
            item.num = PlayerInfo.currentCharacterID;
            item.time = 1;
            PlayerInfo.eventCollection.Add(item);
        }

        // check achievements (category:1 - events collection)
        // find the number of the event
        int eventNum = 0;
        foreach (PlayerInfo.eventItem item in PlayerInfo.eventCollection)
        {
            if (item.num == PlayerInfo.currentCharacterID)
            {
                eventNum = item.time;
                break;
            }
        }
        // check if the player already had the kind of achievement
        int indexOfAc = -1;
        foreach (PlayerInfo.achievementItem ac in PlayerInfo.achievementCollection) {
            if (GamingInfo.achievements[ac.id].category == 1 && GamingInfo.achievements[ac.id].relative_id == PlayerInfo.currentCharacterID) {
                indexOfAc = PlayerInfo.achievementCollection.IndexOf(ac);
                break;
            }
        }
        if (indexOfAc > -1)
        {
            // level up the ac
            switch (PlayerInfo.achievementCollection[indexOfAc].level)
            {
                case 1:
                    if (eventNum >= GamingInfo.achievements[PlayerInfo.currentCharacterID].condition_2)
                    {
                        PlayerInfo.achievementItem new_ac = new PlayerInfo.achievementItem();
                        new_ac.id = PlayerInfo.achievementCollection[indexOfAc].id;
                        new_ac.level = 2;
                        PlayerInfo.achievementCollection[indexOfAc] = new_ac;
                    }
                    break;
                case 2:
                    if (eventNum >= GamingInfo.achievements[PlayerInfo.currentCharacterID].condition_3)
                    {
                        PlayerInfo.achievementItem new_ac = new PlayerInfo.achievementItem();
                        new_ac.id = PlayerInfo.achievementCollection[indexOfAc].id;
                        new_ac.level = 3;
                        PlayerInfo.achievementCollection[indexOfAc] = new_ac;
                    }
                    break;
                case 3:
                    break;
            }
        }
        else {
            // find the corresponding id of the char and the achievement
            int theID = -1;
            foreach (GamingInfo.achievementInfo item in GamingInfo.achievements)
            {
                if (item.category == 1 && item.relative_id == PlayerInfo.currentCharacterID)
                {
                    theID = item.id;
                    break;
                }
            }
            // add a new ac
            PlayerInfo.achievementItem new_ac = new PlayerInfo.achievementItem();
            new_ac.id = theID;
            new_ac.level = 1;
            PlayerInfo.achievementCollection.Add(new_ac);
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
