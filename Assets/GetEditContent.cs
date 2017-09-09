using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on the button "Button_preview"
// trigger the RefAndlistControl.cs to store the info content

public class GetEditContent : MonoBehaviour {

    public GameObject previewPanel;
    private Button thisBtn;
	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(openPreviewAndSet);
        
	}

    void openPreviewAndSet()
    {
        RefAndListControl.getEditContent();
        previewPanel.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
