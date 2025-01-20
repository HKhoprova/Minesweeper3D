using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        // Set the game to run in windowed fullscreen mode
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        Debug.Log("Set to Windowed Fullscreen Mode");
    }

    public void PlaySquare()
    {
        LevelManager.Instance.LoadSquareLevel();
    }
    public void PlayShaped()
    {
        LevelManager.Instance.LoadShapedLevel();
    }

    public void Quit()
    {
        Debug.Log("Quitting application.");
        Application.Quit();
    }
}