using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Texte affiché à l'écran
    public GameObject panel; // Référence au panneau de dialogue
    public string[] dialogues; // Liste des dialogues du PNJ
    private int currentDialogueIndex = 0; // Index du dialogue actuel
    private bool isDialogueActive = false; // Indique si un dialogue est en cours
    public PNJInteractionWithCharacterController playerController; // Référence au joueur

    public void StartDialogue()
    {
        panel.SetActive(true); // Active le panneau de dialogue
        if (dialogues.Length > 0)
        {
            isDialogueActive = true;
            currentDialogueIndex = 0;
            ShowDialogue();
        }
    }

    public void ContinueDialogue()
    {
        if (isDialogueActive)
        {
            currentDialogueIndex++;
            if (currentDialogueIndex < dialogues.Length)
            {
                ShowDialogue();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void ShowDialogue()
    {
        dialogueText.text = dialogues[currentDialogueIndex];
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueText.text = "";

        HealerNPC healer = FindObjectOfType<HealerNPC>();
        if (healer != null)
        {
            healer.HealPlayers();
        }
        if (playerController != null)
        {
            panel.SetActive(false);
            playerController.EndDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PNJInteractionWithCharacterController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = null;
        }
    }
}