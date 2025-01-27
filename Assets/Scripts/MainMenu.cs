using UnityEngine;

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

    public void InitializeForTesting()
    {
        Start();
    }
}