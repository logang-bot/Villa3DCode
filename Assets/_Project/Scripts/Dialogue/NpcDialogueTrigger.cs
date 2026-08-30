using UnityEngine;
using Yarn.Unity;

public class NpcDialogueTrigger : MonoBehaviour
{
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] string startNode;

    public void BeginDialogue()
    {
        if (dialogueRunner.IsDialogueRunning) return;
        dialogueRunner.StartDialogue(startNode);
    }
}
