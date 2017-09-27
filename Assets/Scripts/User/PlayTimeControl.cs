using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayTimeControl : MonoBehaviour
{

    private string display_time;
    private Text textObj;

    // Use this for initialization
    void Start()
    {
        textObj = GetComponent<Text>();
        textObj.text = "0 Hr";
    }

    // Update is called once per frame
    void Update()
    {
        float theTime = PlayerInfo.totalPlayTime_hr;
        int theDay = (int)Math.Floor(theTime) / 24;
        theTime -= theDay * 24;


        if (theDay == 0)
        {
            display_time = theTime + " hr";
        }
        else
        {
            display_time = theDay + " d " + theTime + " hr";
        }
        textObj.text = display_time;
    }
}
