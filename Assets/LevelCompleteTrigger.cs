using UnityEngine;

public class LevelCompleteTrigger : MonoBehaviour
{
    public LevelEnd levelEnd; // Referencja do skryptu LevelEnd

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Sprawdzenie, czy to gracz
        {
            levelEnd.EndLevel(); // Wywołanie zakończenia poziomu
        }
    }
}
