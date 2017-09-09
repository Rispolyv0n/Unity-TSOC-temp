using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SaveObjOnLoad : MonoBehaviour {

    static SaveObjOnLoad obj;

    private void Awake()
    {
        
        if (obj == null)
        {
            obj = this;
            DontDestroyOnLoad(this);
        }
        else if (this != obj) {
            Destroy(gameObject);
        }
        
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
