using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Leaderboard UI")]
    public TMP_Text rank1Text;
    public TMP_Text rank2Text;
    public TMP_Text rank3Text;
    public TMP_Text rank4Text;
    public TMP_Text rank5Text;

    private class ScoreEntry
    {
        public string roundName;
        public float time;

        public ScoreEntry(string name, float roundTime)
        {
            roundName = name;
            time = roundTime;
        }
    }

    private List<ScoreEntry> scores = new List<ScoreEntry>();

    // Keeps track of how many rounds have been completed
    private int roundNumber = 1;

    void Start()
    {
        UpdateLeaderboard();
    }

    public void AddScore(float time)
    {
        // Automatically create round name
        string roundName = "Round " + roundNumber;

        // Next completed challenge becomes next round
        roundNumber++;

        // Add result
        scores.Add(new ScoreEntry(roundName, time));

        // Sort fastest to slowest
        scores.Sort((a, b) => a.time.CompareTo(b.time));

        // Only keep the fastest Top 5
        if (scores.Count > 5)
        {
            scores.RemoveAt(scores.Count - 1);
        }

        UpdateLeaderboard();
    }

    private void UpdateLeaderboard()
    {
        TMP_Text[] rankTexts =
        {
            rank1Text,
            rank2Text,
            rank3Text,
            rank4Text,
            rank5Text
        };

        for (int i = 0; i < rankTexts.Length; i++)
        {
            if (rankTexts[i] == null)
                continue;

            if (i < scores.Count)
            {
                int minutes = Mathf.FloorToInt(scores[i].time / 60f);
                int seconds = Mathf.FloorToInt(scores[i].time % 60f);

                rankTexts[i].text =
                    (i + 1) + ". " +
                    scores[i].roundName + "     " +
                    minutes.ToString("00") + ":" +
                    seconds.ToString("00");
            }
            else
            {
                rankTexts[i].text =
                    (i + 1) + ". ---";
            }
        }
    }
}