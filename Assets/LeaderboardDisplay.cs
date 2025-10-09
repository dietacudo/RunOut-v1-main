using UnityEngine;
using TMPro;

public class LeaderboardDisplay : MonoBehaviour
{
    [Header("Level 1 Scores")]
    public TextMeshProUGUI[] level1ScoreTexts; // 3 pola tekstowe dla poziomu 1

    [Header("Level 2 Scores")]
    public TextMeshProUGUI[] level2ScoreTexts; // 3 pola tekstowe dla poziomu 2

    void Start()
    {
        DisplayScores("Level1", level1ScoreTexts);
        DisplayScores("Level2", level2ScoreTexts);
    }

    void DisplayScores(string levelKey, TextMeshProUGUI[] scoreTexts)
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            float time = PlayerPrefs.GetFloat(levelKey + "_BestTime" + i, float.MaxValue);

            if (time == float.MaxValue)
            {
                scoreTexts[i].text = $"{i + 1}. ---";
            }
            else
            {
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                int milliseconds = Mathf.FloorToInt((time * 100) % 100);
                scoreTexts[i].text = $"{i + 1}. {minutes:00}:{seconds:00}:{milliseconds:00}";
            }
        }
    }
}
