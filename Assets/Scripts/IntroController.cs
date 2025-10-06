using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "MainMenu"; 
    [SerializeField] private Image fadeImage; // czarny obraz na pełny ekran
    [SerializeField] private float fadeDuration = 1f; // czas trwania efektu

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

        float t = 0f;
        Color color = fadeImage.color;

        // stopniowe przyciemnianie
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(menuSceneName);
    }
}
