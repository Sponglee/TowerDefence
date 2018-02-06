using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTile : MonoBehaviour
{
    [SerializeField]
    private Text f, g, h;

    
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
            return g;
        }

        set
        {
            g.gameObject.SetActive(true);
            g = value;
        }
    }

    public Text H
    {
        get
        {
            return h;
        }

        set
        {
            h = value;
        }
    }
}
