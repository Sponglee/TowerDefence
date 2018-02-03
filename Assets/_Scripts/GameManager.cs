using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
  

    // property to access this from other scripts
    public TowerBtn ClickedBtn { get; private set; }


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PickTower(TowerBtn towerBtn)
    {
        this.ClickedBtn = towerBtn;
    }

    public void BuyTower()
    {
        ClickedBtn = null;
    }
}
