using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoneySetControl : MonoBehaviour {

    private Text thisText;
	// Use this for initialization
	void Start () {
        thisText = GetComponent<Text>();
        thisText.text = PlayerInfo.value_money.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        thisText.text = PlayerInfo.value_money.ToString();
    }
}
