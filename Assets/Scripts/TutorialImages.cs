using UnityEngine;

public class TutorialImages : MonoBehaviour
{
    public GameObject imagesPrefab;
    public GameObject imagesInstance;
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

    public void InitializeForTesting()
    {
        Start();
    }

    public void ToggleVisibilityForTesting()
    {
        ToggleVisibility();
    }
}
