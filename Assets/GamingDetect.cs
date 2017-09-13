using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


// active after successfully logging in

public class GamingDetect : MonoBehaviour {

    static GamingDetect gamingDetect;

    // make sure only this script can stay on
    private void Awake()
    {
        if (gamingDetect == null)
        {
            gamingDetect = this;
            DontDestroyOnLoad(this);
        }
        else if (this != gamingDetect)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // for testing
        if (PlayerInfo.value_like > 5) {
            PlayerInfo.value_like = 0;
            SceneManager.LoadScene("finishChar");
        }
	}
}
