using UnityEngine;
using TMPro;

public class SpeedDisplay : MonoBehaviour
{
    public TextMeshProUGUI speedText; // Odniesienie do UI Text
    public PlayerControllerV2 playerController; // Odniesienie do skryptu gracza

    void Update()
    {
        if (playerController != null && speedText != null)
        {
            // Pobierz aktualną prędkość gracza i zaktualizuj tekst
            float speed = playerController.CurrentMoveSpeed; // Zmieniono na currentMoveSpeed
            speedText.text = "Speed: " + speed.ToString("F2"); // Wyświetl z dwoma miejscami po przecinku
        }
    }
}
