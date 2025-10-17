using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioSource hoverSound; // dźwięk przy najechaniu
    public AudioClip clickClip;    // dźwięk kliknięcia
    public string sceneToLoad;     // nazwa sceny do załadowania (opcjonalnie)

    // Wywołuje się, gdy myszka wchodzi na przycisk
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(hoverSound != null)
            hoverSound.Play();
    }

    // Wywołuje się przy kliknięciu
    public void OnPointerClick(PointerEventData eventData)
    {
        if(clickClip != null)
            AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);

        // opcjonalna zmiana sceny
        if(!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
    }
}
