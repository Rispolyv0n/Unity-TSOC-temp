using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnChosen : MonoBehaviour {

    private Button thisBtn;

    public int id;
    private string name;
    public GameObject itsChoosingPanel;
    public Text nameText;
    public Image img_char;
    private GameObject control;
    private Sprite img;

	// Use this for initialization
	void Start () {
        thisBtn=GetComponent<Button>();
        thisBtn.onClick.AddListener(controlPanels);
        img = GamingInfo.characters[id].imgForChoosing;
        img_char.overrideSprite = img;
        name = GamingInfo.characters[id].name;
        nameText.text = name;
        control = GameObject.FindGameObjectWithTag("choose_control");
	}

    void controlPanels() {
        GameObject[] panels = GameObject.FindGameObjectsWithTag("choose_blackPanel");
        foreach (GameObject obj in panels) {
            obj.SetActive(false);
        }
        itsChoosingPanel.SetActive(true);
        control.GetComponent<GetChosenBtn>().choice = id;
        control.GetComponent<GetChosenBtn>().chosen = true;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
