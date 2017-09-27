using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Experimental.Networking;
using UnityEngine.SceneManagement;

public class SendOwnerSignUp : MonoBehaviour
{

    public InputField ownerID;
    public InputField password;
    public InputField signUpEmail;

    public InputField shopName;
    public InputField shopAddress;
    public InputField shopContact;
    public Dropdown category_1;
    private Dropdown category_2;

    public InputField ownerName;
    public InputField ownerPhone;
    public InputField ownerEmail;
    public Dropdown ownerGender;

    public Text warningText;

    private Button btn_signUp;
    private string toUrl;

    // Use this for initialization
    void Start()
    {

        toUrl = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/register_shop";
        btn_signUp = GetComponent<Button>();
        btn_signUp.onClick.AddListener(delegate { warningText.text = "傳送註冊資料中..."; StartCoroutine(sendSignUp()); });

    }

    IEnumerator sendSignUp()
    {
        category_2 = GameObject.FindGameObjectWithTag("dropdowns").GetComponent<Dropdown>();

        WWWForm formdata = new WWWForm();
        formdata.AddField("shopID", ownerID.text);
        formdata.AddField("password", password.text);
        formdata.AddField("email", signUpEmail.text);
        formdata.AddField("phone", shopContact.text);
        formdata.AddField("shopName", shopName.text);
        formdata.AddField("shopAddress", shopAddress.text);
        formdata.AddField("category_1", category_1.value);
        formdata.AddField("category_2", category_2.value);
        formdata.AddField("shop_principal", ownerName.text);
        formdata.AddField("shop_principal_gender", ownerGender.value);
        formdata.AddField("shop_principal_phone", ownerPhone.text);
        formdata.AddField("shop_principal_email", ownerEmail.text);

        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();
        if (sending.error != null)
        {
            if (sending.error == "duplicated shopID")
            {
                warningText.text = "註冊錯誤，店家帳號已被使用";
            }
            else if (sending.error == "duplicated shopName")
            {
                warningText.text = "註冊錯誤，店名已被使用";
            }
            else if (sending.error == "internal error")
            {
                warningText.text = "連線錯誤，請再次嘗試";
            }

            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                warningText.text = "註冊成功，畫面切換中...";
                Debug.Log(sending.downloadHandler.text);
                OwnerInfo.ownerID = ownerID.text;
                SceneManager.LoadScene("ownerMenu");
            }
            else
            {
                warningText.text = "註冊失敗，請再次確認";
                Debug.Log(sending.downloadHandler.text);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
