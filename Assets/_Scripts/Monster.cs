using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {

    [SerializeField]
    private float speed=1f;
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
        health = maxHealth;
    }
    private void Update()
    {
      
        hp.text = health.ToString();
        Move();
        //If tower died
        if (TowerHP.IsDead)
        {
            //Toggle Repath algorythm in GameManager
            GameManager.Instance.cRePath = true;
        }
        
    }

    //Recalculates a path for a monster which had an obscured path and now it's open
    public void RePath()
    {
        //Reset previous path
        LevelManager.Instance.Path = null;
        //Toggle switch of start to this current tile
        AStar.firstCurrent = true;
        //this current start tile

            //<----
          
        //Toggle off obscured ASTar path
        //AStar.NewGoal = false;
        //Generate New path from this place
        LevelManager.Instance.GeneratePath(CurrentTilePos);
        //Set it as Path for this Monster
        SetPath(LevelManager.Instance.Path);
        Debug.Log(LevelManager.Instance.Path.Count + " <- MONSTER");
        
        //Move 
        //Move();
        
        TowerHP.IsDead = false;
    }


    //Spawn monster
    public void Spawn(int gmhealth)
    {
        
       
        health = maxHealth;

       
        hp.text = health.ToString();

        //health.Initialize();

        /*set initial health to 10
        this.health.MaxVal = 10;
        this.health.CurrentVal = this.health.MaxVal;
        */

        transform.position = LevelManager.Instance.BluePortal.transform.position;
        StartCoroutine(Scale(new Vector3(0.1f,0.1f,0.1f), new Vector3(0.3f,0.3f,0.1f), false));
        //Get a path
        SetPath(LevelManager.Instance.Path);
      
    }

    public IEnumerator Scale (Vector3 from, Vector3 to, bool remove)
    {
        health = maxHealth;
        float progress = 0;
        while (progress<=1)
        {
            transform.localScale = Vector3.Lerp(from, to,progress);
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
            transform.position = Vector2.MoveTowards(transform.position, destination, speed*Time.deltaTime);
            if (transform.position == destination)
            {               
                if (path != null && path.Count > 0)
                {                
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
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
            if (path.Count>0)
            {     
                GridPosition = path.Peek().GridPosition;
                destination = path.Pop().WorldPosition;
            } 
        }
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("RedPortal") && gameObject.tag =="Monster")
        {
            StartCoroutine(Scale(new Vector3(0.3f, 0.3f, 0.1f), new Vector3(0.1f, 0.1f, 0.1f), true));
            //DAMAGE US
            GameManager.Instance.Lives -= 1;
            
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
}
