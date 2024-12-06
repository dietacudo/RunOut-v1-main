using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Prędkość poruszania się
    public float sprintSpeed = 10f; // Prędkość sprintu
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private bool isGrounded;

    private float originalHeight; // Oryginalna wysokość kapsuły
    public float crouchHeight = 0.5f; // Wysokość przy kucaniu

    public float slideSpeed = 10f; // Prędkość podczas wślizgu
    public float slideDuration = 1f; // Czas trwania wślizgu w sekundach
    private float slideTimer = 0f; // Timer do odliczania czasu wślizgu
    private bool isSliding = false; // Czy gracz jest aktualnie w wślizgu

    public float slideCooldown = 2f; // Czas oczekiwania na kolejny wślizg
    private float slideCooldownTimer = 0f; // Timer cooldownu na wślizg

    private float lastMoveDirection = 1f; // Postać zawsze porusza się w prawo

    public float maxJumpHeight = 5f; // Maksymalna wysokość skoku
    public float gravityScale = 3f; // Wartość skali grawitacji (im wyższa, tym szybciej postać opada)

    // Nowe zmienne do detekcji ściany
    public float wallCheckDistance = 0.7f; // Odległość do wykrywania ściany
    public LayerMask wallLayer; // Warstwa, która będzie traktowana jako ściana
    private bool isTouchingWall; // Flaga, która sprawdza, czy gracz dotyka ściany
    private bool canClimb; // Flaga, czy gracz może wspinać się po ścianie

    // Zmienne do obsługi przemiennego przyspieszania
    private bool isLeftShiftPressed = false;
    private bool isRightShiftPressed = false;
    private float currentSpeed; // Bieżąca prędkość

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        originalHeight = capsuleCollider.size.y;

        // Ustawienie skali grawitacji
        rb.gravityScale = gravityScale;
        currentSpeed = moveSpeed; // Ustawienie początkowej prędkości
    }

    void Update()
    {
        // Sprawdzanie, czy postać dotyka ziemi
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Sprawdzanie, czy gracz dotyka ściany
        isTouchingWall = Physics2D.Raycast(transform.position, new Vector2(Mathf.Sign(rb.linearVelocity.x), 0), wallCheckDistance, wallLayer);

        // Sprawdzanie, czy gracz może wspinać się po ścianie
        canClimb = isTouchingWall && !isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f;

        // Zmienne do obsługi przyspieszania
        isLeftShiftPressed = Input.GetKey(KeyCode.LeftShift);
        isRightShiftPressed = Input.GetKey(KeyCode.RightShift);

        // Przemienne przyspieszanie
        if (isLeftShiftPressed && !isRightShiftPressed)
        {
            currentSpeed = sprintSpeed; // Przyspieszenie podczas trzymania lewego shift
        }
        else if (isRightShiftPressed && !isLeftShiftPressed)
        {
            currentSpeed = sprintSpeed; // Przyspieszenie podczas trzymania prawego shift
        }
        else
        {
            currentSpeed = moveSpeed; // Normalna prędkość
        }

        // Ruch w prawo (postać porusza się tylko jeśli naciśnięty jest klawisz)
        if (isSliding)
        {
            rb.linearVelocity = new Vector2(lastMoveDirection * slideSpeed, rb.linearVelocity.y); // Poruszaj się w ostatnim kierunku
        }
        else if (canClimb)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Zatrzymaj ruch poziomy

            // Wspinaj się w górę, jeśli trzymasz przycisk
            if (Input.GetKey(KeyCode.W)) // Trzymaj W, aby wspinać się
            {
                rb.linearVelocity = new Vector2(0, moveSpeed); // Ruch w górę po ścianie
            }
            else
            {
                rb.linearVelocity = new Vector2(0, 0); // Zatrzymanie w pionie, jeśli nie trzymasz W
            }
        }
        else
        {
            // Sprawdzamy, czy gracz naciśnie klawisze w lewo/prawo (domyślnie zero w osi X)
            float moveInput = Input.GetAxis("Horizontal"); // Odczytujemy wartość z klawiszy A/D lub strzałek
            rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y); // Ustawiamy ruch na podstawie wejścia
        }

        // Skakanie (kontrola nad prędkością skoku)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Inicjalizuj skok
        }

        // Zapobiegaj "skakaniu po księżycu" – jeśli prędkość w osi Y jest zbyt duża, ogranicz ją
        if (rb.linearVelocity.y > maxJumpHeight)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxJumpHeight); // Ogranicz prędkość skoku
        }

        // Kucanie i wślizg
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Input.GetKey(KeyCode.LeftShift) && isGrounded && slideCooldownTimer <= 0) // Sprawdzamy, czy gracz sprintuje (trzyma shift)
            {
                StartSlide();
            }
            else
            {
                Crouch();
            }
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            if (!isSliding) // Kończy kucanie tylko, jeśli nie trwa wślizg
            {
                StandUp();
            }
        }

        // Zarządzanie wślizgiem
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                EndSlide();
            }
        }

        // Zarządzanie cooldownem
        if (slideCooldownTimer > 0)
        {
            slideCooldownTimer -= Time.deltaTime;
        }
    }

    void Crouch()
    {
        // Zmniejsz wysokość collidera przy kucaniu
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, crouchHeight);
    }

    void StandUp()
    {
        // Przywróć oryginalną wysokość collidera
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, originalHeight);
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        slideCooldownTimer = slideCooldown; // Ustaw timer cooldownu na czas oczekiwania
        capsuleCollider.size = new Vector2(capsuleCollider.size.x, crouchHeight); // Kucnij podczas wślizgu
    }

    void EndSlide()
    {
        isSliding = false;
        StandUp(); // Przywróć oryginalny rozmiar po zakończeniu wślizgu
    }
}
