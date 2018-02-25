using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MProjectile : MonoBehaviour {


    private TowerHP target;
    // ref to parent (Tower script)
    private MonsterRange parent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        MoveToTarget();
       
    }

    //grab references from tower script (parent)
    public void Initialize(MonsterRange parent)
    {
        this.target = parent.Target;
        this.parent = parent;
    }
    private void MoveToTarget()
    {
        if(target !=null && target.MIsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);

            //direction vector
            Vector2 dir = target.transform.position - transform.position;
            //direction turned to degrees
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if(!target.MIsActive)
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Tower") && gameObject.CompareTag("MonsterProj"))
        {
            
            if (target.gameObject == other.gameObject)
            {
            
                TowerHP hitInfo = other.GetComponent<TowerHP>();
                target.TakeDamage(parent.Damage);

                //delete the projectile
                GameManager.Instance.Pool.ReleaseObject(gameObject);
            }

           
        }
        
    }
}
