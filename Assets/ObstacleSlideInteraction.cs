using UnityEngine;
using TMPro; // Import TextMeshPro

public class ObstacleSlideInteraction : MonoBehaviour
{
    public TextMeshPro buttonPromptText; // Zmieniamy na TextMeshPro (dla 3D)
    public float reactionTime = 2f; // Czas na reakcję w sekundach
    private float timer; // Licznik czasu
    private string currentPrompt; // Aktualnie wyświetlany przycisk
    private bool isNearObstacle = false; // Czy gracz jest blisko przeszkody
    private Rigidbody2D playerRb; // Rigidbody gracza, potrzebne do wślizgu
    public float slideForce = 5f; // Siła wślizgu

    private Animator playerAnimator; // Referencja do Animatora gracza
    private bool hasSlid = false; // Zapobiega wielokrotnym wślizgom

    private CapsuleCollider2D playerCollider; // Kolider gracza (CapsuleCollider2D)
    public float reducedHeight = 0.5f; // Zmniejszenie wysokości kolidera podczas wślizgu
    public float reducedOffset = 0.25f; // Offset do dopasowania kolidera w dół
    private Vector2 originalColliderSize; // Oryginalny rozmiar kolidera
    private Vector2 originalColliderOffset; // Oryginalny offset kolidera

    public float resetTime = 1f; // Czas po wślizgu, po którym kolider wraca do oryginalnych rozmiarów

    void Start()
    {
        buttonPromptText.gameObject.SetActive(false); // Na początku przycisk jest ukryty
        GameObject player = GameObject.FindWithTag("Player"); // Znajdź gracza
        playerRb = player.GetComponent<Rigidbody2D>(); // Pobierz Rigidbody gracza
        playerAnimator = player.GetComponent<Animator>(); // Pobierz Animator gracza
        playerCollider = player.GetComponent<CapsuleCollider2D>(); // Pobierz CapsuleCollider2D gracza

        // Zapisz oryginalny rozmiar i offset kolidera
        originalColliderSize = playerCollider.size;
        originalColliderOffset = playerCollider.offset;
    }

    void Update()
    {
        if (isNearObstacle)
        {
            timer -= Time.deltaTime; // Zmniejszanie czasu

            if (timer <= 0f && !hasSlid) // Jeśli czas minął, a gracz nie wykonał wślizgu
            {
                buttonPromptText.gameObject.SetActive(false); // Ukryj przycisk
                isNearObstacle = false;
                hasSlid = false; // Resetuje flagę
                // Możesz dodać inne konsekwencje np. uderzenie w przeszkodę
            }

            // Sprawdzanie, czy gracz nacisnął odpowiedni przycisk
            if ((currentPrompt == "Q" && Input.GetKeyDown(KeyCode.Q)) ||
                (currentPrompt == "W" && Input.GetKeyDown(KeyCode.W)) ||
                (currentPrompt == "E" && Input.GetKeyDown(KeyCode.E)) ||
                (currentPrompt == "A" && Input.GetKeyDown(KeyCode.A)) ||
                (currentPrompt == "S" && Input.GetKeyDown(KeyCode.S)) ||
                (currentPrompt == "D" && Input.GetKeyDown(KeyCode.D)))
            {
                PerformSlide(); // Gracz nacisnął przycisk na czas
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Sprawdzenie, czy to gracz
        {
            isNearObstacle = true;
            timer = reactionTime; // Resetowanie czasu na 2 sekundy
            currentPrompt = GetRandomButton(); // Losowanie przycisku
            ShowButtonPrompt(currentPrompt); // Pokazanie przycisku
        }
    }

    private string GetRandomButton()
    {
        string[] buttonPrompts = { "Q", "W", "E", "A", "S", "D" }; // Możliwe przyciski
        return buttonPrompts[Random.Range(0, buttonPrompts.Length)];
    }

    private void ShowButtonPrompt(string prompt)
    {
        buttonPromptText.text = "Press " + prompt + " to slide!"; // Zmieniony komunikat na "slide"
        buttonPromptText.gameObject.SetActive(true); // Pokazanie przycisku
    }

    private void PerformSlide()
    {
        if (hasSlid) return; // Zapobieganie wielokrotnemu wślizgowi
        hasSlid = true; // Ustaw flagę wślizgu
        buttonPromptText.gameObject.SetActive(false); // Ukrycie przycisku
        isNearObstacle = false; // Gracz wykonał akcję

        // Wyzwól animację wślizgu
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Slide");
        }

        // Zmiana kolidera na mniejszy podczas wślizgu
        playerCollider.size = new Vector2(originalColliderSize.x, reducedHeight); // Zmniejsz wysokość
        playerCollider.offset = new Vector2(originalColliderOffset.x, reducedOffset); // Dopasuj offset w dół (przesunięcie górnej części kolidera)

        // Wślizg za pomocą Rigidbody2D
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0); // Ustalamy prędkość pionową na 0
        playerRb.AddForce(Vector2.right * slideForce, ForceMode2D.Impulse); // Dodajemy siłę w prawo

        // Przywrócenie kolidera do oryginalnych rozmiarów po czasie
        Invoke("ResetCollider", resetTime); // Przywrócenie kolidera po upływie czasu
    }

    // Przywrócenie kolidera do oryginalnych rozmiarów
    private void ResetCollider()
    {
        playerCollider.size = originalColliderSize;
        playerCollider.offset = originalColliderOffset;
    }
}
