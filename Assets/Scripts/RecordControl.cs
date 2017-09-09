using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecordControl : MonoBehaviour {

    public bool isRecording;
    public bool hasSaved;


	// Use this for initialization
	void Start () {
        isRecording = false;
        hasSaved = true;
	}

    public void setHasSaved(bool value) {
        hasSaved = value;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
