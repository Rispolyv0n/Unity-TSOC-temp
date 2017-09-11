using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetHaveGoneStreet : MonoBehaviour {

    private Button thisBtn;

    // Use this for initialization
    void Start()
    {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(setHasGoneStreet);
    }

    void setHasGoneStreet()
    {
        PlayerInfo.firstGoStreet = false;
        if (PlayerInfo.firstGoHome == false && PlayerInfo.firstGoStreet == false)
        {
            PlayerInfo.firstLogIn = false;
        }
        //SceneManager.LoadScene("street");
        //SceneManager.UnloadScene("instruction_street");
        SceneManager.LoadScene("streetTest");
    }

    // Update is called once per frame
    void Update () {
	
	}
}
