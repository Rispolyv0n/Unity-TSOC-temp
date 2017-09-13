using UnityEngine;
using System.Collections;
using Lean.Touch;

public class rotateMarker : MonoBehaviour {
    Vector3 previousPosition;
    Vector3 offsetValue;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LeanTouch.Fingers.Count == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                previousPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                offsetValue = Input.mousePosition - previousPosition;
                previousPosition = Input.mousePosition;
                if (offsetValue.y > 0)
                {
                    transform.Rotate(Vector3.forward, offsetValue.magnitude, Space.Self);
                }
                if (offsetValue.y < 0)
                {
                    transform.Rotate(Vector3.back, offsetValue.magnitude, Space.Self);
                }
            }
        }
    }
}
