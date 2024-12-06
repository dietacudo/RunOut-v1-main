using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    private Vector3 respawnPosition; // Pozycja respawnu gracza
    private bool isDead = false;    // Czy gracz jest martwy?

    private GameObject player;      // Odwo³anie do gracza
    private PlayerControllerV2 playerController; // Odwo³anie do skryptu gracza

    void Start()
    {
        // ZnajdŸ gracza w scenie i zapamiêtaj jego pozycjê pocz¹tkow¹
        player = GameObject.FindWithTag("Player"); // Upewnij siê, ¿e gracz ma tag "Player"
        if (player != null)
        {
            respawnPosition = player.transform.position;
            playerController = player.GetComponent<PlayerControllerV2>();
        }
        else
        {
            Debug.LogError("Player object not found! Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        // SprawdŸ, czy gracz jest martwy i nacisn¹³ spacjê, aby siê odrodziæ
        if (isDead && Input.GetKeyDown(KeyCode.Space))
        {
            RespawnPlayer();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // SprawdŸ, czy obiekt, który wszed³ w kontakt, to gracz
        if (other.gameObject == player && !isDead)
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        isDead = true;
        if (playerController != null)
        {
            playerController.enabled = false; // Wy³¹cz kontrolê gracza
        }

        player.SetActive(false); // Ukryj gracza
        Debug.Log("Player is dead. Press SPACE to respawn.");
    }

    void RespawnPlayer()
    {
        isDead = false;
        player.transform.position = respawnPosition; // Przenieœ gracza do miejsca respawnu
        player.SetActive(true); // W³¹cz gracza

        if (playerController != null)
        {
            playerController.enabled = true; // W³¹cz kontrolê gracza
        }

        Debug.Log("Player has respawned.");
    }
}