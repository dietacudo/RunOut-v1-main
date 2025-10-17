using UnityEngine;
using TMPro;

public class ResetScores : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI confirmationText;

    public void ResetAllScores()
    {
        // Usuń wyniki dla poziomu 1
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.DeleteKey("Level1_BestTime" + i);
        }

        // Usuń wyniki dla poziomu 2
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.DeleteKey("Level2_BestTime" + i);
        }

        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.DeleteKey("Level3_BestTime" + i);
        }

        PlayerPrefs.Save();

        Debug.Log("✅ All leaderboard scores have been reset!");

        if (confirmationText != null)
        {
            confirmationText.text = "All scores have been reset!";
        }
    }
}
