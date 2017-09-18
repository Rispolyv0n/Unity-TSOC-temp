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

    public void setHasGoneStreet()
    {
        PlayerInfo.firstGoStreet = false;
        GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().saveUserPace();
        /*
        if (PlayerInfo.firstGoHome == false && PlayerInfo.firstGoStreet == false)
        {
            PlayerInfo.firstLogIn = false;
        }
        */
    }

    // Update is called once per frame
    void Update () {
	
	}
}
