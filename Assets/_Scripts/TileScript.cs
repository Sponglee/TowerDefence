using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour {
    // Position of Tile in grid(x,y)
    public Point GridPosition { get; private set; }

    //Getting WorldCoordinates for Tile
    public Vector2 WorldPosition
    {
        get { return new Vector2(transform.position.x, transform.position.y); }
    }
    

    public bool IsEmpty { get; private set; }

 


    public bool Debugging{ get; set; }



    // colors for tile selection
    private Color32 fullColor = new Color32(255, 0, 0, 255);
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    //Sprite renderer variable
    private SpriteRenderer spriteRenderer;

    public bool WalkAble { get; set; }
    public bool Enemy { get; set; }

    public void Start()
    {
       spriteRenderer = GetComponent<SpriteRenderer>();
    }

  

    //Setting up a grid values for Tile
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    private void OnMouseOver()
    {
        //clicking the tile places towers after we pick it by pressing on button
        //also checks if tile is Not walkable by monsters to place it
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null )
            {
                
                if (IsEmpty && !Debugging && WalkAble )
                        spriteRenderer.color = emptyColor;
                if ((!IsEmpty && !Debugging) /*|| (WalkAble && !Debugging)*/)
                spriteRenderer.color = fullColor;
                else if (Input.GetMouseButtonDown(0))
                    PlaceTower();
            }   
    }

    private void OnMouseExit()
    {
        if (!Debugging)
         spriteRenderer.color = Color.white;
    }


    // place a tower and *buy* it
    private void PlaceTower()
    {
            // Instantiate tower on gridTile where mouse coursor is
            
            GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
            
            tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y+1;
            tower.transform.SetParent(transform);
            GameManager.Instance.BuyTower();
            WalkAble = false;
            Enemy = true;
            IsEmpty = false;
            spriteRenderer.color = Color.white;



    }

   
}
