using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtn : MonoBehaviour {

  

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
}
