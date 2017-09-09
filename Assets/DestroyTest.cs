using UnityEngine;
using System.Collections;

public class DestroyTest : MonoBehaviour {

    public GameObject obj;

    // Use this for initialization
    void Start () {

        
	}

    private void OnDestroy()
    {
        Destroy(obj);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
