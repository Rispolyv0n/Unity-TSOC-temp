using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class RandomEventTrigger : MonoBehaviour
{

    public GameObject eventPanel;
    public int minHappenSecond;
    public int maxHappenSecond;

    // Use this for initialization
    void Start()
    {

        //minHappenSecond = 150;
        //maxHappenSecond = 600;
        if (PlayerInfo.currentCharacterID >= 0 && PlayerInfo.streetMode.gameMode == true)
        {
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

    void triggerEvent()
    {
        eventPanel.transform.Find("Text_event").GetComponent<Text>().text = GamingInfo.events[PlayerInfo.currentCharacterID].title;
        eventPanel.transform.Find("Image").GetComponent<Image>().overrideSprite = GamingInfo.events[PlayerInfo.currentCharacterID].img;
        eventPanel.SetActive(true);
        // do the event collecting
        bool hasEvent = false;
        int eventIndex = -1; // index in player's event collection list
        for (int i = 0; i < PlayerInfo.eventCollection.Count; ++i)
        {
            if (PlayerInfo.eventCollection[i].id == PlayerInfo.currentCharacterID)
            {
                hasEvent = true;
                eventIndex = i;
                break;
            }
        }
        if (hasEvent)
        {
            PlayerInfo.eventCollection[eventIndex] = new PlayerInfo.eventItem(PlayerInfo.eventCollection[eventIndex].id, PlayerInfo.eventCollection[eventIndex].num + 1);
        }
        else
        {
            PlayerInfo.eventItem item = new PlayerInfo.eventItem();
            item.id = PlayerInfo.currentCharacterID;
            item.num = 1;
            PlayerInfo.eventCollection.Add(item);
        }



        // check achievements (category:1 - events collection)
        // find the number of the event
        int eventNum = 0;
        foreach (PlayerInfo.eventItem item in PlayerInfo.eventCollection)
        {
            if (item.id == PlayerInfo.currentCharacterID)
            {
                eventNum = item.id;
                break;
            }
        }
        // check if the player already had the kind of achievement
        bool needUploadAC = false;
        int indexOfAc = -1;
        foreach (PlayerInfo.achievementItem ac in PlayerInfo.achievementCollection)
        {
            if (GamingInfo.achievements[ac.id].category == 1 && GamingInfo.achievements[ac.id].relative_id == PlayerInfo.currentCharacterID)
            {
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
                        PlayerInfo.achievementItem new_ac = new PlayerInfo.achievementItem(PlayerInfo.achievementCollection[indexOfAc].id, 2);
                        PlayerInfo.achievementCollection[indexOfAc] = new_ac;
                        needUploadAC = true;
                    }
                    break;
                case 2:
                    if (eventNum >= GamingInfo.achievements[PlayerInfo.currentCharacterID].condition_3)
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
                if (item.category == 1 && item.relative_id == PlayerInfo.currentCharacterID)
                {
                    theID = item.id;
                    break;
                }
            }
            // add a new ac
            PlayerInfo.achievementItem new_ac = new PlayerInfo.achievementItem(theID, 1);
            PlayerInfo.achievementCollection.Add(new_ac);
            needUploadAC = true;
        }

        StartCoroutine(PlayerInfo.uploadEventCollection());
        if (needUploadAC)
        {
            StartCoroutine(PlayerInfo.uploadACCollection());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
