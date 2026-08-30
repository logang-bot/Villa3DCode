using System.Collections;
using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    static InteractionPromptUI instance;

    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI label;

    Coroutine hideRoutine;

    void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    public static void Show(string text)
    {
        if (instance == null) return;
        instance.CancelAutoHide();
        instance.label.text = text;
        instance.panel.SetActive(true);
    }

    public static void Hide()
    {
        if (instance == null) return;
        instance.CancelAutoHide();
        instance.panel.SetActive(false);
    }

    public static void ShowMessage(string text, float duration)
    {
        if (instance == null) return;
        instance.CancelAutoHide();
        instance.label.text = text;
        instance.panel.SetActive(true);
        instance.hideRoutine = instance.StartCoroutine(instance.AutoHide(duration));
    }

    void CancelAutoHide()
    {
        if (hideRoutine == null) return;
        StopCoroutine(hideRoutine);
        hideRoutine = null;
    }

    IEnumerator AutoHide(float duration)
    {
        yield return new WaitForSeconds(duration);
        panel.SetActive(false);
        hideRoutine = null;
    }
}
