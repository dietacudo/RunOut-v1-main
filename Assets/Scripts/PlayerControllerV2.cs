using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    public float moveSpeed = 5f;        // Początkowa prędkość ruchu
    public float maxMoveSpeed = 15f;    // Maksymalna prędkość ruchu
    public float speedIncreaseRate = 0.5f;  // Ilość zwiększania prędkości na kliknięcie
    public float speedDecayRate = 1f;       // Szybkość zmniejszania prędkości przy braku aktywności

    [Header("Ustawienia skoku")]
    public float jumpForce = 10f;      // Siła skoku
    public Transform groundCheck;      // Punkt sprawdzający kontakt z ziemią
    public float groundCheckRadius = 0.2f;  // Promień sprawdzania ziemi
    public LayerMask groundLayer;      // Warstwa ziemi

    [Header("Ustawienia zanikania prędkości")]
    public float decayDelay = 1f;          // Czas bezczynności przed zmniejszaniem prędkości

    private float currentMoveSpeed = 0f;   // Aktualna prędkość ruchu
    public float CurrentMoveSpeed => currentMoveSpeed; // Właściwość tylko do odczytu

    private Rigidbody2D rb;
    private Animator animator;             // Animator postaci
    private bool isGrounded;               // Czy postać dotyka ziemi

    private bool wasLeftShiftLast = false; // Flaga, czy ostatni ruch był wykonany lewym Shiftem
    private float lastInputTime = 0f;      // Czas ostatniego poprawnego kliknięcia

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Pobierz Animator

        currentMoveSpeed = 0f;

        // Ustawienia fizyki
        rb.gravityScale = 3f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Sprawdzanie, czy postać dotyka ziemi
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Obsługa zwiększania prędkości
        if (Input.GetKeyDown(KeyCode.LeftShift) && !wasLeftShiftLast)
        {
            wasLeftShiftLast = true;
            IncreaseSpeed();
        }

        if (Input.GetKeyDown(KeyCode.RightShift) && wasLeftShiftLast)
        {
            wasLeftShiftLast = false;
            IncreaseSpeed();
        }

        // Zmniejsz prędkość, jeśli gracz nie jest aktywny
        if (Time.time - lastInputTime > decayDelay)
        {
            DecaySpeed();
        }

        // Skok
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Przekazanie prędkości do Animatora (do sterowania animacjami idle/run)
        if (animator != null)
        {
            animator.SetFloat("Speed", currentMoveSpeed);
        }
    }

    void FixedUpdate()
    {
        // Ruch w osi X tylko jeśli prędkość > 0
        if (currentMoveSpeed > 0f)
        {
            Vector2 newVelocity = new Vector2(currentMoveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = newVelocity;
        }
        else
        {
            // Jeśli prędkość = 0, zatrzymaj postać
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    // Zwiększ prędkość ruchu
    void IncreaseSpeed()
    {
        currentMoveSpeed = Mathf.Min(currentMoveSpeed + speedIncreaseRate, maxMoveSpeed);
        lastInputTime = Time.time;
    }

    // Zmniejsz prędkość ruchu przy braku aktywności
    void DecaySpeed()
    {
        currentMoveSpeed = Mathf.Max(currentMoveSpeed - speedDecayRate * Time.deltaTime, 0f);
    }

    // Skok
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // Wizualizacja zasięgu sprawdzania ziemi w edytorze
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
