using UnityEngine;
using System.Collections;

public class TryVisible : MonoBehaviour {

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.material.mainTexture = spriteRenderer.sprite.texture;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
