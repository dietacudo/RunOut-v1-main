using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "MainMenu"; 

    void Update()
    {
        if (Input.anyKeyDown) 
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }
}
