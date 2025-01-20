using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

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

        for (int row = -1; row <= rows; row++)
        {
            for (int col = -1; col <= cols; col++)
            {
                if (row == -1 || row == rows || col == -1 || col == cols)
                {
                    GenerateBorder(row, col, false, false);
                }
                else
                {
                    GenerateTileFloor(row, col);
                }
            }
        }
        TeleportPlayer(false);
    }

    public void LoadCustomShapeGrid(string filePath)
    {
        filePath = Path.Combine(Application.streamingAssetsPath.Replace('/', '\\'), filePath);
        string[] lines = File.ReadAllLines(filePath);
        rows = lines.Length;
        cols = lines[0].Length;

        tileGrid = new TileHolder[rows, cols];
        floorGrid = new Floor[rows, cols];

        for (int row = -1; row <= rows; row++)
        {
            for (int col = -1; col <= cols; col++)
            {
                if (row == -1 || row == rows || col == -1 || col == cols)
                {
                    if (CheckAdjacent(row, col, lines)) GenerateBorder(row, col, false, true);
                }
                else if (lines[row][col] == '0')
                {
                    if (CheckAdjacent(row, col, lines))
                    {
                        GenerateBorder(row, col, true, true);
                    }
                    else
                    {
                        tileGrid[row, col] = null;
                        floorGrid[row, col] = null;
                    }
                }
                else
                {
                    GenerateTileFloor(row, col);
                }
            }
        }
        TeleportPlayer(true);
    }

    private void GenerateTileFloor(int row, int col)
    {
        var spawnedTile = Instantiate(tilePrefab, new Vector3(row, -0.47f, col), Quaternion.identity);
        spawnedTile.name = $"Tile {row} {col}";
        spawnedTile.SetCoords(row, col);
        tileGrid[row, col] = spawnedTile;

        var spawnedFloorPart = Instantiate(floorPrefab, new Vector3(row, -0.5f, col), Quaternion.identity);
        spawnedFloorPart.name = $"Floor part {row} {col}";
        spawnedFloorPart.UpdateVisuals();
        floorGrid[row, col] = spawnedFloorPart;
    }

    private void GenerateBorder(int row, int col, bool inBorder, bool shapedLevel)
    {
        var spawnedWall = Instantiate(wallPrefab, new Vector3(row, 0.4f, col), Quaternion.identity);
        spawnedWall.name = $"Wall {row} {col}";

        var spawnedFloorPart = Instantiate(floorPrefab, new Vector3(row, 0f, col), Quaternion.identity);
        spawnedFloorPart.SetBorder();
        if (shapedLevel)
        {
            spawnedFloorPart.SetShapedLevel();
        }
        spawnedFloorPart.UpdateVisuals();
        spawnedFloorPart.name = $"Floor border part {row} {col}";

        if (inBorder)
        {
            floorGrid[row, col] = spawnedFloorPart;
        }
    }

    private bool CheckAdjacent(int row, int col, string[] lines)
    {
        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = col - 1; j <= col + 1; j++)
            {
                if (MineManager.IsInBounds(i, j, rows, cols) && lines[i][j] == '1')
                {
                    return true; // Return true if there is a tile adjacent to this cell
                }
            }
        }

        return false;
    }

    private void TeleportPlayer(bool shapedLevel)
    {
        if (shapedLevel)
        {
            int x = rows / 2;
            int z = cols / 2;
            if (tileGrid[x, z] == null)
            {
                int xMin = rows / 2 - 1;
                int xMax = rows / 2 + 1;
                int zMin = cols / 2 - 1;
                int zMax = cols / 2 + 1;
                bool loop = true;
                while (loop)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {
                        if (tileGrid[i, zMin] != null)
                        {
                            x = i;
                            z = zMin;
                            loop = false;
                        }
                        else if (tileGrid[i, zMax] != null)
                        {
                            x = i;
                            z = zMax;
                            loop = false;
                        }
                    }
                    for (int i = zMin + 1; i < zMax; i++)
                    {
                        if (tileGrid[xMin, i] != null)
                        {
                            x = xMin;
                            z = i;
                            loop = false;
                        }
                        else if (tileGrid[xMax, i] != null)
                        {
                            x = xMax;
                            z = i;
                            loop = false;
                        }
                    }

                    bool changedAnyValue = false;
                    if (xMin - 1 >= 0)
                    {
                        xMin--;
                        changedAnyValue = true;
                    }
                    if (xMax + 1 < rows)
                    {
                        xMax++;
                        changedAnyValue = true;
                    }
                    if (zMin - 1 >= 0)
                    {
                        zMin--;
                        changedAnyValue = true;
                    }
                    if (zMax + 1 < cols)
                    {
                        zMax++;
                        changedAnyValue = true;
                    }
                    
                    if (changedAnyValue)
                    {
                        loop = false;
                    }
                }
            }

            playerRB.SetActive(false);
            player.position = new Vector3((float)x, 2f, (float)z);
            playerRB.SetActive(true);
        }
        else
        {
            playerRB.SetActive(false);
            player.position = new Vector3((float)rows * 0.5f - 0.5f, 2f, (float)cols * 0.5f - 0.5f);
            playerRB.SetActive(true);
        }
    }

    public void SetSize(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
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
