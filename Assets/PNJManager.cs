using UnityEngine;

public class PNJManager : MonoBehaviour
{
    private void Start()
    {
        CheckPNJVisibility();
    }

    private void CheckPNJVisibility()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        DialogueSystem[] allPNJs = FindObjectsOfType<DialogueSystem>();
        foreach (DialogueSystem pnj in allPNJs)
        {
            /*PNJStatus status = QuestManager.instance.GetPNJStatus(sceneName, pnj.pnjName);
            if (status != null)
            {
                pnj.gameObject.SetActive(status.active);
            }
            else
            {
                Debug.LogWarning($"Statut introuvable pour le PNJ {pnj.pnjName} dans la scène {sceneName}.");
                pnj.gameObject.SetActive(false);
            }*/
        }
    }
}