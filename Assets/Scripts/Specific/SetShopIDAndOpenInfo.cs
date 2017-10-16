using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetShopIDAndOpenInfo : MonoBehaviour {

    private Button thisBtn;
    public string shopID;

    // Use this for initialization
    void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(openInfo);
    }

    void openInfo()
    {
        PlayerInfo.currentCheckingShopID = shopID;
        SceneManager.LoadScene("shopInfo");
    }

    // Update is called once per frame
    void Update () {
	
	}
}
