using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int rows = 10;
    [SerializeField] private int cols = 10;

    [SerializeField] private TileHolder tilePrefab;
    [SerializeField] private Wall wallPrefab;
    [SerializeField] private Floor floorPrefab;

    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerRB;

    private TileHolder[,] tileGrid;
    private Floor[,] floorGrid;

    public void GenerateGrid()
    {
        tileGrid = new TileHolder[rows, cols];
        floorGrid = new Floor[rows, cols];

        for (int x = -1; x <= rows; x++)
        {
            for (int z = -1; z <= cols; z++)
            {
                if (x == -1 || x == rows || z == -1 || z == cols)
                {
                    var spawnedWall = Instantiate(wallPrefab, new Vector3(x, 0.4f, z), Quaternion.identity);
                    spawnedWall.name = $"Wall {x} {z}"; 
                    
                    var spawnedFloorPart = Instantiate(floorPrefab, new Vector3(x, -0.5f, z), Quaternion.identity);
                    spawnedFloorPart.name = $"Floor part {x} {z}";
                }
                else
                {
                    var spawnedTile = Instantiate(tilePrefab, new Vector3(x, -0.47f, z), Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {z}";
                    spawnedTile.SetCoords(x, z);
                    tileGrid[x, z] = spawnedTile;

                    var spawnedFloorPart = Instantiate(floorPrefab, new Vector3(x, -0.5f, z), Quaternion.identity);
                    spawnedFloorPart.name = $"Floor part {x} {z}";
                    floorGrid[x, z] = spawnedFloorPart;
                }
            }
        }

        playerRB.SetActive(false);
        player.position = new Vector3((float)rows * 0.5f - 0.5f, 2f, (float)cols * 0.5f - 0.5f);
        playerRB.SetActive(true);
    }

    public void LoadCustomShape(string filePath)
    {
        string[] lines = System.IO.File.ReadAllLines(filePath);
        int rows = lines.Length;
        int cols = lines[0].Length;

        tileGrid = new TileHolder[rows, cols];
        floorGrid = new Floor[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (lines[row][col] == '0')
                {
                    // Generate tile and floor part
                    var spawnedTile = Instantiate(tilePrefab, new Vector3(row, -0.49f, col), Quaternion.identity);
                    spawnedTile.name = $"Tile {row} {col}";
                    spawnedTile.SetCoords(row, col);
                    tileGrid[row, col] = spawnedTile;

                    var spawnedFloorPart = Instantiate(floorPrefab, new Vector3(row, -0.52f, col), Quaternion.identity);
                    spawnedFloorPart.name = $"Floor part {row} {col}";
                    floorGrid[row, col] = spawnedFloorPart;
                }
            }
        }
    }

    public TileHolder[,] GetTileGrid()
    {
        return tileGrid;
    }

    public Floor[,] GetFloorGrid()
    {
        return floorGrid;
    }
}
