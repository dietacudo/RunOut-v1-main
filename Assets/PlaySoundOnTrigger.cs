using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public AudioSource audioSource; // przypisz w Inspectorze

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) // sprawdza, czy to gracz
        {
            audioSource.Play();
        }
    }

    // Jeśli używasz 3D:
    // private void OnTriggerEnter(Collider other)
    // {
    //     if(other.CompareTag("Player"))
    //     {
    //         audioSource.Play();
    //     }
    // }
}
