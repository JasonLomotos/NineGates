using UnityEngine;
using TMPro;

public class NpcController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject exitPromptUI;
    public GameObject talkPrompt; // NEW: The world-space prompt

    [Header("Dialogue Content")]
    [TextArea(3, 10)]
    public string[] dialogueLines;

    private int currentLineIndex = 0;
    private bool dialogueStarted = false;

    public bool IsDialogueActive => dialogueStarted;

    void Start()
    {
        // Ensure prompt is off at the start
        if (talkPrompt != null)
        {
            talkPrompt.SetActive(false);
        }
    }
    
    // NEW methods to control the prompt
    public void ShowTalkPrompt()
    {
        if (talkPrompt != null) talkPrompt.SetActive(true);
    }
    
    public void HideTalkPrompt()
    {
        if (talkPrompt != null) talkPrompt.SetActive(false);
    }

    public void StartDialogue()
    {
        dialogueStarted = true;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        if (exitPromptUI != null) exitPromptUI.SetActive(true);
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
            currentLineIndex++;
        }
    }
    
    public void EndDialogue()
    {
        dialogueStarted = false;
        dialoguePanel.SetActive(false);
        if (exitPromptUI != null) exitPromptUI.SetActive(false);
    }
}