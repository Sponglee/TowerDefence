using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour {

    [SerializeField]
    private int price;

    public int Price
    {
        get
        {
            return price;
        }
    }

    [SerializeField]
    private Text priceTxt;

    [SerializeField]
    private GameObject towerPrefab;
    //returning prefab to other scripts
    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }

    // enabling icon sprites for each button
    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }


    private void Start()
    {
        priceTxt.text = Price + "$";
    }
}
