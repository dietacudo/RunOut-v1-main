using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textToBlink;
    [SerializeField] private float blinkSpeed = 0.5f; // czas między pojawianiem a znikaniem

    private void Start()
    {
        if (textToBlink == null)
        {
            textToBlink = GetComponent<TextMeshProUGUI>();
        }
        StartCoroutine(Blink());
    }

    private System.Collections.IEnumerator Blink()
    {
        while (true)
        {
            textToBlink.enabled = !textToBlink.enabled; // włącza/wyłącza widoczność tekstu
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}
