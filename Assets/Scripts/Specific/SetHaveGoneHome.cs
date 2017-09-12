using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetHaveGoneHome : MonoBehaviour {

    private Button thisBtn;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(setHasGoneHome);
	}

    public void setHasGoneHome() {
        PlayerInfo.firstGoHome = false;
        /*
        if (PlayerInfo.firstGoHome == false && PlayerInfo.firstGoStreet == false) {
            PlayerInfo.firstLogIn = false;
        }
        */
    }

	// Update is called once per frame
	void Update () {
	
	}
}
