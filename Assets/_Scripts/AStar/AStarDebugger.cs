using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour {
    [SerializeField]
    private TileScript goal,start;
    [SerializeField]
    private Sprite blankTile;


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
                if (tmp != null)
                {
                    if (start == null)
                    {
                        start = tmp;
                        start.Debugging = true;
                        start.SpriteRenderer.color = new Color32(0, 132, 255, 255);
                        start.SpriteRenderer.sprite = blankTile;
                    }


                    else if (goal == null)
                    {
                        goal = tmp;
                        goal.Debugging = true;
                        goal.SpriteRenderer.color = new Color32(255, 0, 0, 255);
                        goal.SpriteRenderer.sprite = blankTile;
                    }

                }
            }

        }
    }

    public  void DebugPath(HashSet<Node>openList)
    {
        foreach(Node node in openList)
        {
            node.TileRef.SpriteRenderer.color = Color.cyan;
            node.TileRef.SpriteRenderer.sprite = blankTile;
        }
    }
}
