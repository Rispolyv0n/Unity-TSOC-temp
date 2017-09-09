using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BlockPanelControl : MonoBehaviour {

    private Dropdown thisObj;
    public GameObject blockPanel;

	// Use this for initialization
	void Start () {
        thisObj = GetComponent<Dropdown>();
        thisObj.onValueChanged.AddListener(settingBlock);
	}

    private void settingBlock(int arg0)
    {
        if (arg0 == 0)
        {
            blockPanel.SetActive(false);
        }
        else
        {
            blockPanel.SetActive(true);
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
