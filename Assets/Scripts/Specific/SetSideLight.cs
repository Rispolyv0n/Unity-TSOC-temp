using UnityEngine;
using System.Collections;

// set the side light & decide if the spotlights of events should be lit up

public class SetSideLight : MonoBehaviour
{

    public GameObject obj_plane;
    public GameObject anchor_topLeft; // y:0.2
    public GameObject anchor_bottomRight;// y:0.2

    private GameObject obj_sideLight; // y:0.2
    private float bound; // for a dark area near the end

    // Use this for initialization
    void Start()
    {
        // instantiate the sideLight prefab
        obj_sideLight = Resources.Load("Prefabs/Spotlight_side") as GameObject;

        float height = anchor_topLeft.transform.position.y;
        bound = 15;
        while (height > anchor_bottomRight.transform.position.y + bound)
        {
            GameObject obj_leftLight = (GameObject)Instantiate(obj_sideLight, new Vector3(anchor_topLeft.transform.position.x, height, anchor_bottomRight.transform.position.z), Quaternion.Euler(0, 90, 0));
            GameObject obj_rightLight = (GameObject)Instantiate(obj_sideLight, new Vector3(anchor_bottomRight.transform.position.x, height, anchor_bottomRight.transform.position.z), Quaternion.Euler(0, -90, 0));
            height -= 5;
        }

        // setting which light should be turned off
        GameObject[] lights = GameObject.FindGameObjectsWithTag("spotlight");
        bool lit = false;
        foreach (GameObject obj_light in lights)
        {
            for (int i = 0; i < PlayerInfo.eventCollection.Count; ++i)
            {
                if (PlayerInfo.eventCollection[i].id == obj_light.transform.parent.gameObject.GetComponent<PanelEventInfoControl>().eventNum)
                {
                    lit = true;
                    break;
                }
            }
            if (!lit)
            {
                obj_light.SetActive(false);
            }
            lit = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
