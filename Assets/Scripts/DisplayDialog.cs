using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// attached on the button_send in the chatbot scene
// to get the user input and display it in the scrollView

public class DisplayDialog : MonoBehaviour{

    private Button btn_send;

    public InputField user_input;
    public GameObject dialogParent;
    public GameObject scrollView;

    private GameObject dialog_user_prefab;
    private GameObject obj;

	// Use this for initialization
	void Start () {
        btn_send = GetComponent<Button>();
        dialog_user_prefab = Resources.Load<GameObject>("Prefabs/Image_userDialog");
        btn_send.onClick.AddListener(delegate { StartCoroutine(getInputAndDisplay()); });
	}

    IEnumerator getInputAndDisplay() {
        obj = Instantiate(dialog_user_prefab);
        obj.transform.GetChild(0).GetComponent<Text>().text = user_input.text;
        obj.transform.SetParent(dialogParent.transform);
        
        yield return new WaitForEndOfFrame();
        obj.GetComponent<WidthConstraint>().checkWidth();
        scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }
    

    // Update is called once per frame
    void Update () {
	
	}

    
}
