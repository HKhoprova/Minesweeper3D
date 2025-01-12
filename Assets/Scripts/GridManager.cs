using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int rows = 10;
    [SerializeField] private int cols = 10;

    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Wall wallPrefab;
    [SerializeField] private Floor floorPrefab;

    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerRB;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        var spawnedWall1 = Instantiate(wallPrefab, new Vector3((rows + 1f) * 0.5f, 0.4f, cols + 1f), Quaternion.identity);
        spawnedWall1.transform.localScale = new Vector3(rows, 3f, 1f);
        spawnedWall1.name = $"Wall North";

        var spawnedWall2 = Instantiate(wallPrefab, new Vector3(0, 0.4f, (cols + 1f) * 0.5f), Quaternion.identity);
        spawnedWall2.transform.localScale = new Vector3(1f, 3f, cols);
        spawnedWall2.name = $"Wall West";

        var spawnedWall3 = Instantiate(wallPrefab, new Vector3((rows + 1f) * 0.5f, 0.4f, 0), Quaternion.identity);
        spawnedWall3.transform.localScale = new Vector3(rows, 3f, 1f);
        spawnedWall3.name = $"Wall South";

        var spawnedWall4 = Instantiate(wallPrefab, new Vector3(rows + 1f, 0.4f, (cols + 1f) * 0.5f), Quaternion.identity);
        spawnedWall4.transform.localScale = new Vector3(1f, 3f, cols);
        spawnedWall4.name = $"Wall East";

        for (int x = 1; x < rows + 1; x++)
        {
            for (int z = 1; z < cols + 1; z++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {z}";

                var spawnedFloorPart = Instantiate(floorPrefab, new Vector3(x, -0.52f, z), Quaternion.identity);
                spawnedFloorPart.name = $"Floor part {x} {z}";
            }
        }

        var ground = Instantiate(floorPrefab, new Vector3((rows + 1f) * 0.5f, -0.85f, (cols + 1f) * 0.5f), Quaternion.identity);
        ground.transform.localScale = new Vector3(rows, 0.5f, cols);
        ground.name = $"Ground";

        playerRB.SetActive(false);
        player.position = new Vector3((float)rows * 0.5f - 0.5f, 2f, (float)cols * 0.5f - 0.5f);
        playerRB.SetActive(true);
    }
}
