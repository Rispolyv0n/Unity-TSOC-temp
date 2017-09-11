using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddColumn : MonoBehaviour {

    private Button thisBtn;
    private GameObject addObj;

    public GameObject parent;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(addNewColumn);
	}

    void addNewColumn() {
        addObj = Resources.Load("Prefabs/Image_newColumn") as GameObject;
        GameObject obj = Instantiate(addObj);
        obj.transform.SetParent(parent.transform);
        obj.transform.SetSiblingIndex(RefAndListControl.maxIndex + 1);

        RefAndListControl.addNewItem(obj);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
