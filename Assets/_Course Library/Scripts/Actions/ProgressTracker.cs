using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProgressTracker : MonoBehaviour
{
    [Header("Progress UI")]
    public GameObject progressPanel;
    public TMP_Text boxCountText;
    public TMP_Text timerText;
    public TMP_Text statusText;
    public TMP_Text percentageText;
    public Slider progressBar;

    [Header("Completion UI")]
    public GameObject completionPanel;
    public TMP_Text finalBoxText;
    public TMP_Text finalTimeText;
    public TMP_Text ratingText;

    [Header("Leaderboard")]
    public LeaderboardManager leaderboardManager;
    public TMP_InputField playerNameInput;

    [Header("Challenge Start")]
    public GameObject boxBlocker;

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

    // Called when Dump Boxes button is pressed
    public void StartTimer()
    {
        // Player must enter a name first
        if (playerNameInput == null ||
            string.IsNullOrWhiteSpace(playerNameInput.text))
        {
            if (statusText != null)
            {
                statusText.text = "Enter your name first!";
            }

            return;
        }

        // Start challenge only if it is not already running
        if (!timerRunning && completedBoxes < totalBoxes)
        {
            timerRunning = true;

            if (statusText != null)
            {
                statusText.text = "Keep Going!";
            }

            // Release / dump the boxes
            if (boxBlocker != null)
            {
                boxBlocker.SetActive(false);
            }
        }
    }

    // Called when ONE box is successfully completed
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

        // Add player's result to leaderboard
        if (leaderboardManager != null)
        {
            leaderboardManager.AddScore(elapsedTime);
        }

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

        // Hide normal progress panel
        if (progressPanel != null)
        {
            progressPanel.SetActive(false);
        }

        // Show completion panel
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

    // Turn BoxBlocker back on
    if (boxBlocker != null)
    {
        boxBlocker.SetActive(true);
    }

    // Clear previous player's name
    if (playerNameInput != null)
    {
        playerNameInput.text = "";

        // Stop the input field from capturing keyboard input
        playerNameInput.DeactivateInputField();
    }

    // Remove UI selection/focus
    if (EventSystem.current != null)
    {
        EventSystem.current.SetSelectedGameObject(null);
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

        if (statusText != null && completedBoxes < totalBoxes)
        {
            statusText.text = "Keep Going!";
        }

        float progress = 0f;

        if (totalBoxes > 0)
        {
            progress = (float)completedBoxes / totalBoxes;
        }

        if (progressBar != null)
        {
            progressBar.value = progress;
        }

        if (percentageText != null)
        {
            percentageText.text =
                (progress * 100f).ToString("F0") + "%";
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