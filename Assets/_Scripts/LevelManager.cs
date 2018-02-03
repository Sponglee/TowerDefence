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
    private Point blueSpawn, redSpawn;
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
        newTile.transform.position = new Vector2(worldStart.x + TileSize * x + TileSize/2, worldStart.y - TileSize* y - TileSize/2);
        newTile.Setup(new Point(x, y), newTile.transform.position = new Vector2(worldStart.x + TileSize * x + TileSize / 2, worldStart.y - TileSize * y - TileSize / 2), map);

        //Updating the dictionary
   
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
        blueSpawn = new Point(0, 1);
        redSpawn = new Point(12, 8);
        Instantiate(bluePortalPref, Tiles[blueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.Euler(Vector3.forward * -90));
        Instantiate(redPortalPref, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.Euler(Vector3.forward * -90));
    }
}
