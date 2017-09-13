using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomPosition : MonoBehaviour {

    private Image thisObj;
    private float upperBound;
    private float lowerBound;
    private float boundRange;

	// Use this for initialization
	void Start () {
        thisObj = GetComponent<Image>();
        upperBound = 465.0f;
        lowerBound = 50.0f;
        boundRange = upperBound - lowerBound;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setRandomPosition() {
        Random.seed = System.Guid.NewGuid().GetHashCode();
        int getRandomInt = Random.Range(1,11);
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, getRandomInt * 41.5f - 323.5f, this.transform.localPosition.z);
    }
}
