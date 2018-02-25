using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    //money
    private int currency;
    [SerializeField]
    private Text currencyTxt;

    //Object reference for hover icon

    [SerializeField]
    private GameObject hoverRange;

    //Our hp
    private int lives;
    [SerializeField]
    private Text livesTxt;

    //wave number
    private int wave = 0;
    [SerializeField]
    private Text waveTxt;
    [SerializeField]
    private GameObject waveBtn;

    //current selected tower
    private Tower selectedTower;

    //toggle for RePathing on death
    
    public bool cRePath = false;

    //Check if ther're are monsters on the field
    public bool WaveActive
    {
        get
        {
            //If no monsters in ActiveMonsters list - return false
            return ActiveMonsters.Count > 0;
        }
    }
    //GameOver checker
    private bool gameOverChecker = false;
    public bool GameOverChecker
    {
        get
        {
            return gameOverChecker;
        }
    }

    [SerializeField]
    private GameObject gameOverMenu;

    //Health of monsters
    [SerializeField]
    private int gmHealth;


    //List to check if the wave is over
    private List<Monster> activeMonsters = new List<Monster>();
    public List<Monster> ActiveMonsters
    {
        get
        {
            return activeMonsters;
        }

     
    }
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
            this.currencyTxt.text = value.ToString() + " <color=orange>$</color>";
        }
    }

    //set property for our hp
    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            //set value hp if not 0
            this.lives = value;
            livesTxt.text = string.Format("Health: <color=orange>{0}</color>", lives.ToString());
            //Check for gameover on set
            if (lives <= 0)
            {
                this.lives = 0;
                GameOver();
            }
        }
    }

    // property to access this from other scripts
    public TowerBtn ClickedBtn { get; set; }




    //Before start
    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();      
    }
    // Use this for initialization
    void Start()
    {
        Lives = 10;
        Currency = 15;
    }

    // Update is called once per frame
    void Update()
    {
       
        //If there's need to Recalculate a Path - do it for every active monster
        if (cRePath && AStar.NewGoal == true)
        {
            foreach (Monster monster in ActiveMonsters)
            {
                //Recalculate a Path from the point monster is standing atm
                monster.GetComponent<Monster>().RePath();
            }
            //Reset "non bluePortal" toggle
            AStar.cTmp = null;
            
            cRePath = false;
        }
        HandleEscape();
    }

    //Activate tower placement whichever button is pressed
    public void PickTower(TowerBtn towerBtn)
    {
        if (Currency >= towerBtn.Price && !WaveActive && !gameOverChecker)
        {
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
            //Gets hover range size for each button
            hoverRange.transform.localScale = towerBtn.TowerPrefab.transform.GetChild(0).transform.localScale;
        }

    }

    // discard the tower
    private void HandleEscape()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Hover.Instance.Deactivate();
        }
    }

    //Selects tower
    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = tower;
        //toggle spriteRenderer for range(based on current selection)
        selectedTower.Select();
    }

    //Deselect tower
    public void DeselectTower()
    {
        if(selectedTower !=null)
        {
            selectedTower.Select();
        }
        selectedTower = null;
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
    /*
     * 
     *           
     *           Once tower dies - last of many wave monsters isn't on the "CURRENT" tile yet
     *           and rushes there through air
     *           
     *           
     *           ADD way to either guide last monster TO that node first or get path for each monster seperately
     *           
     *           OR (!) courutine to start moving from there like spawnWave (!) <- need this anyway
     *           
     *           
     *           !! MAKE TARGET CHOICE AFFECTED BY F SCORE?
     *    
     *           !!!!( MULTIPLE DEATHS  OF OBSTACLES in 1 WAVE BREAKS THE LOGIC))
     *           !!! Plus make ground around portals unwalkable
     * 
     * */

    //Spawn waves of monsters
    public void StartWave()
    {
        wave++;
        Debug.Log(LevelManager.Instance.BlueSpawn.X + " " + LevelManager.Instance.BlueSpawn.Y);
        //Set Recalc Path toggle off each wave
        cRePath = false;
        //AStar.NewGoal = true;
        //increase difficulty every 3rd wave
        if (wave % 3 == 0)
        {
            gmHealth += 5;
        }

        waveTxt.text = string.Format("Wave: <color=orange>{0}</color>", wave);

        StartCoroutine (SpawnWave());

        waveBtn.SetActive(false);
    }


   //Monster spawn
   private IEnumerator SpawnWave()
    {
        
        LevelManager.Instance.GeneratePath(LevelManager.Instance.BlueSpawn);
        for (int i = 0; i < wave; i++)
        {
            int monsterIndex = Random.Range(0, 2);
            string type = string.Empty;
            switch (monsterIndex)
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
            monster.Spawn(gmHealth);
            //Added to list of active monsters to check for waves
            ActiveMonsters.Add(monster);
            yield return new WaitForSeconds(2.5f);
        }  
    }

    //Removes monsters from activeMonster list for wavechecks
    public void RemoveMonster(Monster monster)
    {
        //Add monster to active monster list
        ActiveMonsters.Remove(monster);
        if (!WaveActive && !GameOverChecker)
        {
            waveBtn.SetActive(true);
        }
    }

    //Show gameOver menu 
    public void GameOver()
    {
        if(!GameOverChecker)
        {
            gameOverChecker = true;
            gameOverMenu.SetActive(true);
        }
    }

    //Restarts game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //In case game was paused before
        Time.timeScale = 1;
    }

    //Quit
    public void Quit()
    {
        Application.Quit();
    }
}
