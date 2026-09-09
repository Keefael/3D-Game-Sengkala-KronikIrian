using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float startTime = 90f; // 1 menit 30 detik
    
    private float timeRemaining;
    private bool isRunning = true;

    void Start()
    {
        timeRemaining = startTime;
        UpdateDisplay();
    }

    void Update()
    {
        if (!isRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateDisplay();
        }
        else
        {
            timeRemaining = 0;
            UpdateDisplay();
            Debug.Log("Waktu Habis!");
            isRunning = false;
        }
    }

    void UpdateDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}