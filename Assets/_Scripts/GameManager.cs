using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

    //money
    private int currency;


  
  

    [SerializeField]
    private Text currencyTxt;

    // Create monster object pool
    public ObjectPool Pool { get; set; }
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

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }
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

    //Spawn waves of monsters
    public void StartWave()
    {
        StartCoroutine (SpawnWave());

    }
   
   private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath();
        
            int monsterIndex = Random.Range(0, 2);
            string type = string.Empty;
            switch(monsterIndex)
            {
                case 0:
                    type = "orc";
                    break;
                case 1:
                    type = "goblin";
                    break;
            }
            //Grab Monster script from monster spawn and spawn it on portal
            Monster monster = Pool.GetObject(type).GetComponent<Monster>();
            monster.Spawn();

        yield return new WaitForSeconds(2.5f);
    }
}
