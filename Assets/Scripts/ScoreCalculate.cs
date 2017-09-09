using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on the result_text on result_panel in the scene game_1
// calculate the score according to the transform.position.y of the beverage & limit
// score range: 1~10 

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
        int standard = 120; // beverage_pos.y + standard = limit_pos.y
        int point = (int)Mathf.Abs(limit.transform.position.y- (beverage.transform.position.y + standard));
        if (point > 100) return 1;
        point = (100-point)/9;
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
