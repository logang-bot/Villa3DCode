using UnityEngine;

public class SneakObservationPoint : MonoBehaviour
{
    [SerializeField] AlertStateMachine target;
    [SerializeField] string clueId;
    [SerializeField] float requiredDuration = 4f;
    [SerializeField] string completionMessage = "You've gathered enough to be sure.";

    bool playerInside;
    bool completed;
    float progress;

    public float Progress01 => progress / requiredDuration;

    void Update()
    {
        if (!playerInside || completed) return;
        progress = target.CurrentState == AlertLevel.Alert
            ? 0f
            : Mathf.Min(requiredDuration, progress + Time.deltaTime);

        ShowProgress();
        if (progress >= requiredDuration) Complete();
    }

    void ShowProgress()
    {
        InteractionPromptUI.Show($"Observing... {Mathf.RoundToInt(Progress01 * 100f)}%");
    }

    void Complete()
    {
        completed = true;
        ClueTracker.Instance?.AddClue(clueId);
        InteractionPromptUI.ShowMessage(completionMessage, 3f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
        progress = 0f;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        if (!completed) InteractionPromptUI.Hide();
    }
}
