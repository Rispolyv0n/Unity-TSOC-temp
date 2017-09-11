using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on the dialog prefab
// to constrain the width, auto expand the height when the text is long enough

public class WidthConstraint : MonoBehaviour {

    public Text text;
    private LayoutElement b;

    public int widthConstraint_text;

    // Use this for initialization
    void Start () {
       

    }

    // called by the send button in the scene chatbot
    public void checkWidth() {
        Debug.Log("checking:"+text.rectTransform.sizeDelta.x);
            if (text.rectTransform.sizeDelta.x > widthConstraint_text)
            {
                Debug.Log("oversize");
                b = text.GetComponent<LayoutElement>();
                if (b != null)
                {
                    Debug.Log("adjusting");
                    b.preferredWidth = widthConstraint_text;
                }
            }
        
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
