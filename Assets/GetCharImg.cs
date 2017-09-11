using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetCharImg : MonoBehaviour {

    private Button thisBtn;
    private Image img;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        if (thisBtn != null) {
            img = thisBtn.GetComponent<Image>();
            img.overrideSprite = GamingInfo.characters[PlayerInfo.currentCharacterID].imgForHome;
        }
        else {
            img = GetComponent<Image>();
            img.overrideSprite = GamingInfo.characters[PlayerInfo.currentCharacterID].imgForHome;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
