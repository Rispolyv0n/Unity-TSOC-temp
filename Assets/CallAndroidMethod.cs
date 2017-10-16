using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Experimental.Networking;
using System;

public class CallAndroidMethod : MonoBehaviour {

    public GameObject imgObj;

    private Button thisBtn;

    private AndroidJavaObject androidObj = null;
    private AndroidJavaObject activityContext = null;

    // Use this for initialization
    void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(callAndroidPlugin);
	}

    public void callAndroidPlugin() {
        imgObj.SetActive(true);
        Debug.Log("call android plugin!!");
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) //com.google.unity.GoogleUnityActivity
        {
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
        }
        using (AndroidJavaClass pluginClass = new AndroidJavaClass("iris.androidtryplugin.Ris"))
        {
            if (pluginClass != null)
            {
                androidObj = pluginClass.CallStatic<AndroidJavaObject>("instance");
                androidObj.Call("setContext", activityContext);
                activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => { androidObj.Call("showMessage", "get img!"); }));
                Debug.Log("call run on ui thread~~~~");
            }
        }
    }
    
    public void OnResumeEventHandler(string path){
        Debug.Log("back in unity:on resume handler");
        displayImgPath(path);
    }

    public IEnumerator displayImgPath(string path)
    {
        Debug.Log("back in unity:displayImgPath func");
        CommentControl.imgPath = path;
        Debug.Log("get img path:"+path);

        Text text = imgObj.transform.GetChild(0).GetComponent<Text>();
        text.text = "ImgPath: " + path;

        WWW www = new WWW("file://" + path);
        yield return www;
        Debug.Log("got image");
        Texture2D texture = www.texture;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        imgObj.GetComponent<Image>().sprite = sprite;
        imgObj.GetComponent<Image>().overrideSprite = sprite;
        imgObj.transform.GetChild(0).gameObject.SetActive(false);

    }


    // Update is called once per frame
    void Update () {
	
	}
}
