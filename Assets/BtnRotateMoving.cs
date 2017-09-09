using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach on the setting button in the ownerMenu scene
// control the button moving, rotating, and the setting panel activating.

public class BtnRotateMoving : MonoBehaviour {

    private Button thisBtn;
    public GameObject settingPanel;

    private float speed; // for moving
    private float rotateSpeed;

    private bool rollLeft;
    private bool rollRight;

    private Vector3 openPos;
    private Vector3 closePos;

    private Vector3 targetPos;

	// Use this for initialization
	void Start () {
        rollLeft = false;
        rollRight = false;

        speed = 700;
        rotateSpeed = 500;

        thisBtn = this.GetComponent<Button>();
        thisBtn.onClick.AddListener(rotateAndMove);

        openPos = new Vector3(-300, thisBtn.transform.localPosition.y, 0);
        closePos = new Vector3(0, thisBtn.transform.localPosition.y, 0);
    }

    private void rotateAndMove() {
        if (thisBtn.transform.localPosition.x <= openPos.x)
        {
            targetPos = closePos;
            rollRight = true;
        }
        else {
            targetPos = openPos;
            rollLeft = true;
        }
    }

	// Update is called once per frame
	void Update () {
        if (rollLeft)
        {
            thisBtn.transform.localPosition = Vector3.MoveTowards(thisBtn.transform.localPosition, targetPos, speed * Time.deltaTime);
            thisBtn.transform.Rotate(Vector3.forward*rotateSpeed*Time.deltaTime);
            if (thisBtn.transform.localPosition.x <= targetPos.x)
            {
                settingPanel.SetActive(true);
                rollLeft = false;
            }

        }
        else if (rollRight) {
            thisBtn.transform.localPosition = Vector3.MoveTowards(thisBtn.transform.localPosition, targetPos, speed * Time.deltaTime);
            thisBtn.transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
            settingPanel.SetActive(false);
            if (thisBtn.transform.localPosition.x >= targetPos.x)
            {
                rollRight = false;
            }
        }
        
	}
}
