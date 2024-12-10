using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleDialogueBox : MonoBehaviour
{
    TMP_Text dialogueText;
    string currentDialogue;
    public string currentRemainingDialogue;
    public List<string> currentDialogueList = new List<string>();
    int dialogueState;
    //0 = empty
    //1 = typing
    //2 = displaying
    float displayTime = 1f;
    float displayTimer = 0f;
    float letterCooldown = 0.04f;
    float letterTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        dialogueState = 0;
        dialogueText = GetComponent<TMP_Text>();
        dialogueText.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        dialogueText.text = currentDialogue;
        switch (dialogueState) {
            case 0:
                //waits for a dialogue
                if (currentDialogueList.Count > 0) {
                    NextDialogue();
                }
                break;
            case 1:
                if (currentRemainingDialogue.Length == 0) {
                    dialogueState = 2;
                    displayTimer = 0f;
                    letterTimer = 0f;
                }

                letterTimer += Time.deltaTime;
                if (letterTimer >= letterCooldown) {
                    letterTimer = 0f;
                    AddLetter();
                }
                break;
            case 2:
                displayTimer += Time.deltaTime;
                if (displayTimer >= displayTime && currentDialogueList.Count > 0) {
                    displayTimer = 0f;
                    dialogueState = 0;
                    NextDialogue();
                }
                break;
        }
    }

    void NextDialogue() {
        Debug.Log("Next: " + currentDialogueList[0]);
        currentDialogue = "";
        currentRemainingDialogue = currentDialogueList[0];
        currentDialogueList.RemoveAt(0);
        displayTimer = 0f;
        letterTimer = 0f;
        dialogueState = 1;
    }

    public void AddDialogue(string dialogue) {
        Debug.Log("Add: " + dialogue);
        currentDialogueList.Add(dialogue);
    }
    
    void AddLetter() {
        currentDialogue += currentRemainingDialogue[0];
        currentRemainingDialogue = currentRemainingDialogue.Substring(1);
    }
}