using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BallReset : MonoBehaviour
{
    public TextMeshProUGUI deathMessageText;
    public TextMeshProUGUI deathCounterText;

    private bool gameOver = false;
    private int deathCount; // licznik śmierci dla bieżącej rozgrywki

    private void Start()
    {
        // Reset licznika przy starcie sceny
        deathCount = 0;
        UpdateDeathCounterUI();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Zwiększenie licznika śmierci
            deathCount++;
            UpdateDeathCounterUI();

            // Wyświetlenie komunikatu o śmierci
            ShowDeathMessage();
        }
    }

    private void ShowDeathMessage()
    {
        Time.timeScale = 0f; // zatrzymanie gry
        deathMessageText.text = "You Died! Press SPACE to Retry";
        gameOver = true;
    }

    private void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            // Reset sceny
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            Time.timeScale = 1f; // przywrócenie czasu
            gameOver = false;
        }
    }

    private void UpdateDeathCounterUI()
    {
        deathCounterText.text = "Deaths: " + deathCount;
    }
}
