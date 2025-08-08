using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.F;

    private NpcController currentNpc;

    void Update()
    {
        if (currentNpc != null && Input.GetKeyDown(interactionKey))
        {
            if (currentNpc.IsDialogueActive)
            {
                currentNpc.EndDialogue();
                currentNpc.ShowTalkPrompt(); // Show the prompt again
            }
            else
            {
                currentNpc.HideTalkPrompt(); // Hide the prompt
                currentNpc.StartDialogue();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        NpcController npc = other.GetComponent<NpcController>();
        if (npc != null)
        {
            currentNpc = npc;
            if (!currentNpc.IsDialogueActive)
            {
                currentNpc.ShowTalkPrompt();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<NpcController>() == currentNpc)
        {
            if (currentNpc != null)
            {
                if (currentNpc.IsDialogueActive)
                {
                    currentNpc.EndDialogue();
                }
                currentNpc.HideTalkPrompt();
                currentNpc = null;
            }
        }
    }
}