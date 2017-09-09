using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;
using System.Globalization;

public class PasswordConfirm : MonoBehaviour {

    public InputField username;
    public InputField email;
    public InputField password;
    public InputField passwordConfirm;
    public Text resultDisplay;
    public Button signUpBtn;

    private bool pwOK;
    private bool emailOK;
    private bool idOK;
    private bool invalid;

    // Use this for initialization
    void Start () {
        pwOK = false;
        emailOK = false;
        idOK = false;
        signUpBtn.interactable = false;
        password.onValueChanged.AddListener(delegate { checkPwConfirm(); });
        passwordConfirm.onValueChanged.AddListener(delegate { checkPwConfirm(); });
        username.onValueChanged.AddListener(delegate { checkUsername(); });
        email.onValueChanged.AddListener(delegate { checkMailForm(); });
	}

    void checkPwConfirm() {
        if (password.text != null && passwordConfirm.text != null)
        {


            if (password.text == "" || passwordConfirm.text == "")
            {
                pwOK = false;
                resultDisplay.text = "密碼欄位不可為空";
            }
            else if (password.text.Length > 40)
            {
                pwOK = false;
                resultDisplay.text = "密碼長度不可超過40字";
            }
            else if (password.text != passwordConfirm.text)
            {
                pwOK = false;
                resultDisplay.text = "請確認您的密碼及密碼確認是否輸入一致";
            }
            else
            {
                pwOK = true;
                resultDisplay.text = "";
            }
        }
    }

    void checkMailForm()
    {
        string strIn = email.text;
        if (strIn == null)
        {
            resultDisplay.text = "email格式錯誤，請再次確認";
            emailOK = false;
        }
        else
        {
            resultDisplay.text = "";
            emailOK = true;
        }

        invalid = false;
        // Use IdnMapping class to convert Unicode domain names.
        strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper);
        if (invalid)
        {
            resultDisplay.text = "email格式錯誤，請再次確認";
            emailOK = false;
        }
        else
        {
            resultDisplay.text = "";
            emailOK = true;
        }

        // Return true if strIn is in valid e-mail format.
        if (!Regex.IsMatch(strIn, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$"))
        {
            resultDisplay.text = "email格式錯誤，請再次確認";
            emailOK = false;
        }
        else
        {
            resultDisplay.text = "";
            emailOK = true;
        }
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
            invalid = true;
        }
        return match.Groups[1].Value + domainName;
    }

    void checkUsername() {
        if (username.text == null || username.text == "")
        {
            idOK = false;
            resultDisplay.text = "使用者名稱欄位不可為空";
        }
        else if (username.text.Length > 30) {
            idOK = false;
            resultDisplay.text = "使用者名稱長度不可超過30字";
        }
        else
        {
            idOK = true;
            resultDisplay.text = "";
        }
    }

    // Update is called once per frame
    void Update () {
        if (pwOK && emailOK && idOK)
        {
            signUpBtn.interactable = true;
        }
        else {
            signUpBtn.interactable = false;
        }

	}
}
