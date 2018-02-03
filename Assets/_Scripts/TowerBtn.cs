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

}
