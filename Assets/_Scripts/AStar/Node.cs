using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public Point GridPosition { get;  set; }

    public TileScript TileRef{ get; private  set; }

    public Vector2 WorldPosition { get; set; }

    //property for parent node 
    public Node Parent { get; private set; }

    //property for GScore
    public int G { get; set; }
    //property for Heuristic Score
    public int H { get; set; }
    //property for AStar function Score
    public int F { get; set; }

    public Node(TileScript tileRef)
    {
        TileRef = tileRef;
        GridPosition = tileRef.GridPosition;
        WorldPosition = tileRef.WorldPosition;
    }


    //calculate astar function
    public void CalcValues(Node parent, Node goal, int gCost)
    {
        //set parent to backtrack
        this.Parent = parent;
        // Neighbour score
        this.G = parent.G+gCost;
        // H value - amount of steps to the goal x10
        this.H = (Math.Abs(GridPosition.X - goal.GridPosition.X)  + Math.Abs(goal.GridPosition.Y - GridPosition.Y)) * 10;
        // AStar function value;
        this.F = G + H;
    }

    
}