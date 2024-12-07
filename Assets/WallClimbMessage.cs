using UnityEngine;
using TMPro; // Wymagane do obsługi TextMeshPro

public class WallClimbMessage : MonoBehaviour
{
    public TextMeshProUGUI climbMessage; // Referencja do tekstu UI
    private bool isTouchingWall = false; // Czy gracz dotyka ściany?

    void Start()
    {
        // Upewnij się, że tekst jest ukryty na początku
        if (climbMessage != null)
        {
            climbMessage.enabled = false;
        }
        else
        {
            Debug.LogError("Brak przypisanego obiektu TextMeshProUGUI w inspektorze!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sprawdzanie, czy obiekt, z którym gracz się zderzył, ma tag "Climbing Wall"
        if (collision.gameObject.CompareTag("Climbing Wall"))
        {
            isTouchingWall = true;

            // Wyświetl komunikat
            if (climbMessage != null)
            {
                climbMessage.enabled = true;
                climbMessage.text = "Press SPACE to climb!";
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Jeśli gracz przestaje dotykać ściany, ukryj komunikat
        if (collision.gameObject.CompareTag("Climbing Wall"))
        {
            isTouchingWall = false;

            if (climbMessage != null)
            {
                climbMessage.enabled = false;
            }
        }
    }
}
