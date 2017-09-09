using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TriggerGetTime : MonoBehaviour {

    private Button thisBtn;
    public GameObject timePanel;
    public GameObject checkPanel;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(getTimeAndClosePanel);
	}

    void getTimeAndClosePanel() {
        int checkResult = RefAndListControl.checkIfOpenTimeComplete();
        Debug.Log(checkResult);
        if (checkResult == -1)
        {
            checkPanel.SetActive(true);
            checkPanel.transform.Find("Text").GetComponent<Text>().text = "編輯失敗\n時間欄位為24小時制\n\"小時\"欄位請填入0至23";
        }
        else if (checkResult == 0)
        {
            checkPanel.SetActive(true);
            checkPanel.transform.Find("Text").GetComponent<Text>().text = "編輯失敗\n請檢查您可能有漏掉的空白欄位";
        }
        else if(checkResult == 1)
        {
            RefAndListControl.getEditTime();
            timePanel.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
