using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;



public class DisplayUpdateTime : MonoBehaviour {

    private Text thisText;

	// Use this for initialization
	void Start () {
        thisText = GetComponent<Text>();
        if (OwnerInfo.hasUpdated)
        {
            thisText.text = "您上次更新於 " + OwnerInfo.updateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }
        else {
            thisText.text = "您尚未做任何更新";
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
