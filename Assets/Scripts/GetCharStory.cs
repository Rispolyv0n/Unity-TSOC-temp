using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetCharStory : MonoBehaviour {

    private Text thisText;
	// Use this for initialization
	void Start () {
        thisText = GetComponent<Text>();
        thisText.text = GamingInfo.characters[PlayerInfo.currentCharacterID].info;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
