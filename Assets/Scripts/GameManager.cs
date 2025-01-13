using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { NotStarted, Playing, Won, Lost }
    private GameState currentGameState = GameState.NotStarted;

    [Header("Managers")]
    [SerializeField] private GridManager gridManager;
    //[SerializeField] private MineManager mineManager;   // Future script
    //[SerializeField] private UIManager uiManager;       // Future script

    [Header("Grid Settings")]
    public int rows = 10;
    public int cols = 10;
    public int mineCount = 10;

    private int revealCount = 0;
    private int totalSafeTiles;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        currentGameState = GameState.NotStarted;

        revealCount = 0;
        totalSafeTiles = rows * cols - mineCount;

        if (gridManager != null)
        {
            gridManager.GenerateGrid();
        }
        else
        {
            Debug.LogError("Grid Manager reference is missing!");
        }

        //if (uiManager != null)
        //{
        //    //reset ui (timer, mine count)
        //}
        //else
        //{
        //    Debug.LogError("UI Manager reference is missing!");
        //}
    }

    public void OnTileClicked(GameObject tileObject)
    {
        if (currentGameState == GameState.Won || currentGameState == GameState.Lost)
            return;

        // Check if the game started; if not, generates mines
        if (currentGameState == GameState.NotStarted)
        {
            currentGameState = GameState.Playing;
            
            Tuple<int, int> tileCoords = ParseTileName(tileObject.name);

            //if (mineManager != null && tileCoords != null)
            //{
            //    mineManager.GenerateMines(rows, cols, mineCount, tileCoords);
            //}
            //else
            //{
            //    Debug.LogError("Mine Manager reference is missing!");
            //}
        }

        Tile tile = tileObject.GetComponent<Tile>();
        if (tile != null)
        {
            if (tile.TryDestroyTile())
            {
                revealCount++;
            }
            else
            {
                return;
            }
        }

        bool isMine = false;
        //if (mineManager != null && tileObject != null)
        //{
        //    isMine = mineManager.IsMine(tileCoords.Item1, tileCoords.Item2);
        //}
        
        if (isMine)
        {
            OnGameLost();
        }
        else if (revealCount == totalSafeTiles) 
        {
            OnGameWon();
        }
    }

    public void OnTileFlagged(GameObject tileObject)
    {
        if (currentGameState == GameState.Won || currentGameState == GameState.Lost)
            return;

        Tile tile = tileObject.GetComponent<Tile>();
        if (tile != null)
        {
            tile.ToggleFlag();
        }
        //if (uiManager != null)
        //{
        //    uiManager.UpdateMinesLeft(tile.IsFlagged()); // +1 if tile flagged and -1 if not flagged
        //}
    }

    private void OnGameWon()
    {
        currentGameState = GameState.Won;

        Debug.Log("You win!");

        //if (uiManager != null)
        //{
        //    uiManager.ShowWinPanel();
        //}
    }

    private void OnGameLost()
    {
        currentGameState = GameState.Lost;

        Debug.Log("You lose!");

        //if (uiManager != null)
        //{
        //    uiManager.ShowGameOverPanel();
        //}
    }

    private Tuple<int, int> ParseTileName(string tileName)
    {
        // Expected format: "Tile X Y"
        string[] parts = tileName.Split(' ');
        if (parts.Length < 3) return null;

        // parts[0] = "Tile", parts[1] = "X", parts[2] = "Y"
        int x, y;
        if (int.TryParse(parts[1], out x) && int.TryParse(parts[2], out y))
        {
            return Tuple.Create(x, y);
        }
        return null;
    }
}
