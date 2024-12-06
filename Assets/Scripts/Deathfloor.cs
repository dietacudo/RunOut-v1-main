using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    private Vector3 respawnPosition; // Pozycja respawnu gracza
    private bool isDead = false;    // Czy gracz jest martwy?

    private GameObject player;      // Odwo�anie do gracza
    private PlayerControllerV2 playerController; // Odwo�anie do skryptu gracza

    void Start()
    {
        // Znajd� gracza w scenie i zapami�taj jego pozycj� pocz�tkow�
        player = GameObject.FindWithTag("Player"); // Upewnij si�, �e gracz ma tag "Player"
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
        // Sprawd�, czy gracz jest martwy i nacisn�� spacj�, aby si� odrodzi�
        if (isDead && Input.GetKeyDown(KeyCode.Space))
        {
            RespawnPlayer();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawd�, czy obiekt, kt�ry wszed� w kontakt, to gracz
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
            playerController.enabled = false; // Wy��cz kontrol� gracza
        }

        player.SetActive(false); // Ukryj gracza
        Debug.Log("Player is dead. Press SPACE to respawn.");
    }

    void RespawnPlayer()
    {
        isDead = false;
        player.transform.position = respawnPosition; // Przenie� gracza do miejsca respawnu
        player.SetActive(true); // W��cz gracza

        if (playerController != null)
        {
            playerController.enabled = true; // W��cz kontrol� gracza
        }

        Debug.Log("Player has respawned.");
    }
}