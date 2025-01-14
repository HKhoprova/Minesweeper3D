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
    [SerializeField] private MineManager mineManager;
    [SerializeField] private UIManager uiManager;

    [Header("Grid Settings")]
    public int rows = 10;
    public int cols = 10;
    public int mineCount = 10;
    private Tile[,] tileGrid;
    private Floor[,] floorGrid;

    private int revealCount = 0;
    private int notFlaggedCount = 0;
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
        Debug.Log("start level!");
    }

    public void InitializeGame()
    {
        currentGameState = GameState.NotStarted;

        revealCount = 0;
        totalSafeTiles = rows * cols - mineCount;
        notFlaggedCount = mineCount;
        tileGrid = new Tile[rows, cols];
        floorGrid = new Floor[rows, cols];

        if (gridManager != null)
        {
            gridManager.GenerateGrid();
            tileGrid = gridManager.GetTileGrid();
            floorGrid = gridManager.GetFloorGrid();
        }
        else
        {
            Debug.LogError("Grid Manager reference is missing!");
        }

        if (uiManager != null)
        {
            uiManager.UpdateMineCounter(notFlaggedCount);
        }
        else
        {
            Debug.LogError("UI Manager reference is missing!");
        }
    }

    public void OnTileClicked(GameObject tileObject)
    {
        if (currentGameState == GameState.Won || currentGameState == GameState.Lost || uiManager.IsGamePaused())
            return;

        Tile tile = tileObject.GetComponent<Tile>();
        Tuple<int, int> tileCoords = tile.GetCoords();

        // Check if the game started; if not, generates mines
        if (currentGameState == GameState.NotStarted)
        {
            currentGameState = GameState.Playing;

            if (mineManager != null && tileCoords != null)
            {
                mineManager.GenerateMinesAndAssignValues(rows, cols, mineCount, tileCoords, floorGrid);
            }
            else
            {
                Debug.LogError("Mine Manager reference is missing!");
            }
        }

        if (tile != null)
        {
            bool isEmpty = false;
            if (mineManager != null && tileObject != null)
            {
                isEmpty = mineManager.IsEmpty(tileCoords.Item1, tileCoords.Item2);
            }

            if (isEmpty)
            {
                RevealEmptyAround(tile, tileCoords.Item1, tileCoords.Item2);
            }
            else if (tile.TryRevealTile())
            {
                revealCount++;
            }
            else
            {
                return;
            }
        }

        bool isMine = false;
        if (mineManager != null && tileObject != null)
        {
            isMine = mineManager.IsMine(tileCoords.Item1, tileCoords.Item2);
        }
        
        if (isMine)
        {
            OnGameLost();
            return;
        }
        
        if (revealCount == totalSafeTiles) 
        {
            OnGameWon();
        }
    }

    public void OnTileFlagged(GameObject tileObject)
    {
        if (currentGameState == GameState.Won || currentGameState == GameState.Lost || uiManager.IsGamePaused())
            return;

        if (currentGameState == GameState.NotStarted)
        {
            OnTileClicked(tileObject);
            return;
        }

        Tile tile = tileObject.GetComponent<Tile>();
        if (tile != null)
        {
            tile.ToggleFlag();
            if (!tile.IsFlagged())
            {
                notFlaggedCount++;
            }
            else
            {
                notFlaggedCount--;
            }
        }

        if (uiManager != null)
        {
            if (tile.IsFlagged())
                uiManager.UpdateMineCounter(notFlaggedCount);
        }
    }

    private void RevealEmptyAround(Tile tile, int row, int col)
    {
        if (tile == null)
            return;

        if (tile.IsRevealed() || tile.IsFlagged())
            return;

        if (tile.TryRevealTile())
        {
            revealCount++;
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (!mineManager.IsInBounds(i, j, rows, cols))
                        continue;

                    Tile checkTile = tileGrid[i, j];

                    if (checkTile.IsRevealed() || checkTile.IsFlagged())
                        continue;

                    if (mineManager.IsEmpty(i, j))
                    {
                        RevealEmptyAround(checkTile, i, j);
                    }
                    else
                    {
                        checkTile.TryRevealTile();
                        revealCount++;
                    }
                }
            }
        }
    }

    private void OnGameWon()
    {
        currentGameState = GameState.Won;

        Debug.Log("You win!");

        if (uiManager != null)
        {
            uiManager.ShowWinScreen();
        }

        FlagAllMines();
    }

    private void FlagAllMines()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Tile tile = tileGrid[row, col];

                if (tile == null)
                    continue;

                if (mineManager.IsMine(row, col) && !tile.IsFlagged())
                {
                    tile.ToggleFlag();
                }
            }
        }
    }

    private void OnGameLost()
    {
        currentGameState = GameState.Lost;

        Debug.Log("You lose!");

        if (uiManager != null)
        {
            uiManager.ShowLoseScreen();
        }

        RevealAllTiles();
    }

    public void RevealAllTiles()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Tile tile = tileGrid[row, col];

                if (tile == null)
                    continue;

                if (!tile.IsRevealed() && !tile.IsFlagged())
                {
                    tile.TryRevealTile();
                }
                else if (tile.IsFlagged() && !mineManager.IsMine(row, col))
                {
                    tile.MarkAsIncorrect();
                }
            }
        }
    }
}
