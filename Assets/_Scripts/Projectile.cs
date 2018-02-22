using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


    private Monster target;
    // ref to parent (Tower script)
    private Tower parent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        MoveToTarget();
       
    }

    //grab references from tower script (parent)
    public void Initialize(Tower parent)
    {
        this.target = parent.Target;
        this.parent = parent;
    }
    private void MoveToTarget()
    {
        if(target !=null && target.IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);

            //direction vector
            Vector2 dir = target.transform.position - transform.position;
            //direction turned to degrees
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if(!target.IsActive)
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            if (target.gameObject == other.gameObject)
            {
                Monster hitInfo = other.GetComponent<Monster>();
                target.TakeDamage(parent.Damage);

                //delete the projectile
                GameManager.Instance.Pool.ReleaseObject(gameObject);
            }

           
        }
    }
}
