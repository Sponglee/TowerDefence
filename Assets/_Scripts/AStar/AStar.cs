using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{

    public static Dictionary<Point, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

    //Create a list of nodes for AStar
    public static void GetPath(Point start)
    {
        if (nodes == null)
        {
            CreateNodes();
        }

        //Declare open list
        HashSet<Node> openList = new HashSet<Node>();
        
        //Declare closed list
        HashSet<Node> closedList = new HashSet<Node>();
        
        //Find start node 
        Node currentNode = nodes[start];

        //Add the start node to OpenList
        openList.Add(currentNode);


     
        for (int x = -1; x <=1; x++)
        {
            for (int y = -1; y <=1; y++)
            {
                Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);
                // Inbounds checks "offgrid" cases
                if ( LevelManager.Instance.InBounds(neighbourPos) &&  LevelManager.Instance.Tiles[neighbourPos].WalkAble && neighbourPos != currentNode.GridPosition)
                {
                    // calculate GScore of a node
                    int gCost = 0;
                    if (Math.Abs(x - y) == 1)
                        gCost = 10;
                    else
                        gCost = 14;
                    //Add neighbours to openlist
                    Node neighbour = nodes[neighbourPos];
                    if(!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                    neighbour.CalcValues(currentNode, gCost);
                }
            }
        }

        openList.Remove(currentNode);
        closedList.Add(currentNode);

        //****ONLY FOR DEBUGGING*****//
        GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList);
    }
    
}
