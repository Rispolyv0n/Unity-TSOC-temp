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
        //do the event collecting
        bool hasEvent = false;
        int eventNum = -1;
        for (int i = 0; i < PlayerInfo.eventCollection.Count; ++i) {
            if (PlayerInfo.eventCollection[i].num == PlayerInfo.currentCharacterID) {
                hasEvent = true;
                eventNum = i;
                break;
            }
        }
        if (hasEvent)
        {
            PlayerInfo.eventCollection[eventNum] = new PlayerInfo.eventItem(PlayerInfo.eventCollection[eventNum].num, PlayerInfo.eventCollection[eventNum].num+1);
        }
        else {
            PlayerInfo.eventItem item = new PlayerInfo.eventItem();
            item.num = PlayerInfo.currentCharacterID;
            item.time = 1;
            PlayerInfo.eventCollection.Add(item);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
