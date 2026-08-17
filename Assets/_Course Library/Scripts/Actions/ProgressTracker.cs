using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProgressTracker : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text boxCountText;
    public TMP_Text timerText;
    public TMP_Text statusText;
    public Slider progressBar;

    [Header("Progress")]
    public int totalBoxes = 5;

    private int completedBoxes = 0;
    private float elapsedTime = 0f;
    private bool timerRunning = false;

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.minValue = 0f;
            progressBar.maxValue = 1f;
            progressBar.value = 0f;
        }

        UpdateUI();
        UpdateTimer();
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimer();
        }
    }

    // Start timer when Dump Boxes is pressed
    public void StartTimer()
    {
        if (!timerRunning && completedBoxes < totalBoxes)
        {
            timerRunning = true;
        }
    }

    // Called when a box is placed on the platform
    public void BoxCompleted()
    {
        if (completedBoxes >= totalBoxes)
            return;

        completedBoxes++;
        UpdateUI();

        if (completedBoxes >= totalBoxes)
        {
            timerRunning = false;
            statusText.text = "Completed!";
        }
    }

    // Called when Reset Button is pressed
    public void ResetProgress()
    {
        completedBoxes = 0;
        elapsedTime = 0f;
        timerRunning = false;

        UpdateUI();
        UpdateTimer();

        statusText.text = "Keep Going!";
    }

    void UpdateUI()
    {
        if (boxCountText != null)
        {
            boxCountText.text =
                "Boxes: " + completedBoxes + " / " + totalBoxes;
        }

        if (progressBar != null)
        {
            float progress = (float)completedBoxes / totalBoxes;
            progressBar.value = progress;
        }

        if (statusText != null && completedBoxes < totalBoxes)
        {
            statusText.text = "Keep Going!";
        }
    }

    void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (timerText != null)
        {
            timerText.text =
                "Time: " +
                minutes.ToString("00") +
                ":" +
                seconds.ToString("00");
        }
    }
}