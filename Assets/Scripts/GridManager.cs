using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Wall _wallPrefab;
    [SerializeField] private Floor _floorPrefab;
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _playerRB;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < _width + 2; x++)
        {
            for (int z = 0; z < _height + 2; z++)
            {
                if ((x == 0 || z == 0) || (x == _width + 1 || z == _height + 1))
                {
                    var spawnedWall = Instantiate(_wallPrefab, new Vector3(x, 0, z), Quaternion.identity);
                    spawnedWall.name = $"Wall {x} {z}";
                }
                else
                {
                    var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {z}";
                }

                var spawnedFloorPart = Instantiate(_floorPrefab, new Vector3(x, -1, z), Quaternion.identity);
                spawnedFloorPart.name = $"Floor part {x} {z}";
            }
        }

        _playerRB.SetActive(false);
        _player.position = new Vector3((float)_width/2 - 0.5f, 2, (float)_height/2 - 0.5f);
        _playerRB.SetActive(true);
    }
}
