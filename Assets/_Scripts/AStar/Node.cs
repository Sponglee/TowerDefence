using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public Point GridPosition { get; private set; }

    public TileScript TileRef{ get; private  set; }

    //property for parent node 
    public Node Parent { get; private set; }

    //property for GScore
    public int G { get; set; }

    public Node(TileScript tileRef)
    {
        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition;
    }


    //calculate astar function
    public void CalcValues(Node parent, int gCost)
    {
        //set parent to backtrack
        this.Parent = parent;
        this.G = parent.G+gCost;
    }

    
}