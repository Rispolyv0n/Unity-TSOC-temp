using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Experimental.Networking;
using System.Text.RegularExpressions;

public class MessageRequesting : MonoBehaviour {

    public InputField messageFromUser;
    public GameObject chatbotMessagePrefab;
    public Button sendMessageBtn;
    public GameObject dialogParent;
    public GameObject scrollView;

    private string url;
    private string stringFromUser;
    private string stringFromChatbot;
    private GameObject obj;


	// Use this for initialization
	void Start () {
        obj = new GameObject();
        url = "https://kevin.imslab.org"+PlayerInfo.port+"/chatbot?str=";
        //sendMessageBtn.onClick.AddListener(delegate { StartCoroutine(sendMessage()); } );
	}

    IEnumerator sendMessage() {
        stringFromUser = messageFromUser.text;
        messageFromUser.text = "";

        using (UnityWebRequest sending = UnityWebRequest.Get(url+stringFromUser))
        {

            yield return sending.Send();

            if (sending.error != null)
            {
                Debug.Log("chatbot error:"+sending.error);

            }
            else
            {
                stringFromChatbot = sending.downloadHandler.text;
                stringFromChatbot = Regex.Replace(stringFromChatbot, @"\n", ""); // remove newline
                obj = Instantiate(chatbotMessagePrefab);
                obj.transform.GetChild(0).GetComponent<Text>().text = stringFromChatbot;
                obj.transform.SetParent(dialogParent.transform);
                Debug.Log("start checking");
                yield return new WaitForEndOfFrame();
                obj.GetComponent<WidthConstraint>().checkWidth();
                scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
            }

        }
    }

    void displayChatbotMessage() {
        

        //StartCoroutine(adjustDialogSize()) ;
    }

    
	
	// Update is called once per frame
	void Update () {
	
	}
}
