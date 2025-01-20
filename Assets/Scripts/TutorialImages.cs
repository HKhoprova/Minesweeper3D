using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialImages : MonoBehaviour
{
    [SerializeField] private GameObject imagesPrefab;
    private GameObject imagesInstance;
    [SerializeField] private float interval = 1.5f;

    private bool isVisible = true;

    private void Start()
    {
        if (imagesInstance == null)
        {
            imagesInstance = Instantiate(imagesPrefab, transform);
        }

        Time.timeScale = 1f;
        CancelInvoke();
        InvokeRepeating(nameof(ToggleVisibility), interval, interval);
    }

    private void ToggleVisibility()
    {
        isVisible = !isVisible;
        imagesInstance.SetActive(isVisible);
    }

    private void OnDestroy()
    {
        if (imagesInstance != null)
        {
            Destroy(imagesInstance);
        }
        CancelInvoke(nameof(ToggleVisibility));
    }
}
