using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform; // Transform kamery
    public float parallaxSpeed = 0.5f; // Szybkość warstwy
    private Vector3 lastCameraPosition;

    private float textureUnitSizeX; // Szerokość tekstury (do przewijania)

    void Start()
    {
        lastCameraPosition = cameraTransform.position;

        // Pobierz rozmiar tekstury na podstawie SpriteRenderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        textureUnitSizeX = spriteRenderer.sprite.bounds.size.x * transform.localScale.x;
    }

    void Update()
    {
        // Oblicz przesunięcie kamery
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxSpeed, 0, 0);
        lastCameraPosition = cameraTransform.position;

        // Sprawdź, czy warstwa jest poza ekranem, i przesuń ją
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y, transform.position.z);
        }
    }
}
