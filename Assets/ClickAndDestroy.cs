using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickAndDestroy : MonoBehaviour {

    private Button thisBtn;

    private GameObject dayObj;

    public GameObject objToDestroy;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        dayObj = thisBtn.transform.parent.parent.parent.gameObject;

        thisBtn.onClick.AddListener(destroyObj);
	}

    void destroyObj() {
        dayObj.GetComponent<LayoutElement>().preferredHeight -= 115;


        Destroy(objToDestroy);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
