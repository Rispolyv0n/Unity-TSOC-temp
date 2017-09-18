using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class BtnSceneControl : MonoBehaviour
{
    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnMouseDown);
        }
    }

    void OnMouseDown()
    {

        if (GamingInfo.fromHomeStreet == false)
        {
            SceneManager.LoadScene("street");
        }
        else {
            SceneManager.LoadScene("home");
        }
    }
}