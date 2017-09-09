using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCalculate : MonoBehaviour {

    private Text thisText;
    public Image limit;
    public Image beverage;
    private int addWhat;
    private string whatValue;

	// Use this for initialization
	void Start () {
        Random.seed = System.Guid.NewGuid().GetHashCode();
        thisText = GetComponent<Text>();
        addWhat = Random.Range(0, 2);
        if (addWhat == 0)
        {
            whatValue = "智力";
        }
        else {
            whatValue = "體力";
        }
        thisText.text = "恭喜你獲得\n" + whatValue + " " + getPoint() +" 點\n太會喝了!!!";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private int getPoint() {
        Debug.Log("L:"+limit.transform.position.y);
        Debug.Log("B:"+beverage.transform.position.y);
        int standard = 120;
        int point = (int)Mathf.Abs(limit.transform.position.y- beverage.transform.position.y);
        if (point > 50 && beverage.transform.position.y > limit.transform.position.y) return 1;
        point = (100-Mathf.Abs(point-standard))/5;
        if (addWhat == 0)
        {
            PlayerInfo.value_intelligence += point;
        }
        else {
            PlayerInfo.value_strength += point;
        }
        
        return point;
    }
}
