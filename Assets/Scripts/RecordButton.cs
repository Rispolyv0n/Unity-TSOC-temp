using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on the recording button
// for UI objects controlling
// trigger the RecordControl.cs

public class RecordButton : MonoBehaviour {
    
    private Button thisBtn;

    // objects controlled
    public Text stateText;
    public GameObject doneRecordingPanel;
    public Button backBtn;

    // the main controlling script "RecordControl" in object "empty_control"
    public GameObject controlObj;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(whenClicking);
	}

    void whenClicking() {
        if (!controlObj.GetComponent<RecordControl>().isRecording)
        {
            controlObj.GetComponent<RecordControl>().isRecording = true;
            controlObj.GetComponent<RecordControl>().hasSaved = false;
            backBtn.interactable = false;
            stateText.gameObject.SetActive(true);
        }
        else {
            controlObj.GetComponent<RecordControl>().isRecording = false;
            doneRecordingPanel.SetActive(true);
            backBtn.interactable = true;
        }
    }

    // Update is called once per frame
	void Update () {
	
	}
}
