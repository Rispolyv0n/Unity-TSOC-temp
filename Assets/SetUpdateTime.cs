using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Experimental.Networking;
using System.Web.Script.Serialization;
using UnityEngine.SceneManagement;

public class SetUpdateTime : MonoBehaviour
{

    private Button thisBtn;
    public GameObject thePanel;
    public bool ifNeedtoStay; // if need to upload before this btn disappear
    public bool ifNeedtoOpenPanel; // if need to upload before opening another panel

    // Use this for initialization
    void Start()
    {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(setTime);

    }

    void setTime()
    {
        //OwnerInfo.updateTime = DateTime.Now;
        StartCoroutine(uploadUpdateTime());

        //OwnerInfo.hasUpdated = true;
    }

    public IEnumerator uploadUpdateTime()
    {
        string toUrl = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/set_shopUpdateTime?shopID=" + OwnerInfo.ownerID + "&updateTime=" + DateTime.Now.ToString();

        Debug.Log("upload update time~" + toUrl);

        UnityWebRequest sending = UnityWebRequest.Get(toUrl);
        yield return sending.Send();

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("upload time:" + sending.downloadHandler.text);
                if (ifNeedtoStay)
                {
                    thePanel.SetActive(false);
                }
                else if (ifNeedtoOpenPanel)
                {
                    thePanel.SetActive(true);
                }
            }
            else
            {
                Debug.Log(sending.downloadHandler.text);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
