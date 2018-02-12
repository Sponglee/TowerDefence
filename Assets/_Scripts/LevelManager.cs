using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager> {
    [SerializeField]
    private GameObject[] tiles;
    [SerializeField]
    private GameObject bluePortalPref, redPortalPref;
    [SerializeField]
    private Transform map;

    private Point mapSize;
   
    public Point blueSpawn, redSpawn;

    public Portal BluePortal{ get; set; }


    //Path for AStar
  
    private Stack<Node> path;
    public Stack<Node> Path
    {
        get
        {
            if (path == null)
            {
                Debug.Log("HEY! PATH IS NULL");
                GeneratePath();
            }
            return new Stack<Node>(new Stack<Node>(path));
        }
    }
    public Dictionary<Point, TileScript> Tiles { get; set; }

    //Get size of a Tile for further calculations
    public float TileSize
    {
        get { return tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    // Use this for initialization
    void Start () {
      
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
            newTile.WalkAble = true;
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
        redSpawn = new Point(13, 8);
        GameObject tmp = Instantiate(bluePortalPref, Tiles[blueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.Euler(Vector3.forward * -90));
        BluePortal = tmp.GetComponent<Portal>();
        BluePortal.name = "BluePortal";

        Instantiate(redPortalPref, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.Euler(Vector3.forward * -90));
    }

    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X <= mapSize.X & position.Y <= mapSize.Y;
    }

    public void GeneratePath()
    {
        // case when there's clear path to redSpawn
        path = AStar.GetPath(blueSpawn, redSpawn);
        if (path.Count == 0)
        {
            //If path to redSpawn is unreachable turn on "NEW GoAL" mode to get to random obstacle
            Debug.Log("NEW PATH");
            AStar.NewGoal = true;
            int rng = UnityEngine.Random.Range(0, AStar.Obstacles.Count-1);
            Point tmp = AStar.Obstacles[rng].GridPosition;
            Debug.Log("NEW GOAL: " + tmp.X + " " + tmp.Y);
            path = AStar.GetPath(blueSpawn, tmp);
        }
    }
}
