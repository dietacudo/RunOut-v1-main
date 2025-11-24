using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BallReset : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI deathMessageText;
    public TextMeshProUGUI deathCounterText;

    [Header("Audio")]
    public AudioSource ballAudio;      // AudioSource kuli, który zatrzymujemy przy śmierci
    public AudioSource audioSource;    // AudioSource do odtwarzania dźwięku śmierci
    public AudioClip deathClip;        // Clip dźwięku śmierci gracza

    private bool gameOver = false;
    private static int deathCount = 0; // licznik śmierci, nie resetuje się przy zmianie sceny

    private void Start()
    {
        UpdateDeathCounterUI();
        if (deathMessageText != null) deathMessageText.text = "";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1️⃣ Zatrzymanie dźwięku kuli
            if (ballAudio != null)
                ballAudio.Stop();

            // 2️⃣ Odtwarzanie dźwięku śmierci gracza
            if (audioSource != null && deathClip != null)
                audioSource.PlayOneShot(deathClip);

            // 3️⃣ Zwiększenie licznika śmierci
            deathCount++;
            UpdateDeathCounterUI();

            // 4️⃣ Wyświetlenie komunikatu o śmierci i zatrzymanie gry
            ShowDeathMessage();
        }
    }

    private void ShowDeathMessage()
    {
        Time.timeScale = 0f; // zatrzymanie gry
        if (deathMessageText != null) deathMessageText.text = "You Died! Press SPACE to Retry";
        gameOver = true;
    }

    private void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            // Najpierw odblokowujemy reakcje (ReactionLock) żeby po respawnie grac mógł znów reagować
            ReactionLock.Unlock();

            // Reset sceny
            Time.timeScale = 1f; // przywrócenie czasu PRZED reloadem (bez tego niektóre rzeczy mogą zostać zatrzymane)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            gameOver = false;
        }
    }

    private void UpdateDeathCounterUI()
    {
        if (deathCounterText != null)
            deathCounterText.text = "Deaths: " + deathCount;
    }

    // 🔥 Funkcja do resetowania licznika (np. w menu)
    public void ResetDeathCounter()
    {
        deathCount = 0;
        UpdateDeathCounterUI();
    }
}
