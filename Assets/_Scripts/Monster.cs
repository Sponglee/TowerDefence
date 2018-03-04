using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {

    [SerializeField]
    private float speed = 1f;
    //path 
    private Stack<Node> path;
    // position
    public Point GridPosition { get; set; }
    // set destination (not final point, but next point from stack
    private Vector3 destination;
    //Spawn trigger (don't move before spawn finishes)
    public bool IsActive { get; set; }
    //Declare a final path stack to backtrack the path
    Stack<Node> MonsterlPath = new Stack<Node>();

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
        Debug.Log("CUR: " + CurrentTilePos.X + " " + CurrentTilePos.Y);
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
                Debug.Log("ISDEAD");
                // }

            }

        }


    }

    //Recalculates a path for a monster which had an obscured path and now it's open
    public void RePath()
    {
        Debug.Log("REPATH");
        //Reset previous path
        path = null;
        //Toggle switch of start to this current tile
        AStar.firstCurrent = true;
        //this current start tile

        //<----

        //Toggle off obscured ASTar path
        //AStar.NewGoal = false;
        //Clear obstacles
        AStar.Obstacles.Clear();
        //Generate New path from this place
        path = LevelManager.Instance.GeneratePath(CurrentTilePos);
        //Set it as Path for this Monster
        SetPath(path);


        //Move 
        //Move();

        //<---- wrong

    }


    //Spawn monster
    public void Spawn(int gmhealth)
    {

        LevelManager.Instance.GeneratePath(LevelManager.Instance.BlueSpawn);
        health = maxHealth;


        hp.text = health.ToString();

        //health.Initialize();

        /*set initial health to 10
        this.health.MaxVal = 10;
        this.health.CurrentVal = this.health.MaxVal;
        */

        transform.position = LevelManager.Instance.BluePortal.transform.position;
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(1f, 1f, 0.1f), false));
        
        SetPath(LevelManager.Instance.Path);

    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        health = maxHealth;
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
            GameManager.Instance.Lives -= 1;

        //}
        //// LEft or Up
        //if ((other.CompareTag("MonsterBound") && other.transform.position.x > this.transform.position.x)
        //            || (other.CompareTag("MonsterBound") && other.transform.position.y < this.transform.position.y))
        //{
           
            
        //    this.speed = 0.1f;}
           
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.CompareTag("MonsterBound") && other.transform.position.x > this.transform.position.x)
                     || (other.CompareTag("MonsterBound") && other.transform.position.y < this.transform.position.y))
        {

            //direction vector from collider to destination
            Vector2 dir = destination - other.transform.position;
            //direction between colliders
            Vector2 dir2 = other.transform.position - transform.position;
            Vector2 dir3 = dir - dir2;
            //direction turned to degrees
            float angle = Mathf.Atan2(dir3.y, dir3.x) * Mathf.Rad2Deg;
            Debug.Log(angle);



            
            
                this.speed = Random.Range(0.001f,0.1f);
                
                    if (other.GetComponentInParent<Monster>())
                        other.GetComponentInParent<Monster>().speed = curr_speed;
                    else
                        this.speed = curr_speed;
                
           
           


        }

    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MonsterBound"))
        {
            Debug.Log("Exit");
            this.speed = curr_speed;
        }
      
       
    }


    //Resets disabled object 
    private void Release()
    {
        health = maxHealth;
        hp.text = health.ToString();
        //Stop before scaling is done
        IsActive = false;
        GridPosition = LevelManager.Instance.BlueSpawn;
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
              
                Release();
                
            }
        }
    }

    private IEnumerator SlowDown()
    {
        yield return new WaitForSeconds(Random.Range(0.1f,1f));
    }

   

   

  
}
