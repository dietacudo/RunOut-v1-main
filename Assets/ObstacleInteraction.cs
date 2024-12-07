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

    void Start()
    {
        buttonPromptText.gameObject.SetActive(false); // Na początku przycisk jest ukryty
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>(); // Znajdź Rigidbody gracza
    }

    void Update()
    {
        if (isNearObstacle)
        {
            timer -= Time.deltaTime; // Zmniejszanie czasu

            if (timer <= 0f) // Jeśli czas minął
            {
                buttonPromptText.gameObject.SetActive(false); // Ukryj przycisk
                isNearObstacle = false; // Gracz przegrał
                // Można dodać konsekwencje np. uderzenie w przeszkodę
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

    // Funkcja wywoływana, gdy gracz zbliży się do przeszkody
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

    // Funkcja losująca przycisk z grupy Q, W, E, A, S, D
    private string GetRandomButton()
    {
        string[] buttonPrompts = { "Q", "W", "E", "A", "S", "D" }; // Możliwe przyciski
        return buttonPrompts[Random.Range(0, buttonPrompts.Length)];
    }

    // Wyświetlanie losowego przycisku w przestrzeni 3D (nad przeszkodą)
    private void ShowButtonPrompt(string prompt)
    {
        buttonPromptText.text = "Press " + prompt + " to climb!"; // Nowy format tekstu
        buttonPromptText.gameObject.SetActive(true); // Pokazanie przycisku
    }

    // Funkcja wykonująca skok
    private void PerformJump()
    {
        buttonPromptText.gameObject.SetActive(false); // Ukrycie przycisku
        isNearObstacle = false; // Gracz wykonał akcję

        // Skok za pomocą Rigidbody2D
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0); // Ustalamy prędkość pionową na 0, by skok nie był zależny od poprzedniego ruchu
        playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Dodajemy siłę w górę, aby wykonać skok

        Debug.Log("Gracz przeskoczył przeszkodę!");
    }
}
