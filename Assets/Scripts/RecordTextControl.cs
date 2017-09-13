using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on hint text on the screen
// changing the content accroding to the RecordControl.cs

public class RecordTextControl : MonoBehaviour {

    private Text thisText;
    private int counter;

    public GameObject control;

	// Use this for initialization
	void Start () {
        thisText = GetComponent<Text>();
        counter = 0;
        InvokeRepeating("changingText",0.5f,0.5f);
    }

    void changingText() {
        if (control.GetComponent<RecordControl>().isRecording)
        {
            counter++;
            switch (counter % 4)
            {
                case 0:
                    thisText.text = "Recording";
                    break;
                case 1:
                    thisText.text = "Recording.";
                    break;
                case 2:
                    thisText.text = "Recording..";
                    break;
                case 3:
                    thisText.text = "Recording...";
                    break;
                default:
                    break;

            }
        }
        
    }

	// Update is called once per frame
	void Update () {
        
        
	}
}
