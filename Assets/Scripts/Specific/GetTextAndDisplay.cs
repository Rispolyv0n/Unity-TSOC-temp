using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetTextAndDisplay : MonoBehaviour {

    public InputField getUserID;
    private Text currentTextObj;
	// Use this for initialization
	void Start () {
        currentTextObj = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisplayGetText() {
        currentTextObj.text = "The password has been sent to the e-mail address you signed up with the username: \n" + getUserID.text + "\n\nPlease check your email for password retrieving.";
    }
}
