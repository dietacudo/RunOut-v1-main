using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Odsłanie do gracza
    public float smoothSpeed = 0.125f; // Płynność ruchu kamery
    public Vector3 offset; // Opcjonalny offset dla kamery

    private float fixedY; // Stała wartość osi Y

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Gracz nie został przypisany do kamery!");
            return;
        }

        // Zapisz aktualną wysokość kamery (oś Y) jako stałą
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        // Podążaj za graczem tylko na osi X
        Vector3 desiredPosition = new Vector3(player.position.x + offset.x, fixedY + offset.y, transform.position.z + offset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
