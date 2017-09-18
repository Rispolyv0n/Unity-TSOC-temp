using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// attach on Button_back in the scene "ownerRecord"
// switch scene control

public class BackBtnInRecord : MonoBehaviour {

    private Button thisBtn;
    public GameObject control;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(checkIfSaved);
	}

    void checkIfSaved() {
        if (control.GetComponent<RecordControl>().hasSaved)
        {
            SceneManager.LoadScene("ownerMenu", LoadSceneMode.Single);
        }
        
    }

	// Update is called once per frame
	void Update () {
	
	}
}
