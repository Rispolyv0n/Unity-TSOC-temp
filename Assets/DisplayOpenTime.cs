using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayOpenTime : MonoBehaviour {

    private Button thisBtn;
    public GameObject timePanel;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(getTempOpenTimeAndOpenTimePanel);
	}

    void getTempOpenTimeAndOpenTimePanel() {
        timePanel.SetActive(true);
        RefAndListControl.displayTempOpenTime();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
