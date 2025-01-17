using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using static UnityEngine.Rendering.DebugUI.Table;


public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text mineCounterText;

    [Header("Timer")]
    [SerializeField] private Timer timer;

    [Header("PauseCustomizing")]
    [SerializeField] private GameObject pauseBgSquared;
    [SerializeField] private GameObject pauseBgShaped;

    private bool isGamePaused = false;

    private void Start()
    {
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        timer.enabled = false;
        Time.timeScale = 1f;
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

    public void ActivateTimer()
    {
        timer.enabled = true;
    }

    public bool IsGamePaused()
    {
        return isGamePaused;
    }
}
