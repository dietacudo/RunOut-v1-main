using UnityEngine;
using UnityEngine.SceneManagement; // Potrzebne do zmiany scen

public class LevelEnd : MonoBehaviour
{
    // Przypisz obiekt przycisku, jeśli chcesz
    public GameObject endLevelPanel; // Panel końcowy
    public string nextSceneName = "MainMenu"; // Nazwa sceny, na którą chcemy wrócić

    void Start()
    {
        // Na początku ukrywamy panel końcowy
        endLevelPanel.SetActive(false);
    }

    // Ta funkcja będzie wywołana, gdy poziom zostanie ukończony
    public void EndLevel()
    {
        // Wyświetlenie panelu końcowego
        endLevelPanel.SetActive(true);
    }

    // Funkcja wywoływana po kliknięciu przycisku
    public void GoToMainMenu()
    {
        // Ładowanie sceny głównej
        SceneManager.LoadScene(nextSceneName);
    }
}
