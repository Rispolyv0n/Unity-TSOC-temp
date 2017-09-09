using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on the empty object in the panel "panel_preview" in the scene "ownerEdit"
// display the content stored in the RefAndListControl.cs

public class PreviewContentControl : MonoBehaviour {

    public GameObject parent;

    public Text name;
    public Text address;

    private Text titlePrefab;
    private Text contentPrefab;
	
    // Use this for initialization
	void Start () {
        
    }

    public void OnEnable()
    {
        // remove old content
        GameObject[] oldList = GameObject.FindGameObjectsWithTag("content");
        if (oldList.Length != 0) {
            foreach (GameObject obj in oldList) {
                Destroy(obj);
            }
        }

        // display new content
        name.text = RefAndListControl.name;
        address.text = RefAndListControl.address;
        titlePrefab = Resources.Load<Text>("Prefabs/Text_title");
        contentPrefab = Resources.Load<Text>("Prefabs/Text_content");
        for (int i = 0; i < RefAndListControl.columnList.Count; ++i)
        {
            Text titleObj = Instantiate(titlePrefab);
            titleObj.transform.SetParent(parent.transform);
            titleObj.text = RefAndListControl.columnList[i].title;
            Text contentObj = Instantiate(contentPrefab);
            contentObj.transform.SetParent(parent.transform);
            contentObj.text = RefAndListControl.columnList[i].content;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
