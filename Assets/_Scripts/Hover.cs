using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover> {
    //Variable-sprite
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start ()
    {
        //instantiating the sprite
        this.spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        FollowMouse();
	}
    //Makes sprite on Hover follow your mouse
    private void FollowMouse()
    {
        if(spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
      
    }
    // Making our mouse an icon
    public void Activate(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        this.spriteRenderer.sprite = sprite;
        
    }

    //Discard icon
    public void Deactivate()
    {
        spriteRenderer.enabled = false;
        // set clickedBtn to empty from this script (GameManager line :10)
        GameManager.Instance.ClickedBtn = null;
    }
}
