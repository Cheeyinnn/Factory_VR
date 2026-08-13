using UnityEngine;
using TMPro;

public class ProgressTracker : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text boxCountText;
    public TMP_Text timerText;
    public TMP_Text statusText;

    [Header("Progress")]
    public int totalBoxes = 5;

    private int completedBoxes = 0;
    private float elapsedTime = 0f;
    private bool timerRunning = true;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimer();
        }
    }

    // Call this when a box is successfully placed/grabbed
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

    void UpdateUI()
    {
        boxCountText.text = "Boxes: " + completedBoxes + " / " + totalBoxes;

        if (completedBoxes < totalBoxes)
            statusText.text = "Keep Going!";
    }

    void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = "Time: " + minutes.ToString("00")
                       + ":" + seconds.ToString("00");
    }
}