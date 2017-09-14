using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LikeValueControl : MonoBehaviour
{
    public float maxValue;
    public float value;
    // Use this for initialization
    void Start()
    {
        switch (PlayerInfo.value_level)
        {
            case 1:
                maxValue = GamingInfo.maxValue_lv1;
                break;
            case 2:
                maxValue = GamingInfo.maxValue_lv2;
                break;
            case 3:
                maxValue = GamingInfo.maxValue_lv3;
                break;
        }
        value = PlayerInfo.value_like;
    }

    // Update is called once per frame
    void Update()
    {
        switch (PlayerInfo.value_level)
        {
            case 1:
                maxValue = GamingInfo.maxValue_lv1;
                break;
            case 2:
                maxValue = GamingInfo.maxValue_lv2;
                break;
            case 3:
                maxValue = GamingInfo.maxValue_lv3;
                break;
        }
        if (SceneManager.GetActiveScene().name == "street")
        {
            value = PlayerInfo.value_like;
            this.transform.localPosition = new Vector3(-270 + 270 * (value / maxValue), 0.0f, 0.0f);
        }
        else
        {
            value = PlayerInfo.value_like;
            this.transform.localPosition = new Vector3(-495 + 495 * (value / maxValue), 0.0f, 0.0f);
        }
    }
}
