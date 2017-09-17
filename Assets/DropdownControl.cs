using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DropdownControl : MonoBehaviour {

    public Dropdown category_1;

    public Dropdown d_no;
    public Dropdown d_entertainment;
    public Dropdown d_restaurant;
    public Dropdown d_shopping;
    public Dropdown d_transportation;

    // Use this for initialization
    void Start () {
        category_1.onValueChanged.AddListener(changeDropdown);
	}

    private void changeDropdown(int arg0)
    {
        GameObject[] dds = GameObject.FindGameObjectsWithTag("dropdowns");
        foreach(GameObject obj in dds)
        {
            obj.SetActive(false);
        }
        switch (category_1.value) {
            case 0:
                d_entertainment.gameObject.SetActive(true);
                break;
            case 1:
                d_restaurant.gameObject.SetActive(true);
                break;
            case 2:
                d_shopping.gameObject.SetActive(true);
                break;
            case 6:
                d_transportation.gameObject.SetActive(true);
                break;
            default:
                d_no.gameObject.SetActive(true);
                break;
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
