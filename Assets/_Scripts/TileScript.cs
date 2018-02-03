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

    

    //Setting up a grid values for Tile
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    private void OnMouseOver()
    {
        //clicking the tile places towers after we pick it by pressing on button
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
            {
                PlaceTower();
            }
        }   
    }

    private void PlaceTower()
    {
            GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
            tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
            tower.transform.SetParent(transform);
            GameManager.Instance.BuyTower();
       
    }

}
