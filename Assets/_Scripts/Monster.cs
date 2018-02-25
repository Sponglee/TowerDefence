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

    private bool pathfind;
   

    [SerializeField]
    private Text hp;
  
    /*Stat class
    [SerializeField]
    private Stat health;
    */
    private int health;
    private int maxHealth = 15;


    public void Start()
    {
        pathfind = false;
        health = maxHealth;

    }
    private void Update()
    {
        hp.text = health.ToString();
        if (!TowerHP.IsDead)
        {
       
            Move();
        }
        else if (TowerHP.IsDead)
        {
           
                Debug.Log(LevelManager.Instance.Tmp.X + " " + LevelManager.Instance.Tmp.Y);
            //Generate new path from the point monster is now
            Point tmp = LevelManager.Instance.Tmp;
            LevelManager.Instance.GeneratePath(tmp);
            //set ne path
            SetPath(LevelManager.Instance.Path);

            //move to the first point
            transform.position = Vector2.MoveTowards(
                    transform.position, new Vector2(tmp.X, tmp.Y), speed * Time.deltaTime);
            
            //move to destination
            //Move();
            pathfind = true;
            TowerHP.IsDead = false;

        }
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
