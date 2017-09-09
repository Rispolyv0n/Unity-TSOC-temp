using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

// attach on image obj in the instruction scenes
// click img to change the img to the next
// then switch to home/street scene if it's the last img

public class ClickAndChangeImg : MonoBehaviour , IPointerClickHandler{

    private Image thisImg;
    private Sprite img;
    private string imgName;
    private int nowPage;
    
    public string imgFolderPath;
    public int maxPage;
    public string sceneName;
    

	// Use this for initialization
	void Start () {
        nowPage = 1;
        thisImg = GetComponent<Image>();
        thisImg.overrideSprite = Resources.Load<Sprite>(imgFolderPath+"00");
	}

    private void OnEnable()
    {
        nowPage = 1;
        thisImg = GetComponent<Image>();
        thisImg.overrideSprite = Resources.Load<Sprite>(imgFolderPath + "00");
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (nowPage <= maxPage)
        {
            string fullPath = "";
            fullPath += imgFolderPath;
            fullPath += nowPage.ToString("D2");
            img = Resources.Load<Sprite>(fullPath);
            thisImg.overrideSprite = img;
            nowPage++;
        }
        else
        {
            if (sceneName.Equals("home"))
            {
                SetHaveGoneHome a = new SetHaveGoneHome();
                a.setHasGoneHome();
            }
            else if (sceneName.Equals("street"))
            {
                SetHaveGoneStreet a = new SetHaveGoneStreet();
                a.setHasGoneStreet();
            }

            if (sceneName.Equals("none"))
            {
                transform.parent.gameObject.SetActive(false);
                return;
            }
            else {
                SceneManager.LoadScene(sceneName);
            }
            
            
        }
    }
}
