using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    [SerializeField]
    private BarScript bar;
    [SerializeField]
    private float maxVal;
    public float MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            this.maxVal = value;
        }
    }

    private float currentVal;
    public float CurrentVal
    {
        get
        {
            return currentVal;
        }

        set
        {
            currentVal = value;
            bar.Value = currentVal;
        }
    }

    public void Initialize()
    {
        this.MaxVal = maxVal;
        this.CurrentVal = currentVal;
    }
}

