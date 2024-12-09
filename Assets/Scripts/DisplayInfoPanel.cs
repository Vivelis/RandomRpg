using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInfoPanel : MonoBehaviour
{
    private QuestManager questManager;

    private void Start() {
        questManager = QuestManager.instance;
        questManager.onQuestStateChange.AddListener(UpdatePanel);
    }

    private void UpdatePanel() {
        int intQuestId = 0;

        try {
            intQuestId = int.Parse(questManager.GetCurrentQuestId());
        } catch (System.Exception) {
            Debug.LogError("Error parsing quest id");
            return;
        }
        if (intQuestId > 1) {
            HidePanel();
        }
    }

    private void HidePanel() {
        gameObject.SetActive(false);
    }
}
