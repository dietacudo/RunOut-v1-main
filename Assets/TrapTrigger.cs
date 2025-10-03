using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public BatTrap trap; // przypisz w Inspectorze konkretną pułapkę (np. Nietoperza albo Strzałę)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            trap.Activate();
        }
    }
}
