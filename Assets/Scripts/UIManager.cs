using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text mineCounterText;
    [SerializeField] private CanvasGroup levelNamePanel;
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float displayTime = 2.0f;

    [Header("Timer")]
    [SerializeField] private Timer timer;

    [Header("Pause")]
    [SerializeField] private GameObject pauseBgSquared;
    [SerializeField] private GameObject pauseBgShaped;
    [SerializeField] private GameObject nextLevelButton;

    private bool isGamePaused = false;

    private void Start()
    {
        nextLevelButton.gameObject.SetActive(false);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        timer.enabled = false;
        Time.timeScale = 1f;

        if (levelNameText != null)
        {
            if (LevelManager.Instance != null)
                levelNameText.text = "Level: " + LevelManager.Instance.SelectedLevel.Name;
            StartCoroutine(ShowAndFadeLevelName());
        }
    }

    private void Update()
    {
        // Handle pressing ESC to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            // Freeze game and show pause panel
            Time.timeScale = 0f;
            timer.enabled = false; // Stop the timer
            pausePanel.SetActive(true);
            if (GameManager.Instance.isCustomShape) pauseBgShaped.SetActive(true);
            else pauseBgSquared.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Unfreeze game and hide pause panel
            Time.timeScale = 1f;
            if (GameManager.Instance.IsGameRunning())
                timer.enabled = true; // Resume the timer
            pausePanel.SetActive(false);
            if (GameManager.Instance.isCustomShape) pauseBgShaped.SetActive(false);
            else pauseBgSquared.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void UpdateMineCounter(int remainingMines)
    {
        mineCounterText.text = $"{remainingMines}";
    }

    private IEnumerator ShowAndFadeLevelName()
    {
        levelNamePanel.alpha = 1f;

        yield return new WaitForSeconds(displayTime);

        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            levelNamePanel.alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            yield return null;
        }

        levelNamePanel.alpha = 0.0f;
    }

    public void ShowWinScreen()
    {
        winPanel.SetActive(true);
        timer.enabled = false;
    }

    public void ShowLoseScreen()
    {
        losePanel.SetActive(true);
        timer.enabled = false;
    }

    public void RestartGame()
    {
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        Time.timeScale = 0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        if (LevelManager.Instance != null)
        {
            Destroy(LevelManager.Instance.gameObject);
        }

        Time.timeScale = 0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ContinueToNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
        RestartGame();
    }

    public void ActivateTimer()
    {
        timer.enabled = true;
    }

    public bool IsGamePaused()
    {
        return isGamePaused;
    }

    public void ActivateNextLevelButton()
    {
        nextLevelButton.gameObject.SetActive(true);
    }
}
