using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenShopInfo : MonoBehaviour {

    private Button thisBtn;
    public string shopID;

    // Use this for initialization
    void Start()
    {
        thisBtn = GetComponent<Button>();
        shopID = thisBtn.GetComponent<GetShopInfo_simple>().shopID;
        thisBtn.onClick.AddListener(openInfo);
    }

    void openInfo()
    {
        PlayerInfo.currentCheckingShopID = shopID;
        SceneManager.LoadScene("shopInfo", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
