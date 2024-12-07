using UnityEngine;

public class BallChasePlayer : MonoBehaviour
{
    public Transform player; // Transform gracza, za którym kula będzie podążać
    public float speed = 5f; // Prędkość kuli

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ignorowanie kolizji między kulą a przeszkodami na poziomie warstw
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Obstacles"), true);
    }

    private void Update()
    {
        if (player != null)
        {
            // Oblicz kierunek w stronę gracza
            Vector2 direction = (player.position - transform.position).normalized;

            // Przesuwaj kulę w kierunku gracza
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignoruj kolizje z przeszkodami - ta logika nie jest już konieczna, bo robimy to w warstwach
        // Ale dla bezpieczeństwa, poniższa kontrola zostaje
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
