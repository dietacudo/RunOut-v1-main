using UnityEngine;
using UnityEngine.SceneManagement;

public class LVLend : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // Nazwa sceny, do której przechodzimy

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
