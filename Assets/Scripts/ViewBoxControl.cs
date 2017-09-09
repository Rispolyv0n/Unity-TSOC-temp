using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ViewBoxControl : MonoBehaviour {
    public Button yourButton;
    public Animator aniObj;
    private bool btn_state;
    // Use this for initialization
    void Start () {
        btn_state = false;
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void TaskOnClick()
    {
        if (btn_state == false) {
            btn_state = true;
            aniObj.SetTrigger("viewBoxEnter");
        } else {
            btn_state = false;
            aniObj.SetTrigger("viewBoxExit");
        }
    }
}
