using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SetUpdateTime : MonoBehaviour {

    private Button thisBtn;

	// Use this for initialization
	void Start () {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(setTime);
        
	}

    void setTime() {
        OwnerInfo.updateTime = DateTime.Now;
        OwnerInfo.hasUpdated = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
