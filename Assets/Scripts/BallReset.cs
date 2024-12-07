using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // Użycie TextMesh Pro

public class BallReset : MonoBehaviour
{
    // UI elementy dla komunikatu o śmierci oraz licznika śmierci
    public TextMeshProUGUI deathMessageText;
    public TextMeshProUGUI deathCounterText;

    // Flaga, która sprawdza, czy gra została zatrzymana
    private bool gameOver = false;

    // Zmienna przechowująca liczbę śmierci gracza
    private int deathCount;

    private void Start()
    {
        // Odczytaj liczbę śmierci z PlayerPrefs, jeśli istnieje
        deathCount = PlayerPrefs.GetInt("DeathCount", 0);
        UpdateDeathCounterUI();
    }

    // Funkcja wywoływana, gdy kula dotknie obiektu
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sprawdzenie, czy obiekt to gracz
        if (collision.gameObject.CompareTag("Player"))
        {
            // Zwiększenie licznika śmierci
            deathCount++;

            // Zapisanie nowej liczby śmierci w PlayerPrefs
            PlayerPrefs.SetInt("DeathCount", deathCount);

            // Wyświetlenie komunikatu o śmierci
            ShowDeathMessage();
        }
    }

    // Funkcja wyświetlająca komunikat o śmierci
    private void ShowDeathMessage()
    {
        // Zatrzymanie gry
        Time.timeScale = 0f;

        // Włączenie komunikatu
        deathMessageText.text = "You Died! Press SPACE to Retry";

        // Ustawienie flagi na true, by gra była zakończona
        gameOver = true;
    }

    // Funkcja Update do sprawdzania naciśnięcia spacji
    private void Update()
    {
        // Jeśli gra jest zakończona i gracz naciśnie spację, zresetuj poziom
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            // Resetowanie sceny po naciśnięciu spacji
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            // Przywrócenie czasu (restart gry)
            Time.timeScale = 1f;

            // Resetowanie flagi
            gameOver = false;

            // Zaktualizowanie liczby śmierci po resecie
            UpdateDeathCounterUI();
        }
    }

    // Funkcja aktualizująca UI z liczbą śmierci
    private void UpdateDeathCounterUI()
    {
        deathCounterText.text = "Deaths: " + deathCount;
    }
}
