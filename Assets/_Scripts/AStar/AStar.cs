using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public static void GetPath(Point start, Point goal)
    {
        if (nodes == null)
        {
            CreateNodes();
        }

        //Declare open list
        HashSet<Node> openList = new HashSet<Node>();
        
        //Declare closed list
        HashSet<Node> closedList = new HashSet<Node>();

        //Declare a final path stack to backtrack the path
        Stack<Node> finalPath = new Stack<Node>();
        
        //Find start node 
        Node currentNode = nodes[start];

        //1 Add the start node to OpenList
        openList.Add(currentNode);

        //10. Search until no path available
        while (openList.Count>0)
        {

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);
                    // Inbounds checks "offgrid" cases
                    if (LevelManager.Instance.InBounds(neighbourPos) && LevelManager.Instance.Tiles[neighbourPos].WalkAble && neighbourPos != currentNode.GridPosition)
                    {
                        // calculate GScore of a node
                        int gCost = 0;


                        if (Math.Abs(x - y) == 1)
                            gCost = 10;
                        else
                        {
                        //Add this if Towers can be places on road tiles (see TileScript.cs)
                           /* if(!ConnectedDiagonally(currentNode, nodes[neighbourPos]))
                            {
                                continue;
                            }*/
                            gCost = 14;
                        }
                          


                        //3. Add neighbours to openlist
                        Node neighbour = nodes[neighbourPos];



                        if (openList.Contains(neighbour))//9.2 check for undiscovered neighbours because not in closed List 
                        {
                            if (currentNode.G + gCost < neighbour.G)
                            {
                                neighbour.CalcValues(currentNode, nodes[goal], gCost); //9.4 remaps parent on the node
                            }

                        }
                        else if (!closedList.Contains(neighbour)) //9.1 ignore closedList
                        {

                            openList.Add(neighbour); //9.2
                            neighbour.CalcValues(currentNode, nodes[goal], gCost); //9.3
                        }
                    }
                }
            }

            //5,8 Moves current node from openList to closedList
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //7. Sort the list and select lowest
            if (openList.Count > 0)
            {
                //sort List by F value and selects first one from it
                currentNode = openList.OrderBy(n => n.F).First();
            }

            //11 Exit if we found the goal
            if (currentNode == nodes[goal])
            {
                while(currentNode.GridPosition != start)
                {
                     finalPath.Push(currentNode);
                     currentNode = currentNode.Parent;
                }
                break;
            }
        }
     
        //****ONLY FOR DEBUGGING*****//
        GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList, finalPath);


       
    }
    //Checking if path cuts a corner (NOT needed for current version of game Rules

    //
    //   X  ^
    //   >  X

    private static bool ConnectedDiagonally(Node currentNode, Node neighbour)
    {
        Point direction = neighbour.GridPosition - currentNode.GridPosition;

        Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y + direction.Y);
        Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);

        if (LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].WalkAble)
        {
            return false;
        }
        if (LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[second].WalkAble)
        {
            return false;
        }
        return true;
    }
}
