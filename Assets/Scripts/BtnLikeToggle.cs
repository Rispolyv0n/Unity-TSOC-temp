using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnLikeToggle : MonoBehaviour {

    public Button yourButton;
    private bool btn_state;
    public GameObject obj;
    // Use this for initialization
    void Start()
    {
        btn_state = false;
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TaskOnClick()
    {
        if (btn_state == false)
        {
            btn_state = true;
            obj.SetActive(true);
        }
        else
        {
            btn_state = false;
            obj.SetActive(false);
        }
    }
}
