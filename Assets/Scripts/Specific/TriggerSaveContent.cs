using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TriggerSaveContent : MonoBehaviour {

    private Button thisBtn;

    // Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(RefAndListControl.saveEditContent);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
