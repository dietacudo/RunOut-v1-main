using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class BallReset : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI deathMessageText;
    public TextMeshProUGUI deathCounterText;

    [Header("Audio")]
    public AudioSource ballAudio;      // AudioSource kuli, który zatrzymujemy przy śmierci
    public AudioSource audioSource;    // AudioSource do odtwarzania dźwięku śmierci
    public AudioClip deathClip;        // Clip dźwięku śmierci gracza

    [Header("Player settings")]
    public string playerTag = "Player"; // tag gracza (domyślnie "Player")
    public string playerControllerTypeName = "PlayerController"; // nazwa klasy skryptu ruchu, który można wyłączyć (opcjonalnie)

    [Header("Death message timing")]
    [Range(0f, 1f)]
    public float deathMessageShowPercent = 0.1f; // pokaż komunikat po X% długości animacji (0-1). Domyślnie 0.7 => 70%

    private bool gameOver = false;
    private static int deathCount = 0; // licznik śmierci, nie resetuje się przy zmianie sceny

    private void Start()
    {
        UpdateDeathCounterUI();
        if (deathMessageText != null) deathMessageText.text = "";
        ReactionLock.Unlock(); // upewnij się, że reakcje są odblokowane na starcie
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(playerTag)) return;

        GameObject player = collision.gameObject;
        Animator playerAnim = player.GetComponent<Animator>();

        // 1) Ustaw Animator tak, żeby działał w unscaled time (animacja zadziała mimo Time.timeScale = 0)
        if (playerAnim != null)
            playerAnim.updateMode = AnimatorUpdateMode.UnscaledTime;

        // 2) Wywołaj trigger "Death"
        if (playerAnim != null)
        {
            if (HasAnimatorParameter(playerAnim, "Death"))
                playerAnim.SetTrigger("Death");
            else
                Debug.LogWarning("Animator nie ma parametru Trigger o nazwie 'Death'. Dodaj go w Animatorze.");
        }

        // 3) Wyłącz kontrolery ruchu (jeśli istnieją), aby gracz nie mógł się poruszać
        var comp = player.GetComponent(playerControllerTypeName);
        if (comp != null)
        {
            if (comp is MonoBehaviour mb) mb.enabled = false;
        }
        else
        {
            var pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = false;
            var pcv2 = player.GetComponent<PlayerControllerV2>();
            if (pcv2 != null) pcv2.enabled = false;
        }

        // 4) Zatrzymaj dźwięk kuli i zagraj dźwięk śmierci
        if (ballAudio != null) ballAudio.Stop();
        if (audioSource != null && deathClip != null) audioSource.PlayOneShot(deathClip);

        // 5) Zwiększ licznik śmierci i zaktualizuj UI
        deathCount++;
        UpdateDeathCounterUI();

        // 6) Zablokuj reakcje (ReactionLock) aż do respawnu
        ReactionLock.Lock();

        // 7) Pauzuj grę (fizyka, Update() itp.). Animator i coroutine czekające w real time będą działać.
        Time.timeScale = 0f;

        // 8) Odczytaj długość clipu śmierci (jeśli dostępny) i poczekaj w czasie rzeczywistym, potem pokaż UI
        float deathClipLength = 1f;
        if (playerAnim != null)
        {
            deathClipLength = GetClipLength(playerAnim, "death"); // znajdź clip zawierający "death" w nazwie
            if (deathClipLength <= 0f) deathClipLength = 1f;
        }

        // zastosuj procent (np. 0.7 => 70% długości animacji)
        float waitTime = Mathf.Clamp01(deathMessageShowPercent) * deathClipLength;
        if (waitTime <= 0f) waitTime = 0.1f; // minimalne opóźnienie, by komunikat nie wchodził natychmiast

        StartCoroutine(WaitAndShowDeathUI(waitTime, playerAnim));
        gameOver = true;
    }

    private IEnumerator WaitAndShowDeathUI(float realSecondsToWait, Animator playerAnim)
    {
        // czekaj w czasie rzeczywistym (niezależnie od Time.timeScale)
        yield return new WaitForSecondsRealtime(realSecondsToWait);

        // pokaż komunikat o śmierci dopiero PO odczekanym czasie (część animacji może dalej lecieć)
        if (deathMessageText != null)
            deathMessageText.text = "You Died! Press SPACE to Retry";

        // ustaw flagę, by Update mógł obsłużyć wciśnięcie spacji
        gameOver = true;

        // pozostaw animator w UnscaledTime (scene reload i tak zresetuje go)
        if (playerAnim != null)
            playerAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            // Odblokuj reakcje przed restartem
            ReactionLock.Unlock();

            // Przywróć czas i zrestartuj scenę
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            gameOver = false;
        }
    }

    private void UpdateDeathCounterUI()
    {
        if (deathCounterText != null)
            deathCounterText.text = "Deaths: " + deathCount;
    }

    public void ResetDeathCounter()
    {
        deathCount = 0;
        UpdateDeathCounterUI();
    }

    // Znajduje długość clipu w AnimatorController, którego nazwa zawiera clipNamePart (np. "death")
    private float GetClipLength(Animator animator, string clipNamePart)
    {
        if (animator == null || animator.runtimeAnimatorController == null) return 1f;
        var clips = animator.runtimeAnimatorController.animationClips;
        foreach (var c in clips)
        {
            if (c == null) continue;
            if (c.name.ToLower().Contains(clipNamePart.ToLower()))
            {
                return c.length;
            }
        }
        // fallback: spróbuj pobrać długość aktualnego stanu
        try
        {
            var info = animator.GetCurrentAnimatorStateInfo(0);
            return info.length > 0f ? info.length : 1f;
        }
        catch
        {
            return 1f;
        }
    }

    private bool HasAnimatorParameter(Animator animator, string paramName)
    {
        if (animator == null) return false;
        foreach (var p in animator.parameters)
            if (p.name == paramName) return true;
        return false;
    }
}
