using UnityEngine;

public class BallAudioZone : MonoBehaviour
{
    public Transform player;      // referencja do gracza
    public AudioSource ballAudio; // AudioSource kuli
    public float maxDistance = 5f; // promień strefy dźwięku

    void Update()
    {
        if (player == null || ballAudio == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if(distance <= maxDistance)
        {
            if(!ballAudio.isPlaying)
                ballAudio.Play(); // start odtwarzania

            // głośność rośnie przy zbliżeniu
            ballAudio.volume = 1 - (distance / maxDistance);
        }
        else
        {
            if(ballAudio.isPlaying)
                ballAudio.Stop(); // zatrzymanie po wyjściu z zasięgu
        }
    }
}
