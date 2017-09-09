using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GetChosenBtn : MonoBehaviour {

    public bool chosen;
    public int choice;

    public GameObject buttonParent;
    private GameObject obj_prefab;

    // Use this for initialization
    void Start () {
        choice = -1;
        chosen = false;
        
        obj_prefab = Resources.Load("Prefabs/Button_char") as GameObject;
        
        // instantiate the buttons
        for (int i = 0; i < GamingInfo.characterNum; ++i) {
            GameObject obj = Instantiate(obj_prefab);
            obj.GetComponent<BtnChosen>().id = i;
            obj.transform.SetParent(buttonParent.transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
        /*
        chosenBtn = EventSystem.current.currentSelectedGameObject;
        if (chosenBtn != null && chosenBtn.tag == "choose_charBtn")
        {
            chosen = true;
            closeOtherPanel();
            chosenBtn = null;
        }
        else if (chosenBtn != null && chosenBtn.tag == "choose_go" && chosen) {
            // change scene and send char data to server
            Debug.Log(finalChoice.name);
            SceneManager.LoadScene("home");
        }
        */
        
    }

    void closeOtherPanel() {
        GameObject[] panelsToClose = GameObject.FindGameObjectsWithTag("choose_blackPanel");
        foreach (GameObject obj in panelsToClose)
        {
            obj.SetActive(false);
            /*
            // doesn't work QQ
            if (obj.transform.parent.gameObject == chosenBtn) {
                obj.SetActive(true);
            }
            */
        }
        
        //chosenBtn.transform.GetChild(2).gameObject.SetActive(true);
        //finalChoice = chosenBtn;
    }
}
