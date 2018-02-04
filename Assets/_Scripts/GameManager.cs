using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

    //money
    private int currency;

    [SerializeField]
    private Text currencyTxt;

    //incapsulation of currency with property
    public int Currency
    {
        get
        {
            return currency;
        }

        set
        {
            currency = value;
            this.currencyTxt.text = value.ToString() + " <color=lime>$</color>";
        }
    }


    // property to access this from other scripts
    public TowerBtn ClickedBtn { get; set; }

    // Use this for initialization
    void Start ()
    {
        Currency = 5;

	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleEscape();
	}

    //Activate tower placement whichever button is pressed
    public void PickTower(TowerBtn towerBtn)
    {
        if(Currency >= towerBtn.Price)
        {
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }
       
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
        if(Currency>= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;
            Hover.Instance.Deactivate();
            ClickedBtn = null;
        }
     
    }


    
}
