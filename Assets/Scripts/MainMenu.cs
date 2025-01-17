using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
        Application.Quit();
    }
}