using UnityEngine;
using System.Collections;

public class setImageWidth : MonoBehaviour {

    private RectTransform nameRec;

    // Use this for initialization
    void Start () {
        nameRec = gameObject.transform.parent.transform as RectTransform;
	}
	
	// Update is called once per frame
	void Update () {
        RectTransform lineRec = gameObject.transform as RectTransform;
        lineRec.sizeDelta = new Vector2(nameRec.rect.width + 100f, lineRec.rect.height);
	}
}
