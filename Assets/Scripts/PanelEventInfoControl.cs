using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelEventInfoControl : MonoBehaviour {

    public int eventNum;
    public GameObject eventInfoPanel;
    public Text eventTitle;
    public Text eventInfo;
    public Text eventCollectionTime;
    public Image eventImg;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().sprite = GamingInfo.events[eventNum].img;
        gameObject.AddComponent<PolygonCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnMouseDown()
    {
        // load the eventInfo and decide if the user can open the info panel
        Debug.Log("click"+eventNum);
        foreach (PlayerInfo.eventItem item in PlayerInfo.eventCollection) {
            if (item.num == eventNum) {
            //if (gameObject.GetComponent<GameObject>().activeInHierarchy==true) { 
                Debug.Log("open");
                eventImg.overrideSprite = GamingInfo.events[eventNum].img;
                eventTitle.text = GamingInfo.events[eventNum].title;
                eventInfo.text = GamingInfo.events[eventNum].content;
                eventCollectionTime.text = "事件蒐集次數 : "+item.time;
                eventInfoPanel.SetActive(true);
                break;
            }
        }
        // http:send eventNum to get Info
        // take GamingInfo.cs as example
        
    }
}
