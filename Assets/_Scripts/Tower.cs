using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    private SpriteRenderer mySpriteRenderer;

    // Use this for initialization
    void Start() {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log(mySpriteRenderer);
    }

    // Update is called once per frame
    void Update() {

    }
    // toggle Tower's range
    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        
    }
}
