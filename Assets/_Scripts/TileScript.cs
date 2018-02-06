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

 
    //property to be used in other scripts
    public SpriteRenderer SpriteRenderer { get; set; }

    public bool Debugging{ get; set; }



    // colors for tile selection
    private Color32 fullColor = new Color32(255, 0, 0, 255);
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    
    public void Start()
    {
       SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool WalkAble { get; set; }

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
                
                if (IsEmpty && !Debugging && !WalkAble)
                        SpriteRenderer.color = emptyColor;
                if ((!IsEmpty && !Debugging) || (WalkAble && !Debugging))
                   SpriteRenderer.color = fullColor;
                else if (Input.GetMouseButtonDown(0))
                    PlaceTower();
            }   
    }

    private void OnMouseExit()
    {
        if (!Debugging)
          SpriteRenderer.color = Color.white;
    }


    // place a tower and *buy* it
    private void PlaceTower()
    {
            // Instantiate tower on gridTile where mouse coursor is
            
            GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
            tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
            tower.transform.SetParent(transform);
            GameManager.Instance.BuyTower();
            WalkAble = false;
            IsEmpty = false;
            SpriteRenderer.color = Color.white;



    }

   
}
