using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Struct for a grid representation (coordinates)
public struct Point
{
    public int X { get;set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
	
}
