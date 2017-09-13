using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GetCharName : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (PlayerInfo.currentCharacterName == null || PlayerInfo.currentCharacterName == "")
        {
            this.GetComponent<InputField>().text = GamingInfo.characters[PlayerInfo.currentCharacterID].name;
        }
        else {
            this.GetComponent<InputField>().text = PlayerInfo.currentCharacterName;
        }

        this.GetComponent<InputField>().onValueChanged.AddListener(saveName);
	    
	}

    private void saveName(string arg0)
    {
        PlayerInfo.currentCharacterName = arg0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
