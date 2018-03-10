using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour {

    [SerializeField]
    private int manaAmount=2;
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
            if (GameManager.Instance.GmBossCoin)
            {
                GameManager.Instance.EatMana(manaAmount*5);
                FltText.GetComponentInChildren<Text>().text 
                    = string.Format("+{0}$",manaAmount*5);
            }
            else
            {
                GameManager.Instance.EatMana(manaAmount);
                FltText.GetComponentInChildren<Text>().text
                   = string.Format("+{0}$", manaAmount);
            }
               
            this.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            GameObject tmp = Instantiate(FltText, transform.position, Quaternion.identity);
            tmp.transform.position = this.transform.position;
            SoundManager.PlaySound("coin");
            Destroy(gameObject);
            }
    }


}
