using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetObjImg : MonoBehaviour {
    private Button thisBtn;
    private Image img;
    // Use this for initialization
    void Start () {
        thisBtn = GetComponent<Button>();
        img = thisBtn.GetComponent<Image>();
        if (PlayerInfo.currentCharacterID < 0)
        {
            img.enabled = false;
        }
        else {
            img.enabled = true;
            img.overrideSprite = GamingInfo.characters[PlayerInfo.currentCharacterID].imgForCollect;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
