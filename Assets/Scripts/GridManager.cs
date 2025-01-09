using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;

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
        //for (int x = 0; x < width + 2; x++)
        //{
        //    for (int z = 0; z < height + 2; z++)
        //    {
        //        if ((x == 0 || z == 0) || (x == width + 1 || z == height + 1))
        //        {
        //            var spawnedWall = Instantiate(wallPrefab, new Vector3(x, 0, z), Quaternion.identity);
        //            spawnedWall.name = $"Wall {x} {z}";
        //        }
        //        else
        //        {
        //            var spawnedTile = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
        //            spawnedTile.name = $"Tile {x} {z}";
        //        }

        //        var spawnedFloorPart = Instantiate(floorPrefab, new Vector3(x, -1, z), Quaternion.identity);
        //        spawnedFloorPart.name = $"Floor part {x} {z}";
        //    }

        var spawnedWall1 = Instantiate(wallPrefab, new Vector3((width + 1f) * 0.5f, 0, height + 1f), Quaternion.identity);
        spawnedWall1.transform.localScale = new Vector3(width, 3, 1);
        spawnedWall1.name = $"Wall North";

        var spawnedWall2 = Instantiate(wallPrefab, new Vector3(0, 0, (height + 1f) * 0.5f), Quaternion.identity);
        spawnedWall2.transform.localScale = new Vector3(1, 3, height);
        spawnedWall2.name = $"Wall West";

        var spawnedWall3 = Instantiate(wallPrefab, new Vector3((width + 1f) * 0.5f, 0, 0), Quaternion.identity);
        spawnedWall3.transform.localScale = new Vector3(width, 3, 1);
        spawnedWall3.name = $"Wall South";

        var spawnedWall4 = Instantiate(wallPrefab, new Vector3(width + 1f, 0, (height + 1f) * 0.5f), Quaternion.identity);
        spawnedWall4.transform.localScale = new Vector3(1, 3, height);
        spawnedWall4.name = $"Wall East";

        for (int x = 1; x < width + 1; x++)
        {
            for (int z = 1; z < height + 1; z++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {z}";

                var spawnedFloorPart = Instantiate(floorPrefab, new Vector3(x, -1, z), Quaternion.identity);
                spawnedFloorPart.name = $"Floor part {x} {z}";
            }
        }

        playerRB.SetActive(false);
        player.position = new Vector3((float)width * 0.5f - 0.5f, 2, (float)height * 0.5f - 0.5f);
        playerRB.SetActive(true);
    }
}
