using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnChosenAndSwitch : MonoBehaviour {

    private Button thisBtn;
    public GameObject control;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(checkIfChosen);
	}

    void checkIfChosen() {
        if (control.GetComponent<GetChosenBtn>().chosen == true && control.GetComponent<GetChosenBtn>().choice>=0) {
            // reset playerinfo's info about current character
            GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().resetCurrentCharacter(control.GetComponent<GetChosenBtn>().choice);
            GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().setCharStartTime();
            SceneManager.LoadScene("home");
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
