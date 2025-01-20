using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public List<Level> squareLevels;
    public List<Level> shapedLevels;

    public Level SelectedLevel { get; set; }
    public int selectedLevelIndex = 0;


    [Header("Square Levels UI")]
    [SerializeField] private TMP_Dropdown squareLevelDropdown;
    [SerializeField] private Button playSquareLevel;
    [SerializeField] private TextMeshProUGUI squareSizeText;
    [SerializeField] private TextMeshProUGUI squareMinesText;
    [SerializeField] private GameObject lockedSquareLevelText;

    [Header("Shaped Levels UI")]
    [SerializeField] private TMP_Dropdown shapedLevelDropdown;
    [SerializeField] private Button playShapedLevel;
    [SerializeField] private TextMeshProUGUI shapedSizeText;
    [SerializeField] private TextMeshProUGUI shapedMinesText;
    [SerializeField] private GameObject lockedShapedLevelText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadLevelData();

        SetLevelNames(squareLevelDropdown, squareLevels);
        shapedLevelDropdown.SetValueWithoutNotify(0);
        RefreshSquareLevelInfo(0);
        squareLevelDropdown.onValueChanged.AddListener(index => RefreshSquareLevelInfo(index));

        SetLevelNames(shapedLevelDropdown, shapedLevels);
        shapedLevelDropdown.SetValueWithoutNotify(0);
        RefreshShapedLevelInfo(0);
        shapedLevelDropdown.onValueChanged.AddListener(index => RefreshShapedLevelInfo(index));

        lockedSquareLevelText.SetActive(false);
        lockedShapedLevelText.SetActive(false);
    }

    private void RemoveListeners()
    {
        squareLevelDropdown.onValueChanged.RemoveAllListeners();
        shapedLevelDropdown.onValueChanged.RemoveAllListeners();
    }
    public string GetDocumentsPath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Minesweeper3D").Replace('/', '\\');
    }

    private string GetLevelDataFilePath()
    {
        return Path.Combine(GetDocumentsPath(), "levels.json");
    }

    private void SaveLevelData()
    {
        string folderPath = GetDocumentsPath();
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = GetLevelDataFilePath();
        LevelCollection levelData = new LevelCollection
        {
            squareLevels = squareLevels,
            shapedLevels = shapedLevels
        };

        string json = JsonUtility.ToJson(levelData, true);
        Debug.Log("Saved to file");
        File.WriteAllText(filePath, json);
    }

    private void LoadLevelData()
    {
        string filePath = GetLevelDataFilePath();

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LevelCollection levelData = JsonUtility.FromJson<LevelCollection>(json);

            if (levelData != null &&
            levelData.squareLevels != null &&
            levelData.shapedLevels != null &&
            levelData.squareLevels.Count > 0 &&
            levelData.shapedLevels.Count > 0)
            {
                squareLevels = levelData.squareLevels;
                shapedLevels = levelData.shapedLevels;
                return;
            }
        }

        CreateDefaultLevelData();
        SaveLevelData();
    }

    private void CreateDefaultLevelData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath.Replace('/', '\\'), "levels.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LevelCollection levelData = JsonUtility.FromJson<LevelCollection>(json);

            if (levelData != null &&
            levelData.squareLevels != null &&
            levelData.shapedLevels != null &&
            levelData.squareLevels.Count > 0 &&
            levelData.shapedLevels.Count > 0)
            {
                squareLevels = levelData.squareLevels;
                shapedLevels = levelData.shapedLevels;
                return;
            }
        }

        squareLevels = new List<Level>
        {
            new Level("Easy 1", 10, 10, 10, false, "", true),
            new Level("Easy 2", 10, 15, 15, false, "", true),
            new Level("Normal 1", 12, 12, 22, false, "", false),
            new Level("Normal 2", 12, 18, 32, false, "", false),
            new Level("Hard 1", 15, 15, 45, false, "", false),
            new Level("Hard 2", 15, 20, 60, false, "", false),
            new Level("Expert 1", 18, 18, 81, false, "", false),
            new Level("Expert 2", 18, 25, 113, false, "", false)
        };

        shapedLevels = new List<Level>
        {
            new Level("Flower", 17, 17, 35, true, "shape1", true),
            new Level("Heart", 17, 15, 30, true, "shape2", false),
            new Level("Circle", 15, 15, 35, true, "shape3", false),
            new Level("Clock", 21, 21, 70, true, "shape4", false),
            new Level("Five", 18, 27, 105, true, "shape5", false)
        };
    }

    public void LoadSquareLevel()
    {
        int index = squareLevelDropdown.value;

        if (index < 0 || index >= squareLevels.Count || !squareLevels[index].IsUnlocked)
            return;

        RemoveListeners();
        SelectedLevel = squareLevels[index];
        selectedLevelIndex = index;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadShapedLevel()
    {
        int index = shapedLevelDropdown.value;

        if (index < 0 || index >= shapedLevels.Count || !shapedLevels[index].IsUnlocked)
            return;

        RemoveListeners();
        SelectedLevel = shapedLevels[index];
        selectedLevelIndex = index;
        SceneManager.LoadScene("GameScene");
    }

    public void UnlockLevel(int index)
    {
        if (index >= 0 && index < squareLevels.Count)
        {
            squareLevels[index].IsUnlocked = true;
            Instance.SaveLevelData();
        }
    }

    private void RefreshSquareLevelInfo(int selectedIndex)
    {
        RefreshLevelInfo(selectedIndex, squareLevels, squareSizeText, squareMinesText, playSquareLevel, lockedSquareLevelText);
    }

    private void RefreshShapedLevelInfo(int selectedIndex)
    {
        RefreshLevelInfo(selectedIndex, shapedLevels, shapedSizeText, shapedMinesText, playShapedLevel, lockedShapedLevelText);
    }

    private void RefreshLevelInfo(int selectedIndex, List<Level> levels, TextMeshProUGUI sizeText, TextMeshProUGUI minesText, Button playButton, GameObject lockedLevelText)
    {
        if (selectedIndex < 0 || selectedIndex >= levels.Count)
        {
            sizeText.text = "Size: -";
            minesText.text = "Mines: -";
            playButton.interactable = false;
            lockedLevelText.SetActive(false);
            return;
        }

        Level selectedLevel = levels[selectedIndex];

        if (!selectedLevel.IsUnlocked)
        {
            sizeText.text = "";
            minesText.text = "";
            playButton.interactable = false;
            lockedLevelText.SetActive(true);
            return;
        }

        int tiles = selectedLevel.Rows * selectedLevel.Cols;

        if (selectedLevel.IsCustomShape)
        {
            tiles = CountTilesInShapeFile(selectedLevel.ShapeFile);
            sizeText.text = $"Tiles: {tiles}";
        }
        else
        {
            sizeText.text = $"Size: {selectedLevel.Rows}x{selectedLevel.Cols}";
        }

        minesText.text = $"Mines: {selectedLevel.Mines}";
        lockedLevelText.SetActive(false);

        if (selectedLevel.Mines < 0 || tiles < 16 || selectedLevel.Mines > tiles)
        {
            playButton.interactable = false;
        }
        else
        {
            playButton.interactable = true;
        }
    }

    private void SetLevelNames(TMP_Dropdown dropdown, List<Level> levels)
    {
        dropdown.ClearOptions();

        List<string> levelNames = new List<string>();
        List<int> lockedLevels = new List<int>();

        int i = 0;
        foreach (var level in levels)
        {
            levelNames.Add(level.Name);
            if (!level.IsUnlocked)
            {
                lockedLevels.Add(i);
            }
            i++;
        }

        dropdown.AddOptions(levelNames);

        foreach (var levelIndex in lockedLevels)
        {
            dropdown.options[levelIndex].text = levelNames[levelIndex] + " (locked)";
        }

        dropdown.RefreshShownValue();
    }

    public int CountTilesInShapeFile(string filePath)
    {
        filePath = Path.Combine(Application.streamingAssetsPath.Replace('/', '\\'), filePath);
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("File doesn't exist: " + filePath);
            return  -1;
        }

        int tileCount = 0;

        foreach (var line in File.ReadLines(filePath))
        {
            foreach (char c in line)
            {
                if (c == '1') tileCount++;
            }
        }

        return tileCount;
    }

    public void LevelWon()
    {
        if (selectedLevelIndex < 0 || selectedLevelIndex >= squareLevels.Count)
        {
            Debug.LogError("Selected level index out of bounds!");
            return;
        }

        if (SelectedLevel.IsCustomShape && selectedLevelIndex + 1 < shapedLevels.Count)
        {
            shapedLevels[selectedLevelIndex + 1].IsUnlocked = true;
        }
        else if (!SelectedLevel.IsCustomShape)
        {
            int pairIndex = selectedLevelIndex / 2;

            int nextPairStartIndex = (pairIndex + 1) * 2;

            if (nextPairStartIndex < squareLevels.Count)
            {
                squareLevels[nextPairStartIndex].IsUnlocked = true;

                if (nextPairStartIndex + 1 < squareLevels.Count)
                {
                    squareLevels[nextPairStartIndex + 1].IsUnlocked = true;
                }
            }
        }
        SaveLevelData();
    }
}


[Serializable]
public class LevelCollection
{
    public List<Level> squareLevels;
    public List<Level> shapedLevels;
}
