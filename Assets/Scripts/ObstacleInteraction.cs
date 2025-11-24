using UnityEngine;
using TMPro;

public class ObstacleInteraction : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text buttonPromptText;      // TextMeshPro (UI lub 3D TMP_Text)
    public float reactionTime = 2f;        // Czas na odpowiedź

    [Header("Jump")]
    public float jumpForce = 10f;          // Siła skoku

    private float timer;
    private string currentPrompt;
    private bool isNearObstacle = false;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private bool hasJumped = false;

    void Start()
    {
        if (buttonPromptText != null) buttonPromptText.gameObject.SetActive(false);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            playerAnimator = player.GetComponent<Animator>();
        }

        // Upewnij się, że reakcje na starcie są odblokowane
        ReactionLock.Unlock();
    }

    void Update()
    {
        if (!isNearObstacle) return;

        // Jeśli globalnie zablokowane — ignorujemy wejścia (timer dalej może lecieć)
        if (ReactionLock.Locked)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                HidePromptReset();
            }
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f && !hasJumped)
        {
            HidePromptReset();
            return;
        }

        // Sprawdź naciśnięcia monitorowanych klawiszy
        bool q = Input.GetKeyDown(KeyCode.Q);
        bool w = Input.GetKeyDown(KeyCode.W);
        bool e = Input.GetKeyDown(KeyCode.E);
        bool a = Input.GetKeyDown(KeyCode.A);
        bool s = Input.GetKeyDown(KeyCode.S);
        bool d = Input.GetKeyDown(KeyCode.D);

        bool any = q || w || e || a || s || d;
        if (!any) return;

        string pressed = q ? "Q" : w ? "W" : e ? "E" : a ? "A" : s ? "S" : "D";

        if (pressed == currentPrompt)
        {
            PerformJump();
        }
        else
        {
            // Złe wciśnięcie -> trwała blokada reakcji aż do respawnu
            ReactionLock.Lock();

            if (buttonPromptText != null)
            {
                buttonPromptText.text = "Wrong!";
            }

            // Tu możesz też wywołać logikę śmierci (jeśli chcesz, by złe wciśnięcie natychmiastowo zabijało gracza).
            // Przykład: player.GetComponent<PlayerHealth>()?.Die();

            isNearObstacle = false;
            hasJumped = false;

            if (buttonPromptText != null) Invoke(nameof(HidePromptImmediate), 0.5f);
        }
    }

    private void HidePromptImmediate()
    {
        if (buttonPromptText != null) buttonPromptText.gameObject.SetActive(false);
    }

    private void HidePromptReset()
    {
        if (buttonPromptText != null) buttonPromptText.gameObject.SetActive(false);
        isNearObstacle = false;
        hasJumped = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Jeżeli reakcje zablokowane, nie pokazujemy promptu (ale możesz chcieć pokazać komunikat)
        if (ReactionLock.Locked) return;

        isNearObstacle = true;
        timer = reactionTime;
        currentPrompt = GetRandomButton();
        ShowButtonPrompt(currentPrompt);
    }

    private string GetRandomButton()
    {
        string[] buttonPrompts = { "Q", "W", "E", "A", "S", "D" };
        return buttonPrompts[Random.Range(0, buttonPrompts.Length)];
    }

    private void ShowButtonPrompt(string prompt)
    {
        if (buttonPromptText == null) return;
        buttonPromptText.text = "Press " + prompt + " to jump!";
        buttonPromptText.gameObject.SetActive(true);
    }

    private void PerformJump()
    {
        if (hasJumped) return;
        hasJumped = true;

        if (buttonPromptText != null) buttonPromptText.gameObject.SetActive(false);
        isNearObstacle = false;

        if (playerAnimator != null) playerAnimator.SetTrigger("Jump");

        if (playerRb != null)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
