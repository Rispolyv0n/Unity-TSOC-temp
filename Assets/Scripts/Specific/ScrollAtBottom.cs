using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// to let the scrollView always show the bottom of the content

public class ScrollAtBottom : MonoBehaviour {

    private ScrollRect myScrollRect;

    // Use this for initialization
    void Start () {
        myScrollRect = GetComponent<ScrollRect>();
        myScrollRect.verticalNormalizedPosition = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        myScrollRect.verticalNormalizedPosition = 0f;
    }
}
