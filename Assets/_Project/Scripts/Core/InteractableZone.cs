using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteractableZone : MonoBehaviour
{
    [SerializeField] string promptText = "Press E to interact";
    [SerializeField] string targetScene;   // if set, loads this scene on interact
    [SerializeField] string interactMessage;     // flavor text shown on E press when targetScene is empty
    [SerializeField] float messageDuration = 4f; // seconds before interactMessage auto-hides
    [SerializeField] UnityEvent onInteract; // used when targetScene is empty

    bool playerInside;

    void Update()
    {
        if (!playerInside) return;
        if (!Keyboard.current.eKey.wasPressedThisFrame) return;

        if (!string.IsNullOrEmpty(targetScene))
        {
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            if (!string.IsNullOrEmpty(interactMessage))
                InteractionPromptUI.ShowMessage(interactMessage, messageDuration);
            onInteract.Invoke();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
        InteractionPromptUI.Show(promptText);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        InteractionPromptUI.Hide();
    }
}
