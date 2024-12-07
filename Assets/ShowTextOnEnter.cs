using UnityEngine;
using TMPro;  // Użycie TextMesh Pro

public class ShowTextOnEnter : MonoBehaviour
{
    // Tekst, który ma się pojawić nad przeszkodą
    public TextMeshPro displayText;

    // Pozycja tekstu nad przeszkodą
    public Transform textPosition;

    // Tekst, który ma być wyświetlony
    public string message = "Press SPACE to jump!";

    // Flaga, która kontroluje, czy tekst jest widoczny
    private bool textVisible = false;

    private void Start()
    {
        // Początkowo ukrywamy tekst
        displayText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzenie, czy obiekt to gracz
        if (other.CompareTag("Player"))
        {
            // Ustawienie tekstu
            displayText.text = message;

            // Przemieszczenie tekstu nad przeszkodą
            displayText.transform.position = textPosition.position;

            // Wyświetlenie tekstu
            displayText.gameObject.SetActive(true);

            // Flaga, że tekst jest widoczny
            textVisible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Ukrycie tekstu, gdy gracz opuści obszar collidera
        if (other.CompareTag("Player") && textVisible)
        {
            displayText.gameObject.SetActive(false);
            textVisible = false;
        }
    }
}
