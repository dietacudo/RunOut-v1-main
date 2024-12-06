using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    public float moveSpeed = 5f;        // Początkowa prędkość ruchu
    public float maxMoveSpeed = 15f;   // Maksymalna prędkość ruchu
    public float speedIncreaseRate = 0.5f;  // Ilość zwiększania prędkości na krok
    public float speedDecayRate = 1f;  // Szybkość zmniejszania prędkości przy braku aktywności

    public float jumpForce = 10f;      // Siła skoku
    public Transform groundCheck;     // Punkt sprawdzający kontakt z ziemią
    public float groundCheckRadius = 0.2f;  // Promień sprawdzania ziemi
    public LayerMask groundLayer;     // Warstwa ziemi

    private float currentMoveSpeed = 0f; // <--- Ta linia musi być tutaj
    public float CurrentMoveSpeed => currentMoveSpeed; // Właściwość tylko do odczytu

    private Rigidbody2D rb;
    private bool isGrounded;          // Czy postać dotyka ziemi

    private bool wasLeftShiftLast = false; // Flaga, czy ostatni ruch był wykonany lewym Shiftem
    private float lastInputTime = 0f;      // Czas ostatniego poprawnego kliknięcia
    public float decayDelay = 1f;          // Czas bezczynności przed zmniejszaniem prędkości

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMoveSpeed = 0f; // Na początku postać jest nieruchoma
    }

    void Update()
    {
        // Sprawdzanie, czy postać dotyka ziemi
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Sprawdź naciśnięcie lewego Shifta
        if (Input.GetKeyDown(KeyCode.LeftShift) && !wasLeftShiftLast)
        {
            wasLeftShiftLast = true; // Lewy Shift jako ostatni
            IncreaseSpeed();         // Zwiększ prędkość
        }

        // Sprawdź naciśnięcie prawego Shifta
        if (Input.GetKeyDown(KeyCode.RightShift) && wasLeftShiftLast)
        {
            wasLeftShiftLast = false; // Prawy Shift jako ostatni
            IncreaseSpeed();          // Zwiększ prędkość
        }

        // Zmniejsz prędkość, jeśli gracz nie jest aktywny
        if (Time.time - lastInputTime > decayDelay)
        {
            DecaySpeed();
        }

        // Obsługa skoku (możliwość skoku podczas poruszania się)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Aktualizuj pozycję postaci w zależności od aktualnej prędkości
        if (currentMoveSpeed > 0f)
        {
            Vector2 newVelocity = new Vector2(currentMoveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = newVelocity; // Zachowaj pionowy ruch (np. skakanie)
        }
    }

    // Zwiększ prędkość ruchu
    void IncreaseSpeed()
    {
        currentMoveSpeed = Mathf.Min(currentMoveSpeed + speedIncreaseRate, maxMoveSpeed);
        lastInputTime = Time.time; // Zaktualizuj czas ostatniego kliknięcia
    }

    // Zmniejsz prędkość ruchu przy braku aktywności
    void DecaySpeed()
    {
        currentMoveSpeed = Mathf.Max(currentMoveSpeed - speedDecayRate * Time.deltaTime, 0f);
    }

    // Funkcja odpowiedzialna za skok
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Dodaj siłę w osi Y, zachowując aktualną prędkość w osi X
    }

    // Rysowanie debugowego okręgu do wizualizacji sprawdzania ziemi
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}