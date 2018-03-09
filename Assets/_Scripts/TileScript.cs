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
    

    public bool IsEmpty { get; set; }


    //keeping track of selected towers
    private GameObject myTower;


    public bool Debugging{ get; set; }



    // colors for tile selection
    private Color32 fullColor = new Color32(255, 0, 0, 255);
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    //Sprite renderer variable
    private SpriteRenderer spriteRenderer;

    public bool WalkAble { get; set; }
    public bool Enemy { get; set; }
    float time;

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
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
            if (IsEmpty && !Debugging && WalkAble)

            {
                spriteRenderer.color = emptyColor;
                if (Input.GetMouseButtonDown(0))
                    PlaceTower();
            }
            else if ((!IsEmpty && !Debugging) || (!WalkAble && !Debugging) ||
               /* (Mathf.Abs(GridPosition.X - LevelManager.Instance.BlueSpawn.X) < 1
                    && Mathf.Abs(GridPosition.Y - LevelManager.Instance.BlueSpawn.Y) == 0)
                        ||*/ GridPosition == LevelManager.Instance.RedSpawn)

            {
                spriteRenderer.color = fullColor;
            }

        }
        else if (!EventSystem.current.IsPointerOverGameObject()
            && GameManager.Instance.ClickedBtn == null /* && Input.GetMouseButtonDown(0)*/)
        {
            if (myTower != null)
            {
                GameManager.Instance.SelectTower(myTower);
            }
            else
            {

                GameManager.Instance.DeselectTower();
            }

        }
        
       
    }

    private void OnMouseExit()
    {
        if (!Debugging)
         spriteRenderer.color = Color.white;
        GameManager.Instance.DeselectTower();
    }


    // place a tower and *buy* it
    private void PlaceTower()
    {
            // Instantiate tower on gridTile where mouse coursor is
            
            GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
            
            tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y+1;
            tower.transform.SetParent(transform);
            // reference to tower for range
            myTower = tower;
          

            GameManager.Instance.BuyTower();
            WalkAble = false;
            Enemy = true;
            IsEmpty = false;
            spriteRenderer.color = Color.white;
            
            foreach (Monster monster in GameManager.Instance.ActiveMonsters)
            {
            ////Get path from curr pos to obstacle
            // Stack<Node> path = AStar.GetPath(monster.CurrentTilePos, GridPosition);
            // Node last = path.Pop();
            ////Get path from curr pos to goal
            //Stack<Node> gpath = AStar.GetPath(monster.CurrentTilePos, LevelManager.Instance.RedSpawn);
            //Node glast = path.Pop();
          
            //if(last.F < glast.F)
            //{
            //    Debug.Log("REEE");
            //    AStar.NewGoal = true;
                monster.MRePath = true;
                
           
                    
            }
            


    }
    //Sends Each monster a grid position whenever it enters a tile
    public void OnTriggerEnter2D(Collider2D other)
    {
        time = 0;
        if (other.CompareTag("Monster"))
        {
            
            other.GetComponent<Monster>().CurrentTilePos = gameObject.GetComponent<TileScript>().GridPosition;
            other.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
            if (this.WalkAble == false)
            {
                other.GetComponent<Monster>().MRePath = true;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Monster"))
        {

            if (other.gameObject.GetComponentInChildren<MonsterRange>().Target == null)
            {

                time += Time.deltaTime;
                if (time >= 3)
                {
                  
                    other.GetComponentInChildren<MonsterRange>().Target = null;
                    LevelManager.Instance.Path = null;
                    other.GetComponent<Monster>().CurrentTilePos = gameObject.GetComponent<TileScript>().GridPosition;
                    other.gameObject.GetComponent<Monster>().MRePath = true;
                   
                    time = 0;
                }
            }

        }
    }




}
