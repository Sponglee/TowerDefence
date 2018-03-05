using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerHP : MonoBehaviour
{
    public bool IsDead;

    [SerializeField]
    public int cost = 0;
   
    public bool MIsActive { get; set; }

 

    [SerializeField]
    private Text hp;

   
    private int health;
    [SerializeField]
    private int maxHealth = 15;


    public void Start()
    {
      
        IsDead = false;
        MIsActive = true;
        health = maxHealth;

    }
    private void Update()
    {
        hp.text = health.ToString();

        
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
        MIsActive = true;
        if (remove)
           Destroy(gameObject);
    }


    public void TakeDamage(int damage)
    {
        if (MIsActive)
        {
            health -= damage;
            hp.text = health.ToString();
       
            if (health <= 0)
            {
                MIsActive = false;
                gameObject.transform.parent.GetComponent<TileScript>().WalkAble = true;
                gameObject.transform.parent.GetComponent<TileScript>().IsEmpty = true;
                IsDead = true;
                Destroy(gameObject);
            }
        }
    }
    //FOr tower selling purposes
    public void OnMouseOver()
    {
        Debug.Log("1");
        if (GameManager.Instance.sellSwitch == true && Input.GetMouseButtonDown(0))
        {
                GameManager.Instance.SellTower(this);
                gameObject.transform.parent.GetComponent<TileScript>().WalkAble = true;
                gameObject.transform.parent.GetComponent<TileScript>().IsEmpty = true;
        }
        else
        {

                GameManager.Instance.HandleEscape();
        }
        
        if (!EventSystem.current.IsPointerOverGameObject()
            && GameManager.Instance.ClickedBtn == null /* && Input.GetMouseButtonDown(0)*/)
            {
            
                GameManager.Instance.SelectTower(this.GetComponentInChildren<Tower>());
            }
            else
            {

                GameManager.Instance.DeselectTower();
            }

        }
    
}
