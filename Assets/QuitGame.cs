using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Wyjście z gry...");

        // wyjdź z gry (działa tylko w buildzie)
        Application.Quit();

        // jeśli chcesz testować w editorze:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
