using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Btn_StreetToShop : MonoBehaviour
{
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
        //SaveObjOnLoad.objState = 0;
        GamingInfo.fromHomeStreet = fromHome;
        SceneManager.LoadScene(sceneName);
    }
}