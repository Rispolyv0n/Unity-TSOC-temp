using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

// attach on btn to go shop or chatbot
// determine the value used for deciding where to go back(GamingInfo.fromHomeStreet)

public class FromHomeOrStreet : MonoBehaviour {
    
    public bool fromHome;
    public string sceneName;

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
        GamingInfo.fromHomeStreet = fromHome;
        SceneManager.LoadScene(sceneName);
    }

}
