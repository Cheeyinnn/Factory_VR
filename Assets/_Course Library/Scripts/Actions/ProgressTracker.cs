using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProgressTracker : MonoBehaviour
{
    [Header("Progress UI")]
    public GameObject progressPanel;
    public TMP_Text boxCountText;
    public TMP_Text timerText;
    public TMP_Text statusText;
    public Slider progressBar;

    [Header("Completion UI")]
    public GameObject completionPanel;
    public TMP_Text finalBoxText;
    public TMP_Text finalTimeText;
    public TMP_Text ratingText;

    [Header("Progress Settings")]
    public int totalBoxes = 5;

    [Header("Rating Settings")]
    public float threeStarTime = 60f;
    public float twoStarTime = 120f;

    private int completedBoxes = 0;
    private float elapsedTime = 0f;
    private bool timerRunning = false;

    void Start()
    {
        ResetProgress();
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimer();
        }
    }

    // Call this when the Dump Boxes button is pressed
    public void StartTimer()
    {
        if (!timerRunning && completedBoxes < totalBoxes)
        {
            timerRunning = true;
        }
    }

    // Call this when ONE box is successfully completed
    public void BoxCompleted()
    {
        if (completedBoxes >= totalBoxes)
            return;

        completedBoxes++;

        UpdateProgressUI();

        if (completedBoxes >= totalBoxes)
        {
            CompleteChallenge();
        }
    }

    private void CompleteChallenge()
    {
        timerRunning = false;

        // Final box result
        if (finalBoxText != null)
        {
            finalBoxText.text =
                "Boxes: " + completedBoxes + " / " + totalBoxes;
        }

        // Final time
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (finalTimeText != null)
        {
            finalTimeText.text =
                "Time: " +
                minutes.ToString("00") +
                ":" +
                seconds.ToString("00");
        }

        // Rating
        if (ratingText != null)
        {
            if (elapsedTime <= threeStarTime)
            {
                ratingText.text = "Rating: Excellent!";
            }
            else if (elapsedTime <= twoStarTime)
            {
                ratingText.text = "Rating: Good!";
            }
            else
            {
                ratingText.text = "Rating: Needs Improvement";
            }
        }

        // Switch panels
        if (progressPanel != null)
        {
            progressPanel.SetActive(false);
        }

        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }
    }

    // Called by Reset Button
    public void ResetProgress()
    {
        completedBoxes = 0;
        elapsedTime = 0f;
        timerRunning = false;

        // Show normal progress panel
        if (progressPanel != null)
        {
            progressPanel.SetActive(true);
        }

        // Hide completion panel
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }

        // Reset progress bar
        if (progressBar != null)
        {
            progressBar.minValue = 0f;
            progressBar.maxValue = 1f;
            progressBar.value = 0f;
        }

        UpdateProgressUI();
        UpdateTimer();
    }

    private void UpdateProgressUI()
    {
        if (boxCountText != null)
        {
            boxCountText.text =
                "Boxes: " + completedBoxes + " / " + totalBoxes;
        }

        if (statusText != null)
        {
            if (completedBoxes < totalBoxes)
            {
                statusText.text = "Keep Going!";
            }
        }

        if (progressBar != null)
        {
            progressBar.value =
                (float)completedBoxes / totalBoxes;
        }
    }

    private void UpdateTimer()
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