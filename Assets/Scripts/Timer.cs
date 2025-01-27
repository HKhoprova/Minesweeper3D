using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float time;
    private bool timeIsRunning = false;

    public TMP_Text timerText;

    private void Start()
    {
        timeIsRunning = true;
    }

    private void Update()
    {
        if (timeIsRunning)
        {
            time += Time.deltaTime;
            DisplayTime(time);
        }
    }

    private void DisplayTime (float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void DisplayTimeForTesting(float timeToDisplay)
    {
        DisplayTime(timeToDisplay);
    }
}
