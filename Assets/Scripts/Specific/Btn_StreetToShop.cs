using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Btn_StreetToShop : MonoBehaviour
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
        //SaveObjOnLoad.objState = 0;
        GamingInfo.fromHomeStreet = false;
        SceneManager.LoadScene("shop");
    }
}