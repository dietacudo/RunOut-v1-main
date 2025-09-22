using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Funkcja wywoływana przez przycisk
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Opcjonalnie – zakończenie gry
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Gra zakończona (działa tylko w buildzie).");
    }
}
