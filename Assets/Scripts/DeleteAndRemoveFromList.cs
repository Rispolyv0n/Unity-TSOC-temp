using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on the delete button which is attached on the prefab "Image_newColumn" in the scene "ownerEdit"
// destroy the whole obj and remove it from the column obj record list in the script "RefAndListControl.cs"

public class DeleteAndRemoveFromList : MonoBehaviour {

    public GameObject theWholeObj;
    private Button thisBtn;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(removeControl);
	}

    void removeControl() {
        RefAndListControl.removeItem(thisBtn.transform.parent.parent.Find("Button_drag").gameObject.GetComponent<ClickAndDrag>().listIndex);
        Destroy(theWholeObj);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
