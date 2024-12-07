using UnityEngine;
using TMPro; // Importujemy TextMeshPro dla 3D

public class ObstacleInteraction : MonoBehaviour
{
    public TextMeshPro buttonPromptText; // Wyświetl tekst dla przeszkody
    public float reactionTime = 2f; // Czas na reakcję w sekundach
    private float timer; // Licznik czasu
    private string currentPrompt; // Aktualnie wyświetlany przycisk
    private bool isNearObstacle = false; // Czy gracz jest blisko przeszkody
    private Rigidbody2D playerRb; // Rigidbody gracza, potrzebne do ruchu
    public float jumpForce = 10f; // Siła skoku
    public float slideSpeed = 15f; // Prędkość wślizgu
    public float slideDuration = 1f; // Czas trwania wślizgu
    private bool isSliding = false; // Czy gracz wykonuje wślizg

    private Animator playerAnimator; // Referencja do Animatora gracza
    private BoxCollider2D playerCollider; // Collider gracza
    private Vector2 originalSize; // Oryginalny rozmiar collidra
    private Vector2 slideSize; // Rozmiar podczas wślizgu
    private string requiredAction; // Określa, jaka akcja jest wymagana ("Jump" lub "Slide")

    void Start()
    {
        buttonPromptText.gameObject.SetActive(false); // Ukrycie tekstu na początku
        GameObject player = GameObject.FindWithTag("Player"); // Znajdź gracza
        playerRb = player.GetComponent<Rigidbody2D>();
        playerAnimator = player.GetComponent<Animator>();
        playerCollider = player.GetComponent<BoxCollider2D>();
        originalSize = playerCollider.size;
        slideSize = new Vector2(originalSize.x, originalSize.y / 2); // Ustaw mniejszy rozmiar na wślizg
    }

    void Update()
    {
        if (isNearObstacle)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f && !isSliding && requiredAction != "Jump") // Brak akcji = gracz przegrywa
            {
                buttonPromptText.gameObject.SetActive(false);
                isNearObstacle = false;
            }

            if (Input.GetKeyDown(KeyCode.Q) && currentPrompt == "Q" ||
                Input.GetKeyDown(KeyCode.W) && currentPrompt == "W" ||
                Input.GetKeyDown(KeyCode.E) && currentPrompt == "E" ||
                Input.GetKeyDown(KeyCode.A) && currentPrompt == "A" ||
                Input.GetKeyDown(KeyCode.S) && currentPrompt == "S" ||
                Input.GetKeyDown(KeyCode.D) && currentPrompt == "D")
            {
                if (requiredAction == "Jump")
                    PerformJump();
                else if (requiredAction == "Slide")
                    PerformSlide();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return; // Ignoruj gracza

        ObstacleType obstacle = other.GetComponent<ObstacleType>();
        if (obstacle != null)
        {
            requiredAction = obstacle.requiredAction; // Pobierz akcję dla przeszkody
            isNearObstacle = true;
            timer = reactionTime;
            currentPrompt = GetRandomButton();
            ShowButtonPrompt(currentPrompt, requiredAction);
        }
    }

    private void ShowButtonPrompt(string prompt, string action)
    {
        buttonPromptText.text = $"Press {prompt} to {action}!";
        buttonPromptText.gameObject.SetActive(true);
    }

    private string GetRandomButton()
    {
        string[] buttonPrompts = { "Q", "W", "E", "A", "S", "D" };
        return buttonPrompts[Random.Range(0, buttonPrompts.Length)];
    }

    private void PerformJump()
    {
        buttonPromptText.gameObject.SetActive(false);
        isNearObstacle = false;

        playerAnimator?.SetTrigger("Jump");
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
        playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void PerformSlide()
    {
        buttonPromptText.gameObject.SetActive(false);
        isNearObstacle = false;

        playerAnimator?.SetTrigger("Slide");
        isSliding = true;

        playerCollider.size = slideSize;
        playerRb.linearVelocity = new Vector2(slideSpeed, playerRb.linearVelocity.y);

        Invoke(nameof(EndSlide), slideDuration);
    }

    private void EndSlide()
    {
        isSliding = false;
        playerCollider.size = originalSize;
    }
}
