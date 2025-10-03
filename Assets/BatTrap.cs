using UnityEngine;

public class BatTrap : MonoBehaviour
{
    public float speed = 5f;       // prędkość lotu
    private bool isTriggered = false;
    
    private void Update()
    {
        if (isTriggered)
        {
            // Lecimy w lewo wzdłuż osi X
           transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);

        }
    }

    public void Activate()
    {
        isTriggered = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Gracz trafiony przez pułapkę!");
            // np. wywołaj tu śmierć gracza
        }
    }
}
