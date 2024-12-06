using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Wpisz nazwę swojej sceny
    }

    public void OpenOptions()
    {
        Debug.Log("Otwieranie opcji...");
    }

    public void OpenCredits()
    {
        Debug.Log("Twórcy gry...");
    }

    public void QuitGame()
    {
        Debug.Log("Wyjście z gry...");
        Application.Quit(); // Działa tylko w buildzie, nie w edytorze
    }
}
