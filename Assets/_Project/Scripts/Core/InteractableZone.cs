using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteractableZone : MonoBehaviour
{
    [SerializeField] string promptText = "Press E to interact";
    [SerializeField] string targetScene;   // if set, loads this scene on interact
    [SerializeField] UnityEvent onInteract; // used when targetScene is empty

    bool playerInside;

    void Update()
    {
        if (!playerInside) return;
        if (!Keyboard.current.eKey.wasPressedThisFrame) return;

        if (!string.IsNullOrEmpty(targetScene))
            SceneManager.LoadScene(targetScene);
        else
            onInteract.Invoke();
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
