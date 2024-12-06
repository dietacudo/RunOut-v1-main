using UnityEngine;
using UnityEngine.SceneManagement;

public class BallReset : MonoBehaviour
{
    // Funkcja wywoływana, gdy kula dotknie obiektu z tagiem "Player"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sprawdzenie, czy obiekt to gracz
        if (collision.gameObject.CompareTag("Player"))
        {
            // Resetowanie sceny
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
