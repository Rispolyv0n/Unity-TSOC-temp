using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


// active after successfully logging in

public class GamingDetect : MonoBehaviour {

    static GamingDetect gamingDetect;

    static public float temp_value_like;

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
        // for testing : to level 2
        if (PlayerInfo.value_like + PlayerInfo.value_intelligence + PlayerInfo.value_strength > GamingInfo.totalPoints_toLv2)
        {
            PlayerInfo.value_level = 2;
        }
        // for testing : to level 3
        if (PlayerInfo.value_like + PlayerInfo.value_intelligence + PlayerInfo.value_strength > GamingInfo.totalPoints_toLv3)
        {
            PlayerInfo.value_level = 3;
        }
        // for testing : done
        if (PlayerInfo.value_like + PlayerInfo.value_intelligence + PlayerInfo.value_strength > GamingInfo.totalPoints_toDone) {
            temp_value_like = PlayerInfo.value_like;
            PlayerInfo.value_like = -1;
            SceneManager.LoadScene("finishChar");
        }
        // for testing : achievement
	}
}
