using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Tooltip("Nazwa(y) scen, w których MUZYKA MA GRAĆ (np. MainMenu, Options)")]
    public string[] playInScenes = new string[] { "MainMenu", "Options" };

    [Tooltip("Czas fade (s) przy włączaniu/wyłączaniu muzyki")]
    public float fadeDuration = 0.8f;

    [Tooltip("Jeśli true — obiekt muzyki zostanie zniszczony po wejściu do sceny gry")]
    public bool destroyWhenStopped = true;

    [Tooltip("Maksymalna głośność muzyki (0..1) używana zamiast '1.0'")]
    public float maxVolume = 0.02f; // ustaw tutaj 0.02 w Inspectorze

    AudioSource src;

    void Awake()
    {
        // singleton (nie dublujemy dźwięku)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        src = GetComponent<AudioSource>();

        // rekomendowane ustawienia AudioSource w Inspectorze:
        // loop = true; playOnAwake = false; spatialBlend = 0;
        src.playOnAwake = false;
        src.loop = true;
    }

    void Start()
    {
        string cur = SceneManager.GetActiveScene().name;
        if (IsPlayScene(cur))
        {
            src.volume = maxVolume;
            if (!src.isPlaying) src.Play();
            // jeżeli chcesz fade-in na start, użyj StartCoroutine(FadeTo(maxVolume, fadeDuration));
        }
        else
        {
            src.volume = 0f;
            // nie odtwarzamy jeśli nie w playInScenes
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsPlayScene(scene.name))
        {
            // włącz muzykę (fade in do maxVolume)
            if (!src.isPlaying) src.Play();
            StopAllCoroutines();
            StartCoroutine(FadeTo(maxVolume, fadeDuration));
        }
        else
        {
            // fade out do 0, potem stop (i opcjonalnie destroy)
            StopAllCoroutines();
            StartCoroutine(FadeOutAndMaybeDestroy(fadeDuration));
        }
    }

    bool IsPlayScene(string sceneName)
    {
        return playInScenes != null && playInScenes.Contains(sceneName);
    }

    IEnumerator FadeTo(float targetVolume, float duration)
    {
        float start = src.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            src.volume = Mathf.Lerp(start, targetVolume, t / duration);
            yield return null;
        }
        src.volume = targetVolume;
    }

    IEnumerator FadeOutAndMaybeDestroy(float duration)
    {
        yield return FadeTo(0f, duration);
        src.Stop();
        if (destroyWhenStopped) Destroy(gameObject);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
