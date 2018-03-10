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
    //private Point[] blueSpawn;
    private List<Point> blueSpawn;
    public List<Point> BlueSpawn
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

    int count;


    // Use this for initialization
    void Start ()
    {
        count = 0;
        InitializeLevel();
        SpawnPortals();
       
           
	}
    
    // Create a level
    private void InitializeLevel()
    {
        //instance of blueSpawn list of portals
        blueSpawn = new List<Point>();
        
        //Dictionary of tiles to form a grid
        Tiles = new Dictionary<Point, TileScript>();
        //string[] mapData = ReadLevelText();
        string[] mapData = ReadLevelTextWebGL();
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

        //Moving tile to it's place
        newTile.transform.position = new Vector2(worldStart.x + TileSize * x + TileSize/2, worldStart.y - TileSize * y - TileSize/2 );
       
        newTile.Setup(new Point(x, y), newTile.transform.position = new Vector2(worldStart.x + TileSize * x + TileSize / 2, worldStart.y - TileSize * y - TileSize / 2), map);

        //Checks if tile is a road(Walkable) or not and sets it in 
        //Walkable property for TileScript instance
        if (tileIndex == 4)
        {
            newTile.WalkAble = true;
            //randomize tiles by turning 90*
            i++;
            newTile.transform.Rotate(Vector3.forward, (180 * i + 90) % 360);
        }
        // Spawn points (index 2 )
        else if (tileIndex == 2)
        {
            //add new blue portal if there's tile for it
            blueSpawn.Add(newTile.GridPosition);
            count++;
            newTile.WalkAble = true;
        }
        // Spawn points for exit (index 3 )
        else if (tileIndex == 3)
        {
            newTile.WalkAble = true;
        }
        else
        {
            newTile.WalkAble = false;  
        }
       
   
    }

    //Get Level.txt file
    //private string[] ReadLevelText()
    //{
    //    int levelIndex = UnityEngine.Random.Range(0, 2);
    //    string type = string.Empty;
    //    switch (levelIndex)
    //    {
    //        case 0:
    //            type = "Level";
    //            break;
    //        //case 1:
    //        //    type = "Level2";
    //        //    break;
    //        //case 2:
    //        //    type = "Level3";
    //        //    break;
    //    }
    //    TextAsset bindData = Resources.Load(type) as TextAsset;
    //    string data = bindData.text.Replace(Environment.NewLine, string.Empty);
    //    return data.Split('-');
    //}

    //Get Level.txt file
    private string[] ReadLevelTextWebGL()
    {
        //int levelIndex = UnityEngine.Random.Range(0, 2);
        int levelIndex = 0;
        string level="";
        switch (levelIndex)
        {
            //case 0:
            //    level = "111111111111111-124444444444111-111444111114111-111444411114111-111414441114111-111411444114111-111411144414111-111411114444111-111444444444441-111111111111111";
            //    break;
            case 0:
                level = "" +
                    "111111111111111-" +
                    "124444411111111-" +
                    "111444441111111-" +
                    "111144444111111-" +
                    "111114444411111-" +
                    "111111444441111-" +
                    "111111144444111-" +
                    "111111114444411-" +
                    "111111111444431-" +
                    "111111111111111";
                break;

        }
        
        return level.Split('-');
    }

    // Add portals to the grid
    private void SpawnPortals()
    {

        Camera.main.transform.Translate(Vector3.up*0.65f);
      
        //Red Spawn coordinates yet
        redSpawn = new Point(13, 8);//13.8
        Instantiate(redPortalPref, Tiles[RedSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.Euler(Vector3.forward * -90));




        //for (int i = 0; i < blueSpawnFile.Count; i++)
        //{
        //    Point index = blueSpawnFile[i];



        //   GameObject tmp = Instantiate(bluePortalPref, Tiles[index].GetComponent<TileScript>().WorldPosition, Quaternion.Euler(Vector3.forward * -90));

        //}

        //v3 of each tile with blue portals
        Vector3 v3tmp = Tiles[blueSpawn[0]].GetComponent<TileScript>().WorldPosition;
        Debug.Log(v3tmp.x + "       " + v3tmp.y + "         " + v3tmp.z + "    :     ");
        GameObject tmp = Instantiate(bluePortalPref, v3tmp, Quaternion.Euler(Vector3.forward * -90));
        BluePortal = tmp.GetComponent<Portal>();
        BluePortal.name = "BluePortal";
    }

    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X <= mapSize.X & position.Y <= mapSize.Y;
    }

    public Stack<Node> GeneratePath(Point spawn)
    {


        if ( AStar.Obstacles == null)
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
