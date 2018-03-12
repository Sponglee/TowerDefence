using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {

    [SerializeField]
    private float speed = 1f;
    
    //Boss toggle
    private bool IsBoss=false;
    //path 
    private Stack<Node> path;
    // position
    public Point GridPosition { get; set; }
    // set destination (not final point, but next point from stack
    private Vector3 destination;
    //Spawn trigger (don't move before spawn finishes)
    [SerializeField]
    public bool IsActive { get; set; }
    //Declare a final path stack to backtrack the path
    //Stack<Node> MonsterlPath = new Stack<Node>();

    MonsterRange monsterRange;
    // id of tower that is being damaged
    public int tow_id;

    private float curr_speed;
    private bool slowed = false;
    public bool Slowed
    {
        get
        {
            return slowed;
        }

        set
        {
            slowed = value;
        }
    }
    private bool mRePath;
    public bool MRePath
    {
        get
        {
            return mRePath;
        }

        set
        {
            mRePath = value;
        }
    }


    TowerHP tar;

    private Point currentTilePos;
    public Point CurrentTilePos
    {
        get
        {
            return currentTilePos;
        }

        set
        {
            currentTilePos = value;
        }
    }



    [SerializeField]
    private Text hp;

    /*Stat class
    [SerializeField]
    private Stat health;
    */
    [SerializeField]
    private int health;

    private int maxHealth = 15;


    public void Start()
    {
        this.curr_speed = speed;
        health = maxHealth;
        monsterRange = this.GetComponentInChildren<MonsterRange>();
        this.MRePath = false;
    }
    private void Update()
    {
      
        hp.text = health.ToString();
        Move();
        //If tower died (and is target?)

        TowerHP tar = this.GetComponentInChildren<MonsterRange>().Target;
        if (tar != null)
        {
            if (tar.IsDead) // <----
            {

                //Toggle Repath algorythm in GameManager
                //if (monsterRange.Target == null)
                //{
                //   Debug.Log("NO TARGET mTarget: " + monsterRange.mTarget);
                //    monsterRange.mTarget = true;
                //}

                //else
                //{
                this.MRePath = true;
              
                // }

            }

        }


    }

    //Recalculates a path for a monster which had an obscured path and now it's open
    public void RePath()
    {
       
        //Reset previous path
        path = null;
        //Toggle switch of start to this current tile
        AStar.firstCurrent = true;
        //this current start tile

        //<----
        AStar.NewGoal = false;

        //this.GetComponent<MonsterRange>().Target = null;
        //Clear obstacles
        AStar.Obstacles.Clear();
        
        path = LevelManager.Instance.GeneratePath(CurrentTilePos);
        //Set it as Path for this Monster
        SetPath(path);


        //Move 
        //Move();

        //<---- wrong

    }


    //Spawn monster
    public void Spawn(int gmhealth, int boss = 0)
    {
        
        maxHealth += gmhealth;
        health = maxHealth;
        hp.text = health.ToString();
        //int rng = Random.Range(0, LevelManager.Instance.BlueSpawn.Length);
        int rng = 0;
        //Debug.Log(" >>>" + LevelManager.Instance.BlueSpawn[rng].X + "." + LevelManager.Instance.BlueSpawn[rng].Y);
        LevelManager.Instance.GeneratePath(LevelManager.Instance.BlueSpawn[rng]);


        //health.Initialize();

        /*set initial health to 10
        this.health.MaxVal = 10;
        this.health.CurrentVal = this.health.MaxVal;
        */

        if (boss == 1)
        {
            IsBoss = true;
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            health = maxHealth * 10;
            maxHealth = health;
          
            hp.text = health.ToString();
            StartCoroutine(Scale(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(4f, 4f, 0.1f), false));
        }
           
        else 
            StartCoroutine(Scale(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(1f, 1f, 0.1f), false));

        transform.position = LevelManager.Instance.BluePortal.transform.position;
        SetPath(LevelManager.Instance.Path);

    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        float progress = 0;
        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime;
            yield return null;
        }
        transform.localScale = to;
        IsActive = true;
        if (remove)
            Release();
    }

    //Move along the path
    private void Move()
    {
        if (IsActive)
        {

            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
                else if (path.Count == 0 && !monsterRange.Target && GridPosition != LevelManager.Instance.RedSpawn)
                {
                    this.MRePath = true;
                    this.GetComponentInChildren<MonsterRange>().anim.SetBool("attack", false);
                }
            }
        }

    }

    //Sets a path 
    private void SetPath(Stack<Node> newPath)
    {
        if (newPath != null)
        {
            path = newPath;
            if (path.Count > 0)
            {
                GridPosition = path.Peek().GridPosition;
                destination = path.Pop().WorldPosition;
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("RedPortal") && gameObject.tag == "Monster")
        {
            StartCoroutine(Scale(new Vector3(1f, 1f, 0.1f), new Vector3(0.1f, 0.1f, 0.1f), true));
            //DAMAGE US
            
            this.GetComponentInChildren<MonsterRange>().Target = null;
            Release();
            if (IsBoss)
            {
                GameManager.Instance.Lives = 0;
            }
                
            else
                GameManager.Instance.Lives -= 1;

            

        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {


       if (other.CompareTag("Monster"))
        {
            if ((other.CompareTag("MonsterBound") && other.transform.position.x > this.transform.position.x)
                    || (other.CompareTag("MonsterBound") && other.transform.position.y < this.transform.position.y))
            {
                this.speed = curr_speed;
                this.speed = Random.Range(0.1f, 0.2f);
                float time=0;
                time +=  Time.deltaTime;

                if (time >= 0.2)
                    this.speed = curr_speed;
            }
        }


    }
        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("MonsterBound"))
            {
               
                this.speed = curr_speed;
            }


        }


        //Resets disabled object 
        private void Release()
        {
         
        //int rng = Random.Range(0,LevelManager.Instance.BlueSpawn.Count);
        int rng = 0;
        
       
        //Stop before scaling is done
        IsActive = false;
        GridPosition = LevelManager.Instance.BlueSpawn[rng];
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        //Romove itself from the active monsterlist
        GameManager.Instance.RemoveMonster(this);
    }

    public void TakeDamage(int damage)
    {
        if (IsActive)
        {
            health -= damage;
            hp.text = health.ToString();
          
            if(health <=0)
            {
              
                
                
                if (IsBoss)
                {

                    gameObject.GetComponentInChildren<MonsterRange>().anim.SetBool("attack",false);
                    gameObject.GetComponentInChildren<MonsterRange>().anim.SetTrigger("death");
                    this.speed = 0.00f;
                    gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    this.GetComponentInChildren<MonsterRange>().Target = null;
                    StartCoroutine(BossMana());
                    GameManager.Instance.backGroundMusic.GetComponent<AudioSource>().Stop();
                    GameManager.Instance.backMusic = Resources.Load<AudioClip>("YetAnotherJourney");
                    GameManager.Instance.backGroundMusic.GetComponent<AudioSource>().Play();
                    this.speed = 1f;
                    health = maxHealth;
                }
                   
                else
                {
                    GameManager.Instance.SpawnMana(this);
                    Release();
                }
                

            }
        }
    }

    private IEnumerator SlowDown()
    {
    
        this.speed = curr_speed;
        yield return new WaitForSeconds(Random.Range(1f,2f));
    }

   private IEnumerator BossMana()
    {
        for (int indexator = 0; indexator < 15; indexator++)
        {
            Debug.Log("HEY");
            GameManager.Instance.SpawnMana(this,1);
            SoundManager.PlaySound("drop");
            yield return new WaitForSeconds(Random.Range(0.12f, 0.3f));
        }

        Release();
        

    }

   

  
}
