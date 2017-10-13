using UnityEngine;
using System.Collections;

public class objFaceToCamera : MonoBehaviour
{
    private bool isStoreObj = false;
    //private Quaternion rot;

    // Use this for initialization
    void Start()
    {
        /*
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
            */  
    }
    //public Camera m_Camera;

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "storeInfoObj")
        {
            if(isStoreObj == false)
            {
                transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                Camera.main.transform.rotation * Vector3.up);
                //rot = gameObject.transform.rotation;
                isStoreObj = true;
                //Debug.Log("RRRRRRRR");
            }
            else
            {
                //transform.LookAt
                gameObject.GetComponent<objFaceToCamera>().enabled = false;
            }
            
            //transform.LookAt(transform.position);
            //transform.LookAt(gameObject.transform);
            //Debug.Log("RRRRRRRR");
        }
        else
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
        }
        
        /*
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
            //Debug.Log("not the store obj");
         */
        
    }
}
