using UnityEngine;

public class LevelFinish2 : MonoBehaviour
{
    private GameTimer timer;

    void Start()
    {
        timer = FindObjectOfType<GameTimer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (timer != null)
            {
                float finalTime = timer.GetFinalTime();
                SaveScore(finalTime, "Level2");
                Debug.Log($"Poziom 2 ukończony! Czas: {finalTime:F2} sekundy");
            }
        }
    }

    void SaveScore(float time, string levelKey)
    {
        float[] topScores = new float[3];
        for (int i = 0; i < 3; i++)
        {
            topScores[i] = PlayerPrefs.GetFloat(levelKey + "_BestTime" + i, float.MaxValue);
        }

        topScores[2] = time;
        System.Array.Sort(topScores);

        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetFloat(levelKey + "_BestTime" + i, topScores[i]);
        }

        PlayerPrefs.Save();
    }
}
