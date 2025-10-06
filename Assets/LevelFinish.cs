using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    private GameTimer timer; // Odwołanie do timera

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
                SaveScore(finalTime);
                Debug.Log($"Poziom ukończony! Czas: {finalTime:F2} sekundy");
            }
        }
    }

    void SaveScore(float time)
    {
        // Wczytaj obecne 3 najlepsze wyniki
        float[] topScores = new float[3];
        for (int i = 0; i < 3; i++)
        {
            topScores[i] = PlayerPrefs.GetFloat("BestTime" + i, float.MaxValue);
        }

        // Dodaj nowy wynik i posortuj
        topScores[2] = time;
        System.Array.Sort(topScores);

        // Zapisz Top 3
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetFloat("BestTime" + i, topScores[i]);
        }

        PlayerPrefs.Save();
    }
}
