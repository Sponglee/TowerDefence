using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour {
    [SerializeField]
    private TileScript goal,start;
    [SerializeField]
    private Sprite blankTile;
    //arrow for parent
    [SerializeField]
    private GameObject arrowPrefab;
    
    //Debug tile for debug layer
    [SerializeField]
    private GameObject debugTilePrefab;

    private void Update()
    {
        ClickTile();

        if(Input.GetKeyDown(KeyCode.Space))
            {
                AStar.GetPath(start.GridPosition);
            }
    }
    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(2))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 

            if(hit.collider != null)
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>();
                Debug.Log(tmp.WorldPosition);
                if (tmp != null)
                {
                    if (start == null)
                    {
                        start = tmp;
                        CreateDebugTile(start.WorldPosition, new Color32(255, 132, 0, 255));
                    }


                    else if (goal == null)
                    {
                        goal = tmp;
                        CreateDebugTile(goal.WorldPosition, new Color32(255, 0, 0, 255));
                      
                    }

                }
            }

        }
    }


    private void CreateDebugTile(Vector3 worldPos, Color32 color)
    {
        GameObject debugTile = Instantiate(debugTilePrefab, worldPos, Quaternion.identity);
        debugTile.GetComponent<SpriteRenderer>().color = color;

    }

    //Debug path for closed list
    public void DebugPath(HashSet<Node>openList, HashSet<Node> closedList)
    {
        //Open list loop
        foreach (Node node in openList)
        {
            if (node.TileRef != start)
            {
              CreateDebugTile(node.TileRef.WorldPosition, new Color32(0, 132,255 , 255));
            }
            //show arrow to parent node
            PointToParent(node, node.TileRef.WorldPosition);
        }
        // Closed list loop
        foreach (Node node in closedList)
        {
            if (node.TileRef != start && node.TileRef != goal)
            {
               CreateDebugTile(node.TileRef.WorldPosition, new Color32(0, 0, 255, 255));
            }
        }
    }

   
    private void PointToParent(Node node, Vector2 position)
    {
        if (node.Parent != null)
        { 
            GameObject arrow = GameObject.Instantiate(arrowPrefab, position, Quaternion.identity);
            arrow.GetComponent<SpriteRenderer>().sortingOrder = 3;
            //Point to the Right
            if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            //Top right
            else if ((node.GridPosition.X<node.Parent.GridPosition.X)&&(node.GridPosition.Y>node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, -135+180);
            }
            //Up
            else if ((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, -90+180);
            }
            //Top left
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, -45+180);
            }
            //Down
            else if ((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 90+180);
            }
           
            //Bottom right
            else if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 135+180);
            }
            // Left
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            //Bottom left
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 45+180);
            }

        }
    }

   
}
