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

        HashSet<Node> openList = new HashSet<Node>();

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
                    Node neighbour = nodes[neighbourPos];
                }
              
               
            }
        }
        //ONLY FOR DEBUGGING//
        GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList);
    }
    
}
