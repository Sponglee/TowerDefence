using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
  
    

    // property to access this from other scripts
    public TowerBtn ClickedBtn { get; set; }


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleEscape();
	}

    //Activate tower placement whichever button is pressed
    public void PickTower(TowerBtn towerBtn)
    {
        this.ClickedBtn = towerBtn;
        Hover.Instance.Activate(towerBtn.Sprite);
    }

    // discard the tower
    private void HandleEscape()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Hover.Instance.Deactivate();
        }
    }

    // handles buy 
    public void BuyTower()
    {
        Hover.Instance.Deactivate();
        ClickedBtn = null;
    }
}
