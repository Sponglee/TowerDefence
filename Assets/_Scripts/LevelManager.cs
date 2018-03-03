using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : Singleton<LevelManager> {

    int i = 0;
    [SerializeField]
    private GameObject[] tiles;
    [SerializeField]
    private GameObject bluePortalPref, redPortalPref;
    [SerializeField]
    private Transform map;

    private Point mapSize;

    private Point redSpawn;
    public Point RedSpawn
    {
        get
        {
            return redSpawn;
        }
    }
    public Portal BluePortal{ get; set; }

    public Dictionary<Point, TileScript> Tiles { get; set; }

    //Path for AStar
    private Stack<Node> path;
    public Stack<Node> Path
    {
        get
        {
            //if (path == null)
            //{
            //    //Makes a Path between cTMP position and Redspawn
            //    GeneratePath(BlueSpawn);
            //}
            return new Stack<Node>(new Stack<Node>(path));
        }
        set { path = value; }
    }
    private Point blueSpawn;
    public Point BlueSpawn
    {
        get
        {
            return blueSpawn;
        }
    }
    //Temporary position
    Point tmp;
    public Point Tmp
    {
        get
        {
            return tmp;
        }

        set
        {
            tmp = value;
        }
    }


    //Get size of a Tile for further calculations
    public float TileSize
    {
        get { return tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }




    // Use this for initialization
    void Start ()
    {
     
        InitializeLevel();
        SpawnPortals();
	}
    
    // Create a level
    private void InitializeLevel()
    {
  
        //Dictionary of tiles to form a grid
        Tiles = new Dictionary<Point, TileScript>();
        string[] mapData = ReadLevelText();

        //Map size from Level.txt
        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;
        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        //Rows y
        for (int y = 0; y<mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            //Columns x
            for (int x= 0; x<mapX; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }
    }

    //Spawn each Tile from Level.txt
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        //Get a single tile prefab code
        int tileIndex = int.Parse(tileType);
        
        //Instantiating TileScript to each component
        TileScript newTile = Instantiate(tiles[tileIndex]).GetComponent<TileScript>();

        //Checks if tile is a road(Walkable) or not and sets it in 
        //Walkable property for TileScript instance
        if (tileIndex != 4)
        {
            newTile.WalkAble = false;
        }
        else
        {
            
            newTile.WalkAble = true;
            i++;
            newTile.transform.Rotate(Vector3.forward, (180 * i+90) % 360);
        }
           
        //Moving tile to it's place
        newTile.transform.position = new Vector2(worldStart.x + TileSize * x + TileSize/2, worldStart.y - TileSize * y - TileSize/2 );
       
        newTile.Setup(new Point(x, y), newTile.transform.position = new Vector2(worldStart.x + TileSize * x + TileSize / 2, worldStart.y - TileSize * y - TileSize / 2), map);

       
   
    }

    //Get Level.txt file
    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }

    // Add portals to the grid
    private void SpawnPortals()
    {
        Camera.main.transform.Translate(Vector3.up*0.65f);
        blueSpawn = new Point(1, 1);
        redSpawn = new Point(13, 8);//13.8
        GameObject tmp = Instantiate(bluePortalPref, Tiles[BlueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.Euler(Vector3.forward * -90));
        BluePortal = tmp.GetComponent<Portal>();
        BluePortal.name = "BluePortal";

        Instantiate(redPortalPref, Tiles[RedSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.Euler(Vector3.forward * -90));
    }

    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X <= mapSize.X & position.Y <= mapSize.Y;
    }

    public Stack<Node> GeneratePath(Point spawn)
    {
        if (AStar.Obstacles ==null )
            // case when there's clear path to redSpawn
            AStar.NewGoal = false;
          
        

        path = AStar.GetPath(spawn, RedSpawn);
        

        if (AStar.NewGoal)
        {

            //If path to redSpawn is unreachable turn on "NEW GoAL" mode to get to random obstacle
            //closest F score 
            if(AStar.Obstacles.Count != 0)
            {
                Node closestEnemy = AStar.Obstacles.OrderBy(n => n.F).First();

                Tmp = closestEnemy.GridPosition;
            }
               
           
            //Check if 
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point neighbourPos = new Point(Tmp.X - x, Tmp.Y - y);
                    // Inbounds checks "offgrid" cases and Walkables
                    if (LevelManager.Instance.InBounds(neighbourPos) && LevelManager.Instance.Tiles[neighbourPos].WalkAble
                        && neighbourPos != Tmp)
                    {
                        path = AStar.GetPath(spawn, Tmp);
                        return path;
                    }
                }
            }
           
        }
        return path;
    }
}
