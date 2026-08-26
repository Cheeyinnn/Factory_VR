using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Leaderboard Rows")]
    public GameObject rank1Row;
    public GameObject rank2Row;
    public GameObject rank3Row;
    public GameObject rank4Row;
    public GameObject rank5Row;

    [Header("Leaderboard Text")]
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

    private int roundNumber = 1;

    void Start()
    {
        UpdateLeaderboard();
    }

    public void AddScore(float time)
    {
        string roundName = "Round " + roundNumber;

        roundNumber++;

        scores.Add(new ScoreEntry(roundName, time));

        // Fastest first
        scores.Sort((a, b) => a.time.CompareTo(b.time));

        // Keep only Top 5
        if (scores.Count > 5)
        {
            scores.RemoveAt(scores.Count - 1);
        }

        UpdateLeaderboard();
    }

    private void UpdateLeaderboard()
    {
        GameObject[] rankRows =
        {
            rank1Row,
            rank2Row,
            rank3Row,
            rank4Row,
            rank5Row
        };

        TMP_Text[] rankTexts =
        {
            rank1Text,
            rank2Text,
            rank3Text,
            rank4Text,
            rank5Text
        };

        for (int i = 0; i < rankRows.Length; i++)
        {
            if (i < scores.Count)
            {
                // Show this row
                if (rankRows[i] != null)
                {
                    rankRows[i].SetActive(true);
                }

                int minutes =
                    Mathf.FloorToInt(scores[i].time / 60f);

                int seconds =
                    Mathf.FloorToInt(scores[i].time % 60f);

                if (rankTexts[i] != null)
                {
                    rankTexts[i].text =
                        scores[i].roundName +
                        "\t\t " +
                        minutes.ToString("00") +
                        ":" +
                        seconds.ToString("00");
                }
            }
            else
            {
                // Hide unused rows
                if (rankRows[i] != null)
                {
                    rankRows[i].SetActive(false);
                }
            }
        }
    }
}