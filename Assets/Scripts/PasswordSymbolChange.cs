using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PasswordSymbolChange : MonoBehaviour {

    private InputField getField;
	// Use this for initialization
	void Start () {
        getField = GetComponent<InputField>();
        getField.asteriskChar = '●';
    }

	// Update is called once per frame
	void Update () {
        
    }
}
