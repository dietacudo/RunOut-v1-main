using UnityEngine;
using TMPro;

public class GameStartInfo : MonoBehaviour
{
    public GameObject infoPanel; // Panel z informacjami (cały panel UI)
    public TextMeshProUGUI infoText; // Tekst w panelu
    public bool gameStarted = false;

    void Start()
    {
        // Sprawdzamy, czy to pierwsze uruchomienie gry
        if (PlayerPrefs.GetInt("GameStarted", 0) == 0) // 0 oznacza, że gra nigdy wcześniej nie była uruchomiona
        {
            // Pokazujemy panel powitalny
            infoPanel.SetActive(true);
            // Zatrzymujemy czas, aby gracz mógł przeczytać informacje
            Time.timeScale = 0f;

            // Zapisujemy, że gra została uruchomiona
            PlayerPrefs.SetInt("GameStarted", 1);
            PlayerPrefs.Save();
        }
        else
        {
            // Jeśli gra była już uruchomiona, nie pokazujemy panelu powitalnego
            infoPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Czekamy, aż gracz naciśnie spację, żeby rozpocząć grę
        if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
        {
            StartGame(); // Rozpoczęcie gry
        }
    }

    // Funkcja rozpoczynająca grę
    void StartGame()
    {
        gameStarted = true; // Ustaw flagę, że gra się rozpoczęła
        infoPanel.SetActive(false); // Ukrycie całego panelu, łącznie z tłem i tekstem
        Time.timeScale = 1f; // Przywrócenie normalnego czasu (gra się zaczyna)
    }
}
