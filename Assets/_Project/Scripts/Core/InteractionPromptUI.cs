using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    static InteractionPromptUI instance;

    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI label;

    void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    public static void Show(string text)
    {
        if (instance == null) return;
        instance.label.text = text;
        instance.panel.SetActive(true);
    }

    public static void Hide()
    {
        if (instance == null) return;
        instance.panel.SetActive(false);
    }
}
