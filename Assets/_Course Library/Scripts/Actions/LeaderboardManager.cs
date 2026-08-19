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
        public string playerName;
        public float time;

        public ScoreEntry(string name, float playerTime)
        {
            playerName = name;
            time = playerTime;
        }
    }

    private List<ScoreEntry> scores = new List<ScoreEntry>();

    // Keeps track of how many players have played
    private int playerNumber = 1;

    void Start()
    {
        UpdateLeaderboard();
    }

    public void AddScore(float time)
    {
        // Automatically create player name
        string playerName = "Player " + playerNumber;

        // Next completed attempt becomes next player
        playerNumber++;

        // Add result
        scores.Add(new ScoreEntry(playerName, time));

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
                    scores[i].playerName + "     " +
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