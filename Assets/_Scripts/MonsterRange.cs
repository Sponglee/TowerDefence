using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRange : MonoBehaviour {

    [SerializeField]
    private int damage = 1;
    public int Damage
    {
        get
        {
            return damage;
        }
    }

    //serializable projectile type
    [SerializeField]
    private string projectileType;
    //speed for projectiles
    [SerializeField]
    private float projectileSpeed;
    //public property for projectileSpeed
    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }
    [SerializeField]
    private float towerRange;

    //Left or right bool
    bool flip=false;


    //tower ref
    private TowerHP target;
    // public property for target
    public TowerHP Target
    {
        get { return target; }
    }


    // queue of monsters that will enter the range
    private Queue<TowerHP> monsters = new Queue<TowerHP>();

    //Range sprite
    private SpriteRenderer mySpriteRenderer;

    //attack toggle
    private bool canAttack = true;

    [SerializeField]
    private float AttackCooldown;
    private float AttackTimer;

    // Use this for initialization
    void Start() {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
     
    }

    // Update is called once per frame
    void Update() {
        
        Attack();

    }


    public void Attack()
    {

        //animator for a tower
        Animator anim = transform.parent.GetComponent<Animator>();
        //left or right bool

       
        if (!canAttack)
        {
            AttackTimer += Time.deltaTime;
            if (AttackTimer >= AttackCooldown)
            {
                canAttack = true;
                AttackTimer = 0;
            }
        }
        //target left the area
        if (target == null && monsters.Count>0)
        {
            target = monsters.Dequeue();
        }
        //make sure that we dont attack inactive monsters
        if (target != null)
        {
            if (canAttack)
            {
                
                Shoot();
                Debug.Log("Time to shoot " + target);

               
                anim.SetBool("attack", true);
                canAttack = false;
            }
        }
        else if (target == null)
        {
            anim.SetBool("attack", false);
        }
           

    }

    private void Shoot()
    {
        MProjectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<MProjectile>();
       
        projectile.transform.position = transform.position + new Vector3(0.7f, 0.7f, 0f);
     

        //passing Tower to Projectile script to get references 
        projectile.Initialize(this);
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Tower")
        {
            Debug.Log("WE GOTTA BE SHOOTING");
            monsters.Enqueue(other.GetComponent<TowerHP>());

        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Tower")
        {

            target = null;
        }
    }

    public void TowerDamage()
    {

    }
}
