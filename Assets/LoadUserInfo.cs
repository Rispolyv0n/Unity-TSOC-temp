using UnityEngine;
using System.Collections;

public class LoadUserInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(PlayerInfo.downloadUserInfo());
        
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
