using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayTimeControl : MonoBehaviour {

    private string display_time;
    private Text textObj;

	// Use this for initialization
	void Start () {
        textObj = GetComponent<Text>();
        textObj.text = "0 Hr";
	}
	
	// Update is called once per frame
	void Update () {
        float theTime = PlayerInfo.value_playTime_hr;
        int theDay = PlayerInfo.value_playTime_day;
        if (theDay == 0)
        {
            display_time = theTime + " Hr";
        }
        else {
            display_time = theDay + " D " + theTime + " Hr";
        }
        textObj.text = display_time;
	}
}
