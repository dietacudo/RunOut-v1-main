using UnityEngine;

public class Sandstorm : MonoBehaviour
{
    [Header("Ruch burzy")]
    public float speed = 2f;           // prędkość przesuwania
    public Vector2 direction = Vector2.right; // kierunek ruchu (domyślnie w prawo)

    [Header("Zachowanie")]
    public bool loop = false;          // czy ma wracać na początek po wyjściu z ekranu
    public float resetX = -10f;        // pozycja, na której się resetuje (jeśli loop = true)
    public float endX = 20f;           // granica końca drogi

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Jeśli burza ma się zapętlać
        if (loop && transform.position.x > endX)
        {
            Vector3 pos = transform.position;
            pos.x = resetX;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gracz został złapany przez burzę!"); 
            // tutaj możesz np. zabić gracza, wywołać ekran śmierci itd.
        }
    }
}
