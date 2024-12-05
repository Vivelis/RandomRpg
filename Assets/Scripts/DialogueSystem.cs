using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject panel;
    public string pnjName;
    public PNJInteractionWithCharacterController playerController;

    [System.Serializable]
    public class QuestDialogue
    {
        public int questState;
        public List<string> dialogues;
    }

    public List<QuestDialogue> questDialogues = new List<QuestDialogue>();

    private int currentDialogueIndex = 0;
    private bool isDialogueActive = false;
    private int dialogueState;
    private bool actionRequired = false;

    public void UpdateStatus(PNJStatus status)
    {
        dialogueState = status.dialogueState;
        actionRequired = status.action;
        gameObject.SetActive(status.active);
    }

    public void StartDialogue()
    {
        if (dialogueState >= 0 && dialogueState < questDialogues.Count)
        {
            var dialogues = questDialogues[dialogueState].dialogues;

            if (dialogues.Count > 0)
            {
                panel.SetActive(true);
                isDialogueActive = true;
                currentDialogueIndex = 0;
                ShowDialogue(dialogues);
            }
        }
        else
        {
            Debug.LogWarning($"PNJ {pnjName}: DialogueState {dialogueState} est hors des limites.");
        }
    }

    public void ContinueDialogue()
    {
        if (isDialogueActive && dialogueState >= 0 && dialogueState < questDialogues.Count)
        {
            var dialogues = questDialogues[dialogueState].dialogues;

            currentDialogueIndex++;
            if (currentDialogueIndex < dialogues.Count)
            {
                ShowDialogue(dialogues);
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void ShowDialogue(List<string> dialogues)
    {
        dialogueText.text = dialogues[currentDialogueIndex];
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueText.text = "";
        panel.SetActive(false);

        if (playerController != null)
        {
            playerController.EndDialogue();
        }

        if (actionRequired)
        {
            QuestManager.instance.AdvanceQuestState();
        }
    }
}