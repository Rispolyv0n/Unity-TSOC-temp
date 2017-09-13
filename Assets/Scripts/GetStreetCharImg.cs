using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GetStreetCharImg : MonoBehaviour {

    
    private Image img;
    // Use this for initialization
    void Start () {
        img = GetComponent<Image>();
        if (PlayerInfo.currentCharacterID < 0)
        {
            img.enabled = false;
        }
        else {
            img.enabled = true;
            img.overrideSprite = GamingInfo.characters[PlayerInfo.currentCharacterID].imgForStreet;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
