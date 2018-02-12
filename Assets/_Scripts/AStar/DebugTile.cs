using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTile : MonoBehaviour
{
    [SerializeField]
    private Text f, g, h, grid;

    
    public Text F
    {
        get
        {
            f.gameObject.SetActive(true);
            return f;
        }

        set
        {
            
            f = value;
        }
    }

    public Text G
    {
        get
        {
            g.gameObject.SetActive(true);
            return g;
        }

        set
        {
          
            g = value;
        }
    }

    public Text H
    {
        get
        {
            h.gameObject.SetActive(true);
            return h;
        }

        set
        {
            h = value;
        }
    }

    public Text Grid
    {
        get
        {
            grid.gameObject.SetActive(true);
            return grid;
        }

        set
        {

            grid = value;
        }
    }
}
