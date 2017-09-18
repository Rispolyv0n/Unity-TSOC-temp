using UnityEngine;
using System.Collections;

public class MouseDragCamera : MonoBehaviour {
    private float speed;
    public bool dir_x;
    public bool dir_y;
    public bool dir_z;
    public float lowerBound; //-16
    public float higherBound;
    public bool mouseDir_x;
    public bool mouseDir_y;
    // Use this for initialization
    void Start () {
        speed = 10f;
        //bound = -16;
        //dir_x = false;
        //dir_y = false;
        //dir_z = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (dir_x == true)
        {
            if (mouseDir_x == true)
            {
                if (Input.GetMouseButton(0))
                {
                    transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed * Time.deltaTime, 0.0f, 0.0f);
                    if (transform.position.x < lowerBound)
                    {
                        transform.position = new Vector3(lowerBound, transform.position.y, transform.position.z);
                    }
                    if (transform.position.x > higherBound)
                    {
                        transform.position = new Vector3(higherBound, transform.position.y, transform.position.z);
                    }

                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    transform.position -= new Vector3(Input.GetAxis("Mouse Y") * speed * Time.deltaTime, 0.0f, 0.0f);
                    if (transform.position.x < lowerBound)
                    {
                        transform.position = new Vector3(lowerBound, transform.position.y, transform.position.z);
                    }
                    if (transform.position.x > higherBound)
                    {
                        transform.position = new Vector3(higherBound, transform.position.y, transform.position.z);
                    }
                }
            }
        }
        else if (dir_y == true)
        {
            if (mouseDir_x == true)
            {
                if (Input.GetMouseButton(0))
                {
                    transform.position -= new Vector3(0.0f, Input.GetAxis("Mouse X") * speed * Time.deltaTime, 0.0f);
                    if (transform.position.y < lowerBound)
                    {
                        transform.position = new Vector3(transform.position.x, lowerBound, transform.position.z);
                    }
                    if (transform.position.y > higherBound)
                    {
                        transform.position = new Vector3(transform.position.x, higherBound, transform.position.z);
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    transform.position -= new Vector3(0.0f, Input.GetAxis("Mouse Y") * speed * Time.deltaTime, 0.0f);
                    if (transform.position.y < lowerBound)
                    {
                        transform.position = new Vector3(transform.position.x, lowerBound, transform.position.z);
                    }
                    if (transform.position.y > higherBound)
                    {
                        transform.position = new Vector3(transform.position.x, higherBound, transform.position.z);
                    }
                }
            }
        }
        else if (dir_z == true) {
            if (mouseDir_x == true)
            {
                if (Input.GetMouseButton(0))
                {
                    transform.position -= new Vector3(0.0f, 0.0f, Input.GetAxis("Mouse X") * speed * Time.deltaTime);
                    if (transform.position.z < lowerBound)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, lowerBound);
                    }
                    if (transform.position.z > higherBound)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, higherBound);
                    }
                }
            }
            else {
                if (Input.GetMouseButton(0))
                {
                    transform.position -= new Vector3(0.0f, 0.0f, Input.GetAxis("Mouse Y") * speed * Time.deltaTime);
                    if (transform.position.z < lowerBound)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, lowerBound);
                    }
                    if (transform.position.z > higherBound)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, higherBound);
                    }
                }
            }
            
        }
        
	}
}
