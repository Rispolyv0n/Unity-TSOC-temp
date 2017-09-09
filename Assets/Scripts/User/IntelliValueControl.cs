using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntelliValueControl : MonoBehaviour
{
    public float maxValue;
    public float value;
    // Use this for initialization
    void Start()
    {
        maxValue = 100.0f;
        value = PlayerInfo.value_intelligence;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "street")
        {
            value = PlayerInfo.value_intelligence;
            this.transform.localPosition = new Vector3(-270 + 270 * (value / maxValue), 0.0f, 0.0f);
        }
        else {
            value = PlayerInfo.value_intelligence;
            this.transform.localPosition = new Vector3(-495 + 495 * (value / maxValue), 0.0f, 0.0f);
        }
    }
}
