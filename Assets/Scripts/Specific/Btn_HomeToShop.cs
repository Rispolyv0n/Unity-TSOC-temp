using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Btn_HomeToShop : MonoBehaviour
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
        //SaveObjOnLoad.objState = 1;
        GamingInfo.fromHomeStreet = true;
        SceneManager.LoadScene("shop");
    }
}