using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour
{
    [Header("Scena docelowa")]
    [SerializeField] private string menuSceneName = "MainMenu";

    [Header("Efekt przejścia")]
    [SerializeField] private Image fadeImage; 
    [SerializeField] private float fadeDuration = 1f;

    [Header("Dźwięk")]
    [SerializeField] private AudioClip clickClip; // dźwięk przy naciśnięciu spacji
    [SerializeField] private float delayBeforeLoad = 0.3f; // czas na zagranie dźwięku

    private bool isFading = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isFading)
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        isFading = true;

        // 🔊 Odtworzenie dźwięku
        if (clickClip != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
        }

        // 🔥 Czekamy chwilę, żeby dźwięk był słyszalny zanim zacznie się fade
        yield return new WaitForSeconds(delayBeforeLoad);

        float t = 0f;
        Color color = fadeImage.color;

        // 🎬 Stopniowe przyciemnianie ekranu
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // 🔁 Po zakończeniu fade'a – zmiana sceny
        SceneManager.LoadScene(menuSceneName);
    }
}
