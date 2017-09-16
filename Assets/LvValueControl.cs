using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LvValueControl : MonoBehaviour {

    private Text thisText;

	// Use this for initialization
	void Start () {
        thisText = GetComponent<Text>();
        thisText.text = "Lv." + PlayerInfo.value_level;
	}
	
	// Update is called once per frame
	void Update () {
        thisText.text = "Lv." + PlayerInfo.value_level;
    }
}
