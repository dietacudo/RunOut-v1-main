using UnityEngine;
using TMPro;

public class ObstacleSlideInteraction : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text buttonPromptText;
    public float reactionTime = 2f;

    [Header("Slide")]
    public float slideForce = 5f;
    public float reducedHeight = 0.5f;
    public float reducedOffset = 0.25f;
    public float resetTime = 1f; // czas po którym collider wraca

    private float timer;
    private string currentPrompt;
    private bool isNearObstacle = false;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private bool hasSlid = false;

    private CapsuleCollider2D playerCollider;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    void Start()
    {
        if (buttonPromptText != null) buttonPromptText.gameObject.SetActive(false);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            playerAnimator = player.GetComponent<Animator>();
            playerCollider = player.GetComponent<CapsuleCollider2D>();
        }

        if (playerCollider != null)
        {
            originalColliderSize = playerCollider.size;
            originalColliderOffset = playerCollider.offset;
        }

        // Upewnij się, że reakcje na starcie są odblokowane
        ReactionLock.Unlock();
    }

    void Update()
    {
        if (!isNearObstacle) return;

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
        if (timer <= 0f && !hasSlid)
        {
            HidePromptReset();
            return;
        }

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
            PerformSlide();
        }
        else
        {
            ReactionLock.Lock();

            if (buttonPromptText != null)
            {
                buttonPromptText.text = "Wrong!";
            }

            isNearObstacle = false;
            hasSlid = false;

            if (buttonPromptText != null) Invoke(nameof(HidePromptImmediate), 0.5f);

            // Możesz tu też wywołać śmierć gracza, jeśli chcesz natychmiastowy efekt.
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
        hasSlid = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
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
        buttonPromptText.text = "Press " + prompt + " to slide!";
        buttonPromptText.gameObject.SetActive(true);
    }

    private void PerformSlide()
    {
        if (hasSlid) return;
        hasSlid = true;

        if (buttonPromptText != null) buttonPromptText.gameObject.SetActive(false);
        isNearObstacle = false;

        if (playerAnimator != null) playerAnimator.SetTrigger("Slide");

        if (playerCollider != null)
        {
            playerCollider.size = new Vector2(originalColliderSize.x, reducedHeight);
            playerCollider.offset = new Vector2(originalColliderOffset.x, reducedOffset);
        }

        if (playerRb != null)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
            // Dodajemy pchnięcie w prawo; jeśli chcesz kierunek ruchu, rozważ użyć transform.right * slideForce
            playerRb.AddForce(Vector2.right * slideForce, ForceMode2D.Impulse);
        }

        // Przywróć collider po czasie
        Invoke(nameof(ResetCollider), resetTime);
    }

    private void ResetCollider()
    {
        if (playerCollider == null) return;
        playerCollider.size = originalColliderSize;
        playerCollider.offset = originalColliderOffset;
    }
}
