using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddColumn : MonoBehaviour {

    private Button thisBtn;
    private GameObject addObj;

    public GameObject parent;
    public GameObject scrollView;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(delegate { StartCoroutine(addNewColumn()); } );
	}

    IEnumerator addNewColumn() {
        addObj = Resources.Load("Prefabs/Image_newColumn") as GameObject;
        GameObject obj = Instantiate(addObj);
        obj.transform.SetParent(parent.transform);
        obj.transform.SetSiblingIndex(RefAndListControl.maxIndex + 1);

        RefAndListControl.addNewItem(obj);

        yield return new WaitForEndOfFrame();
        scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
