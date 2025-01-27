using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using System;
using System.Reflection;

[TestFixture]
public class UnitTests
{
    [Test]
    public void LevelManager_UnlockLevel_UnlocksLevel()
    {
        // Arrange
        var levelManagerObject = new GameObject("LevelManager");
        var levelManager = levelManagerObject.AddComponent<LevelManager>();

        // Set LevelManager.Instance to the newly created LevelManager
        typeof(LevelManager).GetProperty("Instance").SetValue(null, levelManager);

        var testLevel = new Level("Test Level", 10, 10, 10, false, "", false);
        levelManager.squareLevels = new List<Level> { testLevel };

        // Act
        levelManager.UnlockLevel(0);

        // Assert
        Assert.IsTrue(testLevel.IsUnlocked, "Level should be unlocked after calling UnlockLevel.");
    }

    [Test]
    public void GridManager_GenerateGrid_SetsCorrectSize()
    {
        // Arrange
        var gridManagerObject = new GameObject("GridManager");
        var gridManager = gridManagerObject.AddComponent<GridManager>();

        // Create mock prefabs
        var tilePrefab = new GameObject("TilePrefab").AddComponent<TileHolder>();
        var wallPrefab = new GameObject("WallPrefab").AddComponent<Wall>();
        var floorPrefab = new GameObject("FloorPrefab").AddComponent<Floor>();

        // Assign prefabs to GridManager using reflection (private fields)
        gridManager.GetType().GetField("tilePrefab", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gridManager, tilePrefab);
        gridManager.GetType().GetField("wallPrefab", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gridManager, wallPrefab);
        gridManager.GetType().GetField("floorPrefab", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gridManager, floorPrefab);

        // Create and assign mock player and playerRB
        var player = new GameObject("Player").transform;
        var playerRB = new GameObject("PlayerRB");

        gridManager.GetType().GetField("player", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gridManager, player);
        gridManager.GetType().GetField("playerRB", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gridManager, playerRB);

        // Set the grid size
        gridManager.SetSize(5, 5);

        // Act
        gridManager.GenerateGrid();

        // Assert
        Assert.AreEqual(5, gridManager.GetTileGrid().GetLength(0), "Grid should have the correct number of rows.");
        Assert.AreEqual(5, gridManager.GetTileGrid().GetLength(1), "Grid should have the correct number of columns.");
    }

    [Test]
    public void Timer_DisplaysFormattedTime()
    {
        // Arrange
        var timerObject = new GameObject("Timer");
        var timer = timerObject.AddComponent<Timer>();

        // Create a TextMesh Pro object
        var textObject = new GameObject("TimerText");
        var text = textObject.AddComponent<TextMeshProUGUI>();
        timer.timerText = text;

        // Act
        timer.DisplayTimeForTesting(125.5f); // 2 minutes and 5.5 seconds

        // Assert
        Assert.AreEqual("02:05", text.text, "Timer should display formatted time as mm:ss.");
    }

    [Test]
    public void TutorialImages_ToggleVisibility_SetsActiveState()
    {
        // Arrange
        var tutorialImages = new GameObject().AddComponent<TutorialImages>();
        var prefab = new GameObject();
        tutorialImages.imagesPrefab = prefab;
        tutorialImages.InitializeForTesting();

        // Act
        tutorialImages.ToggleVisibilityForTesting();

        // Assert
        Assert.IsFalse(tutorialImages.imagesInstance.activeSelf, "Images should be inactive after toggling visibility.");
    }

    [Test]
    public void MineManager_GeneratesCorrectNumberOfMines()
    {
        // Arrange
        var mineManager = new GameObject("MineManager").AddComponent<MineManager>();

        // Mock GameManager and set it as the singleton instance
        var gameManagerObject = new GameObject("GameManager");
        var gameManager = gameManagerObject.AddComponent<GameManager>();
        GameManager.Instance = gameManager;

        // Mock grid dimensions and safe zone
        int rows = 10;
        int cols = 10;
        int mineCount = 15;
        var safeTileCoords = Tuple.Create(0, 0);

        // Create a mocked floor grid
        var floorGrid = new Floor[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var floorObject = new GameObject($"Floor_{i}_{j}");
                floorGrid[i, j] = floorObject.AddComponent<Floor>();
            }
        }

        // Initialize tileGrid in GameManager
        gameManager.tileGrid = new TileHolder[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var tileObject = new GameObject($"Tile_{i}_{j}");
                var tileHolder = tileObject.AddComponent<TileHolder>();
                gameManager.tileGrid[i, j] = tileHolder;
            }
        }

        // Act
        mineManager.GenerateMinesAndAssignValues(rows, cols, mineCount, safeTileCoords, floorGrid);

        // Count mines
        int actualMineCount = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (mineManager.IsMine(i, j))
                {
                    actualMineCount++;
                }
            }
        }

        // Assert
        Assert.AreEqual(mineCount, actualMineCount, "The number of mines generated should match the specified count.");
    }

    [Test]
    public void LevelManager_SetLevelNames_DisplaysLockedLevels()
    {
        // Arrange
        var levelManagerObject = new GameObject("LevelManager");
        var levelManager = levelManagerObject.AddComponent<LevelManager>();

        var dropdownObject = new GameObject("Dropdown");
        var dropdown = dropdownObject.AddComponent<TMP_Dropdown>();
        levelManager.GetType().GetField("squareLevelDropdown", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(levelManager, dropdown);

        levelManager.squareLevels = new List<Level>
    {
        new Level("Level 1", 10, 10, 10, false, "", true),
        new Level("Level 2", 10, 10, 15, false, "", false) // Locked
    };

        // Act
        levelManager.SetLevelNamesForTesting(dropdown, levelManager.squareLevels);

        // Assert
        Assert.AreEqual("Level 2 (locked)", dropdown.options[1].text, "Locked levels should display '(locked)' in the dropdown.");
    }

    [Test]
    public void MineManager_NoMinesInSafeZone()
    {
        // Arrange
        var mineManagerObject = new GameObject("MineManager");
        var mineManager = mineManagerObject.AddComponent<MineManager>();

        int rows = 10;
        int cols = 10;
        int mineCount = 15;
        var safeTileCoords = Tuple.Create(5, 5);

        var floorGrid = new Floor[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                floorGrid[i, j] = new GameObject($"Floor_{i}_{j}").AddComponent<Floor>();
            }
        }

        // Act
        mineManager.GenerateMinesAndAssignValues(rows, cols, mineCount, safeTileCoords, floorGrid);

        // Assert
        for (int i = 4; i <= 6; i++)
        {
            for (int j = 4; j <= 6; j++)
            {
                Assert.IsFalse(mineManager.IsMine(i, j), $"Tile ({i},{j}) in the safe zone should not contain a mine.");
            }
        }
    }

    [Test]
    public void PlayerController_JumpResetsAfterCooldown()
    {
        // Arrange
        var playerObject = new GameObject("Player");
        var playerController = playerObject.AddComponent<PlayerController>();

        // Access private fields via reflection
        var jumpCooldownField = playerController.GetType().GetField("jumpCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
        var readyToJumpField = playerController.GetType().GetField("readyToJump", BindingFlags.NonPublic | BindingFlags.Instance);

        jumpCooldownField.SetValue(playerController, 0.1f);
        readyToJumpField.SetValue(playerController, false);

        // Act
        playerController.GetType().GetMethod("ResetJump", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(playerController, null);

        // Assert
        var readyToJump = (bool)readyToJumpField.GetValue(playerController);
        Assert.IsTrue(readyToJump, "Player should be ready to jump after the cooldown period.");
    }
}