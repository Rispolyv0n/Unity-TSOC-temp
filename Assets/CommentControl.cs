using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CommentControl : MonoBehaviour {

    public Button star_1;
    public Button star_2;
    public Button star_3;
    public Button star_4;
    public Button star_5;

    private Button[] starBtns;

    public InputField comment;

    public Button sendBtn;

    private int stars;
    private string commentContent;

    // Use this for initialization
    void Start () {
        stars = 3;
        starBtns = new Button[5] {star_1, star_2, star_3, star_4, star_5};
        star_1.onClick.AddListener(delegate { starClick(1); });
        star_2.onClick.AddListener(delegate { starClick(2); });
        star_3.onClick.AddListener(delegate { starClick(3); });
        star_4.onClick.AddListener(delegate { starClick(4); });
        star_5.onClick.AddListener(delegate { starClick(5); });
        sendBtn.onClick.AddListener(sendComment);
    }

    void starClick(int starNum) {
        foreach (Button btn in starBtns) {
            btn.GetComponent<Image>().color = new Color(222 / 255f, 222 / 255f, 222 / 255f);
        }
        for (int i = 0; i < starNum; ++i) {
            starBtns[i].GetComponent<Image>().color = new Color(244 / 255f, 201 / 255f, 118 / 255f);
        }
        stars = starNum;
    }

    void sendComment() {
        commentContent = comment.text;
        Debug.Log(stars + ":" + commentContent);
        // send the request
        // close the scene : remember to remove the sceneSwitch component on the send Button
    }

	// Update is called once per frame
	void Update () {
	
	}
}
