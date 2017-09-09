using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnfoldAndExpand : MonoBehaviour {

    private Button thisBtn;

    private GameObject timePeriodPrefab;

    private GameObject dayObj;
    private GameObject timeSectionPanel;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();

        timePeriodPrefab = Resources.Load<GameObject>("Prefabs/Image_timePeriod");
        dayObj = thisBtn.transform.parent.parent.gameObject;
        timeSectionPanel = thisBtn.transform.parent.gameObject;

        thisBtn.onClick.AddListener(unfold);
    }

    public void unfold() {
        dayObj.GetComponent<LayoutElement>().preferredHeight += 115;
        thisBtn.transform.SetSiblingIndex(timeSectionPanel.transform.childCount-1);
        GameObject timeObj = Instantiate(timePeriodPrefab);
        timeObj.transform.SetParent(timeSectionPanel.transform);
        timeObj.transform.SetSiblingIndex(timeSectionPanel.transform.childCount-2);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
