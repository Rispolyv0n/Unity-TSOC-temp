using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


// active after successfully logging in

public class GamingDetect : MonoBehaviour
{

    static GamingDetect gamingDetect;

    //static public int temp_value_like;

    // make sure only this script can stay on
    private void Awake()
    {
        /*
        if (gamingDetect == null)
        {
            gamingDetect = this;
            DontDestroyOnLoad(this);
        }
        else if (this != gamingDetect)
        {
            Destroy(gameObject);
        }
        */
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        // for testing : to level 2
        if (PlayerInfo.value_like + PlayerInfo.value_intelligence + PlayerInfo.value_strength > GamingInfo.totalPoints_toLv2)
        {
            GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().setValue_level(2);
        }
        // for testing : to level 3
        if (PlayerInfo.value_like + PlayerInfo.value_intelligence + PlayerInfo.value_strength > GamingInfo.totalPoints_toLv3)
        {
            GameObject.FindGameObjectWithTag("playerInfo").GetComponent<PlayerInfo>().setValue_level(3);
        }
        // for testing : done
        if (PlayerInfo.value_like + PlayerInfo.value_intelligence + PlayerInfo.value_strength > GamingInfo.totalPoints_toDone) {
            temp_value_like = PlayerInfo.value_like;
            PlayerInfo.value_like = -1;
            SceneManager.LoadScene("finishChar");
        }
        

        // for testing : achievement - category 2 (others) e.g. addicted(gaming hours)(id:4)
        bool found = false;
        for (int i=0; i < PlayerInfo.achievementCollection.Count; ++i) {
            // check ac : addicted
            if (PlayerInfo.achievementCollection[i].id == 4 && PlayerInfo.achievementCollection[i].level < 3) {
                found = true;
                if (PlayerInfo.achievementCollection[i].level == 1 && PlayerInfo.totalPlayTime_hr >= GamingInfo.achievements[4].condition_2) {
                    PlayerInfo.achievementItem new_ac = new PlayerInfo.achievementItem(4,2);
                    PlayerInfo.achievementCollection[i] = new_ac;
                } else if (PlayerInfo.achievementCollection[i].level == 2 && PlayerInfo.totalPlayTime_hr >= GamingInfo.achievements[4].condition_3) {
                    PlayerInfo.achievementItem new_ac = new PlayerInfo.achievementItem(4,3);
                    PlayerInfo.achievementCollection[i] = new_ac;
                }
                break; // found & done checking
            }
        }

        // add lv1 ac
        if (!found && PlayerInfo.totalPlayTime_hr >= GamingInfo.achievements[4].condition_1) {
            PlayerInfo.achievementItem new_ac = new PlayerInfo.achievementItem(4,1);
            PlayerInfo.achievementCollection.Add(new_ac);
        }
        */
    }
}
