using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class updateTextOfStoreObj : MonoBehaviour {

    public int setChildCount;

	// Use this for initialization
	void Start () {

        if(setChildCount == 1)
        {
            gameObject.transform.GetComponent<Text>().text = gameObject.transform.parent.GetComponent<ARStoreObject>().m_storeName;

            //ARStoreObject storeObj = gameObject.transform.parent.GetComponent<ARStoreObject>();
            //Text a = gameObject.transform.GetComponent<Text>();
            //a.text = storeObj.m_storeName;
        }
        else if(setChildCount == 2)
        {
            gameObject.transform.GetComponent<Text>().text = gameObject.transform.parent.GetComponent<ARStoreObject>().m_storeIntro;

        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
