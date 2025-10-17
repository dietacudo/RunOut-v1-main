using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public AudioClip clickClip; // dźwięk kliknięcia

    public void LoadScene(string sceneName)
    {
        if(clickClip != null)
            StartCoroutine(LoadSceneAfterClick(sceneName));
        else
            SceneManager.LoadScene(sceneName); // fallback jeśli brak dźwięku
    }

    private IEnumerator LoadSceneAfterClick(string sceneName)
    {
        // odtwórz dźwięk
        AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);

        // poczekaj chwilę, żeby dźwięk zdążył zagrać
        yield return new WaitForSeconds(0.2f);

        // zmiana sceny
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Gra zakończona (działa tylko w buildzie).");
    }
}
