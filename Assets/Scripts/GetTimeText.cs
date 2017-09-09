using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetTimeText : MonoBehaviour {

    private Text thisText;

	// Use this for initialization
	void Start () {
        thisText = GetComponent<Text>();
        thisText.text = RefAndListControl.parseOpenTimeStruct();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
