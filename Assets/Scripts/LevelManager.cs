using System.Collections;
using System.Collections.Generic;
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

    public Level SelectedLevel { get; private set; }


    [Header("Square Levels UI")]
    [SerializeField] private TMP_Dropdown squareLevelDropdown;
    [SerializeField] private Button playSquareLevel;
    [SerializeField] private TextMeshProUGUI squareSizeText;
    [SerializeField] private TextMeshProUGUI squareMinesText;
    [SerializeField] private GameObject lockedSquareLevelText;

    [Header("Shaped Levels UI")]
    [SerializeField] private TMP_Dropdown shapedLevelDropdown;
    [SerializeField] private Button playShapedLevel;
    [SerializeField] private TextMeshProUGUI shapedDifficultyText;
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

    public void LoadSquareLevel()
    {
        int index = squareLevelDropdown.value;

        if (index < 0 || index >= squareLevels.Count || !squareLevels[index].IsUnlocked)
            return;

        RemoveListeners();
        SelectedLevel = squareLevels[index];
        SceneManager.LoadScene("GameScene");
    }

    public void LoadShapedLevel()
    {
        int index = shapedLevelDropdown.value;

        if (index < 0 || index >= shapedLevels.Count || !shapedLevels[index].IsUnlocked)
            return;

        RemoveListeners();
        SelectedLevel = shapedLevels[index];
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
        RefreshLevelInfo(selectedIndex, shapedLevels, shapedSizeText, shapedMinesText, playShapedLevel, lockedShapedLevelText, shapedDifficultyText);
    }

    private void RefreshLevelInfo(int selectedIndex, List<Level> levels, TextMeshProUGUI sizeText, TextMeshProUGUI minesText, Button playButton, GameObject lockedLevelText, TextMeshProUGUI difficultyText = null)
    {
        if (selectedIndex < 0 || selectedIndex >= levels.Count)
        {
            sizeText.text = "Size: -";
            minesText.text = "Mines: -";
            playButton.interactable = false;
            if (difficultyText != null) difficultyText.text = "Difficulty: -";
            lockedLevelText.SetActive(false);
            return;
        }

        Level selectedLevel = levels[selectedIndex];
        int tiles = 0;

        if (!selectedLevel.IsUnlocked)
        {
            sizeText.text = "";
            minesText.text = "";
            playButton.interactable = false;
            if (difficultyText != null) difficultyText.text = "";
            lockedLevelText.SetActive(true);
            return;
        }

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
        if (difficultyText != null) difficultyText.text = $"Difficulty: {CalculateShapedLevelDifficulty(tiles, selectedLevel.Mines)}";
        lockedLevelText.SetActive(false);

        if (selectedLevel.Mines < 0 || tiles < 0)
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

    private int CountTilesInShapeFile(string filePath)
    {
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

    private string CalculateShapedLevelDifficulty(int tiles, int mines)
    {
        if (tiles < 0 || tiles > mines || mines < 0)
            return "Unacceptable";

        float density = mines / tiles;

        if (density < 0 || density >= 0.5) return "Impossible";
        else if (density <= 0.1) return "Easy";
        else if (density <= 0.15) return "Normal";
        else if (density <= 0.2) return "Hard";
        else return "Expert";
    }

    private void SaveLevelData()
    {
        // Save `levels` list to JSON or PlayerPrefs.
    }

    private void LoadLevelData()
    {
        // Load `levels` list from JSON or PlayerPrefs.

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
            new Level("Shape 1", 11, 10, 15, true, "path/to/shape1", true),
            new Level("Shape 2", 15, 15, 30, true, "path/to/shape2", false),
            new Level("Shape 3", 20, 20, 45, true, "path/to/shape3", false),
            new Level("Shape 4", 25, 25, 50, true, "path/to/shape4", false)
        };
    }
}
