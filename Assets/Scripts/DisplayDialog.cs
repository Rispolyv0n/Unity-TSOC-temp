using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Experimental.Networking;
using System.Text.RegularExpressions;

// attached on the button_send in the chatbot scene
// to get the user input and display it in the scrollView
// send the user input to http request, get the chatbot reply message and display it

public class DisplayDialog : MonoBehaviour
{

    private Button btn_send;

    public InputField user_input;
    public GameObject dialogParent;
    public GameObject scrollView;

    private GameObject chatbotMessagePrefab;
    private GameObject dialog_user_prefab;
    private GameObject obj_u;
    private GameObject obj_c;

    private string url;
    private string stringFromUser;
    private string stringFromChatbot;


    // Use this for initialization
    void Start()
    {
        btn_send = GetComponent<Button>();
        dialog_user_prefab = Resources.Load<GameObject>("Prefabs/Image_userDialog");
        chatbotMessagePrefab = Resources.Load<GameObject>("Prefabs/Image_chatbotDialog");
        url = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/chatbot?str=";

        btn_send.onClick.AddListener(delegate { StartCoroutine(getInputAndDisplay()); });
    }

    IEnumerator getInputAndDisplay()
    {
        obj_u = Instantiate(dialog_user_prefab);
        obj_u.transform.GetChild(0).GetComponent<Text>().text = user_input.text;
        obj_u.transform.SetParent(dialogParent.transform);

        // rearrange the width of the dialog box in the end of the frame
        yield return new WaitForEndOfFrame();
        obj_u.GetComponent<WidthConstraint>().checkWidth();
        scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;


        stringFromUser = user_input.text;
        user_input.text = "";

        bool hasReceivedMessage = false;
        using (UnityWebRequest sending = UnityWebRequest.Get(url + stringFromUser))
        {

            yield return sending.Send();

            if (sending.error != null)
            {
                Debug.Log("chatbot error:" + sending.error);

            }
            else
            {
                stringFromChatbot = sending.downloadHandler.text;
                stringFromChatbot = Regex.Replace(stringFromChatbot, @"\n", ""); // remove newline
                obj_c = Instantiate(chatbotMessagePrefab);
                obj_c.transform.GetChild(0).GetComponent<Text>().text = stringFromChatbot;
                obj_c.transform.SetParent(dialogParent.transform);

                hasReceivedMessage = true;
            }

        }
        if (hasReceivedMessage)
        {
            yield return new WaitForEndOfFrame(); // don't know why waitforendofframe should appear twice, wait until next end of frame?
            StartCoroutine(adjusting_c());
        }


    }

    IEnumerator adjusting_c()
    {
        yield return new WaitForEndOfFrame();
        obj_c.GetComponent<WidthConstraint>().checkWidth();
        scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }
    // Update is called once per frame
    void Update()
    {

    }


}
