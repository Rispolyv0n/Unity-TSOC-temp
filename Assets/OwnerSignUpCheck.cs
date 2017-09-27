using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;
using System.Globalization;
using UnityEngine.Experimental.Networking;
using UnityEngine.SceneManagement;

public class OwnerSignUpCheck : MonoBehaviour
{

    public InputField userID;
    public InputField userEmail;
    public InputField password;
    public InputField passwordConfirm;

    public InputField shopName;
    public InputField shopAddress;
    public InputField shopContact;

    public Dropdown category_a;
    private Dropdown category_b;

    public InputField ownerName;
    public Dropdown ownerGender;
    public InputField ownerContact;
    public InputField ownerEmail;

    public Text warningText;
    public Button signUpBtn;

    private bool emailInvalid;

    private string toUrl;

    // Use this for initialization
    void Start()
    {
        toUrl = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/register_shop";
        category_b = GameObject.FindGameObjectWithTag("dropdowns").GetComponent<Dropdown>();
        signUpBtn.onClick.AddListener(sendSignUpInfo);

    }

    void sendSignUpInfo()
    {
        if (isBlank(userID.text))
        {
            warningText.text = "帳號欄位空白，請再次確認";
            return;
        }
        if (userID.text.Length > 30)
        {
            warningText.text = "帳號長度不可超過30字";
            return;
        }

        if (!emailFormIsCorrect(userEmail.text))
        {
            warningText.text = "註冊信箱欄位輸入錯誤，請再次確認";
            return;
        }
        if (!emailFormIsCorrect(ownerEmail.text))
        {
            warningText.text = "店家聯絡人信箱欄位輸入錯誤，請再次確認";
            return;
        }

        if (isBlank(password.text) || isBlank(passwordConfirm.text))
        {
            warningText.text = "密碼欄位空白，請再次確認";
            return;
        }
        if (!password.text.Equals(passwordConfirm.text))
        {
            warningText.text = "請確認兩次密碼輸入為相同內容";
            return;
        }

        if (isBlank(shopName.text))
        {
            warningText.text = "店名欄位空白，請再次確認";
            return;
        }
        if (isBlank(shopAddress.text))
        {
            warningText.text = "店家地址欄位空白，請再次確認";
            return;
        }
        if (isBlank(shopContact.text))
        {
            warningText.text = "店家連絡電話欄位空白，請再次確認";
            return;
        }

        if (isBlank(ownerName.text))
        {
            warningText.text = "店家負責人姓名欄位空白，請再次確認";
            return;
        }
        if (isBlank(ownerContact.text))
        {
            warningText.text = "店家負責人連絡電話欄位空白，請再次確認";
            return;
        }

        // send info to http request
        warningText.text = "傳送註冊資料中...";
        StartCoroutine(sendSignUp());
    }

    bool isBlank(string str)
    {
        if (str == null || str == "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool emailFormIsCorrect(string strIn)
    {
        if (isBlank(strIn))
        {
            return false;
        }

        emailInvalid = false;
        // Use IdnMapping class to convert Unicode domain names.
        strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper);
        if (emailInvalid)
        {
            return false;
        }

        // Return true if strIn is in valid e-mail format.
        if (!Regex.IsMatch(strIn, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$"))
        {
            return false;
        }

        return true;
    }

    private string DomainMapper(Match match)
    {

        // IdnMapping class with default property values.
        IdnMapping idn = new IdnMapping();

        string domainName = match.Groups[2].Value;
        try
        {
            domainName = idn.GetAscii(domainName);
        }
        catch (ArgumentException)
        {
            emailInvalid = true;
        }
        return match.Groups[1].Value + domainName;
    }

    IEnumerator sendSignUp()
    {
        category_b = GameObject.FindGameObjectWithTag("dropdowns").GetComponent<Dropdown>();

        WWWForm formdata = new WWWForm();
        formdata.AddField("shopID", userID.text);
        formdata.AddField("password", password.text);
        formdata.AddField("email", userEmail.text);
        formdata.AddField("phone", shopContact.text);
        formdata.AddField("shopName", shopName.text);
        formdata.AddField("shopAddress", shopAddress.text);
        formdata.AddField("category_1", category_a.value);
        formdata.AddField("category_2", category_b.value);
        formdata.AddField("shop_principal", ownerName.text);
        formdata.AddField("shop_principal_gender", ownerGender.value);
        formdata.AddField("shop_principal_phone", ownerContact.text);
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
            else
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
                OwnerInfo.ownerID = userID.text;
                SceneManager.LoadScene("ownerMenu");
            }
            else if (sending.downloadHandler.text == "duplicated shopID")
            {
                warningText.text = "註冊錯誤，店家帳號已被使用";
            }
            else if (sending.downloadHandler.text == "duplicated shopName")
            {
                warningText.text = "註冊錯誤，店名已被使用";
            }
            else
            {
                warningText.text = "註冊失敗，請再次確認";
            }
            Debug.Log(sending.downloadHandler.text);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
