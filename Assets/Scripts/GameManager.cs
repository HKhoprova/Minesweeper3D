using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private enum GameState { NotStarted, Playing, Won, Lost }
    private GameState currentGameState = GameState.NotStarted;

    [Header("Managers")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private MineManager mineManager;
    [SerializeField] private UIManager uiManager;

    [Header("Grid Settings")]
    [SerializeField] private int rows = 10;
    [SerializeField] private int cols = 10;
    [SerializeField] private int mineCount = 10;
    private TileHolder[,] tileGrid;
    private Floor[,] floorGrid;

    [Header("Prefabs")]
    [SerializeField] private Mine minePrefab;
    [SerializeField] private SootStain sootPrefab;

    private int revealCount = 0;
    private int notFlaggedMinesCount = 0;
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
        notFlaggedMinesCount = mineCount;
        tileGrid = new TileHolder[rows, cols];
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
            uiManager.UpdateMineCounter(notFlaggedMinesCount);
        }
        else
        {
            Debug.LogError("UI Manager reference is missing!");
        }
    }

    public void OnTileClicked(GameObject tileHolderObject)
    {
        if (currentGameState == GameState.Won || currentGameState == GameState.Lost || uiManager.IsGamePaused())
            return;

        TileHolder tile = tileHolderObject.GetComponent<TileHolder>();
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
            if (mineManager != null && tileHolderObject != null)
            {
                isEmpty = mineManager.IsEmpty(tileCoords.Item1, tileCoords.Item2);
            }

            if (isEmpty)
            {
                RevealEmptyAround(tile, tileCoords.Item1, tileCoords.Item2);
            }
            else if (tile.TryRevealTile(currentGameState == GameState.Lost))
            {
                revealCount++;
            }
            else
            {
                return;
            }
        }

        bool isMine = false;
        if (mineManager != null && tileHolderObject != null)
        {
            isMine = mineManager.IsMine(tileCoords.Item1, tileCoords.Item2);
        }
        
        if (isMine)
        {
            int row = tileCoords.Item1;
            int col = tileCoords.Item2;
            var sootStain = Instantiate(sootPrefab, new Vector3(row, 0.001f, col), Quaternion.identity);
            sootStain.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            sootStain.name = $"Soot stain";
            OnGameLost();
            return;
        }
        
        if (revealCount == totalSafeTiles) 
        {
            OnGameWon();
        }
    }

    public void OnTileFlagged(GameObject tileHolderObject)
    {
        if (currentGameState == GameState.Won || currentGameState == GameState.Lost || uiManager.IsGamePaused())
            return;

        if (currentGameState == GameState.NotStarted)
        {
            OnTileClicked(tileHolderObject);
            return;
        }

        TileHolder tile = tileHolderObject.GetComponent<TileHolder>();
        if (tile != null)
        {
            tile.ToggleFlag();
            if (!tile.IsFlagged())
            {
                notFlaggedMinesCount++;
            }
            else
            {
                notFlaggedMinesCount--;
            }

            if (uiManager != null)
            {
                uiManager.UpdateMineCounter(notFlaggedMinesCount);
            }
        }
    }

    private void RevealEmptyAround(TileHolder tile, int row, int col)
    {
        if (tile == null)
            return;

        if (tile.IsRevealed() || tile.IsFlagged())
            return;

        if (tile.TryRevealTile(currentGameState == GameState.Lost))
        {
            revealCount++;
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (!mineManager.IsInBounds(i, j, rows, cols))
                        continue;

                    TileHolder checkTile = tileGrid[i, j];

                    if (checkTile.IsRevealed() || checkTile.IsFlagged())
                        continue;

                    if (mineManager.IsEmpty(i, j))
                    {
                        RevealEmptyAround(checkTile, i, j);
                    }
                    else
                    {
                        checkTile.TryRevealTile(currentGameState == GameState.Lost);
                        revealCount++;
                    }
                }
            }
        }
    }

    private void OnGameWon()
    {
        currentGameState = GameState.Won;

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
                TileHolder tile = tileGrid[row, col];

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
                TileHolder tile = tileGrid[row, col];

                if (tile == null)
                    continue;

                if (!tile.IsRevealed() && !tile.IsFlagged())
                {
                    tile.TryRevealTile(currentGameState == GameState.Lost);
                    if (mineManager.IsMine(row, col))
                    {
                        var spawnedMine = Instantiate(minePrefab, new Vector3(row, 0.001f, col), Quaternion.identity);
                        spawnedMine.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        spawnedMine.name = $"Mine at {row} {col}";
                    }
                }
                else if (tile.IsFlagged() && !mineManager.IsMine(row, col))
                {
                    tile.MarkAsIncorrect();
                }
            }

        }
    }
}
