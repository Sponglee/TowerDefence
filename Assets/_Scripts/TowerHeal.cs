using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHeal : MonoBehaviour {

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


    //monster ref
    [SerializeField]
    private TowerHP target;
    // public property for target
    public TowerHP Target
    {
        get { return target; }
    }

   

    // queue of monsters that will enter the range
    private List<TowerHP> monsters = new List<TowerHP>();

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
    // toggle Tower's range
    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        
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



        //make sure that we dont attack inactive monsters
        if (target != null)
        {
           
            if (canAttack)
            {
                
                Shoot();
                if (target.transform.position.x >= transform.position.x)
                {
                    transform.parent.GetComponent<SpriteRenderer>().flipX = true;
                    flip = true;
                }
                else
                {
                    transform.parent.GetComponent<SpriteRenderer>().flipX = false;
                    flip = false;
                }


                anim.SetBool("attack", true);
                canAttack = false;
            }
        }
        else if (target == null)
        {
           
            if (monsters.Count>0)
                target = monsters[Random.Range(0,monsters.Count)];
            anim.SetBool("attack", false);
        }
        
           

    }

    private void Shoot()
    {
        HProjectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<HProjectile>();

        if (flip)
        {
            projectile.transform.position = transform.position + new Vector3(0.7f, 0.7f, 0f);
        }
        else
        {
            projectile.transform.position = transform.position + new Vector3(-0.7f, 0.7f, 0f);
        }

        //passing Tower to Projectile script to get references 
        projectile.Initialize(this);
    }


    public void OnTriggerStay2D  (Collider2D other)
    {
        if (other.tag == "Tower" && other.GetComponent<TowerHP>().IsDamaged)
        {
            
            if (!monsters.Contains(other.GetComponent<TowerHP>()))
                monsters.Add(other.GetComponent<TowerHP>());
        }
        else if (other.CompareTag("Tower") && other.GetComponent<TowerHP>() == target)
        {
            if (!other.GetComponent<TowerHP>().IsDamaged || other.GetComponent<TowerHP>().IsDead)
            {
                target = null;
                
                monsters.Remove(other.GetComponent<TowerHP>());
            } 
        }
    }

    public void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag("Tower") && other.GetComponent<TowerHP>() == target)
        {
          
            target = null;
            monsters.Remove(other.GetComponent<TowerHP>());
        }
           
    }



}
