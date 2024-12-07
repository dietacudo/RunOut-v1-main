using UnityEngine;
using TMPro; // Importujemy TextMeshPro dla 3D

public class ObstacleInteraction : MonoBehaviour
{
    public TextMeshPro buttonPromptText; // Zmieniamy na TextMeshPro (dla 3D)
    public float reactionTime = 2f; // Czas na reakcję w sekundach
    private float timer; // Licznik czasu
    private string currentPrompt; // Aktualnie wyświetlany przycisk
    private bool isNearObstacle = false; // Czy gracz jest blisko przeszkody
    private Rigidbody2D playerRb; // Rigidbody gracza, potrzebne do skoku
    public float jumpForce = 10f; // Siła skoku

    private Animator playerAnimator; // Referencja do Animatora gracza
    private bool hasJumped = false; // Zapobiega podwójnym skokom

    void Start()
    {
        buttonPromptText.gameObject.SetActive(false); // Na początku przycisk jest ukryty
        GameObject player = GameObject.FindWithTag("Player"); // Znajdź gracza
        playerRb = player.GetComponent<Rigidbody2D>(); // Pobierz Rigidbody gracza
        playerAnimator = player.GetComponent<Animator>(); // Pobierz Animator gracza
    }

    void Update()
    {
        if (isNearObstacle)
        {
            timer -= Time.deltaTime; // Zmniejszanie czasu

            if (timer <= 0f && !hasJumped) // Jeśli czas minął, a gracz nie wykonał skoku
            {
                buttonPromptText.gameObject.SetActive(false); // Ukryj przycisk
                isNearObstacle = false;
                hasJumped = false; // Resetuje flagę
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
                PerformJump(); // Gracz nacisnął przycisk na czas
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
        buttonPromptText.text = "Press " + prompt + " to climb!"; // Nowy format tekstu
        buttonPromptText.gameObject.SetActive(true); // Pokazanie przycisku
    }

    private void PerformJump()
    {
        if (hasJumped) return; // Zapobieganie wielokrotnemu skokowi
        hasJumped = true; // Ustaw flagę skoku
        buttonPromptText.gameObject.SetActive(false); // Ukrycie przycisku
        isNearObstacle = false; // Gracz wykonał akcję

        // Wyzwól animację skoku
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Jump");
        }

        // Skok za pomocą Rigidbody2D
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0); // Ustalamy prędkość pionową na 0
        playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Dodajemy siłę w górę
    }
}
