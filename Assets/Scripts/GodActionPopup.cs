using UnityEngine;
using TMPro;
using System.Collections;

public class GodActionPopup : MonoBehaviour
{
    public static GodActionPopup Instance;

    public TextMeshProUGUI actionText;
    public float displayDuration = 3f;
    public CanvasGroup canvasGroup;
    private Coroutine currentCoroutine;

    void Awake()
    {
        Instance = this;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void ShowPopup(string message)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        actionText.text = message;
        currentCoroutine = StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0f;
        float elapsed = 0f;
        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / 0.3f);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(displayDuration);
        elapsed = 0f;
        while (elapsed < 0.5f) //Fade out popup
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (elapsed / 0.5f));
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}