using UnityEngine;
using TMPro;

public class LeaderboardDisplay : MonoBehaviour
{
    public TextMeshProUGUI[] scoreTexts; // Przeciągnij tu 3 pola tekstowe w inspectorze

    void Start()
    {
        DisplayScores();
    }

    void DisplayScores()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            float time = PlayerPrefs.GetFloat("BestTime" + i, float.MaxValue);

            if (time == float.MaxValue)
            {
                scoreTexts[i].text = (i + 1) + ". ---";
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
