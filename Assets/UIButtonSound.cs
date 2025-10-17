using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioSource hoverSound; // dźwięk przy najechaniu
    public AudioClip clickClip;    // dźwięk kliknięcia
    public string sceneToLoad;     // nazwa sceny do załadowania (opcjonalnie)

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
            hoverSound.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickClip != null)
            PlayClickSound(clickClip);

        if (!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
    }

    // Tworzy tymczasowy AudioSource do odtworzenia dźwięku
    private void PlayClickSound(AudioClip clip)
    {
        GameObject tempGO = new GameObject("TempAudio"); // tymczasowy obiekt
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.Play();
        Destroy(tempGO, clip.length); // usuń po odtworzeniu
    }
}
