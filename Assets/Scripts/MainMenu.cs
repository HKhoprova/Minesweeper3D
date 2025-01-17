using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject images;
    [SerializeField] private float interval = 1.5f;

    private bool isVisible = true;

    private void Start()
    {
        if (images == null)
        {
            images = gameObject;
        }
        InvokeRepeating(nameof(ToggleVisibility), interval, interval);
    }

    private void ToggleVisibility()
    {
        isVisible = !isVisible;
        images.SetActive(isVisible);
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
        Application.Quit();
    }
}