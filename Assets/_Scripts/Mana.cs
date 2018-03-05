using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour {

    [SerializeField]
    private int manaAmount;
    [SerializeField]
    private GameObject FltText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
      


    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            {
                GameManager.Instance.EatMana(manaAmount);
                this.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                GameObject tmp = Instantiate(FltText, transform.position, Quaternion.identity);
                tmp.transform.position = this.transform.position;
                Destroy(gameObject);
            }
    }


}
