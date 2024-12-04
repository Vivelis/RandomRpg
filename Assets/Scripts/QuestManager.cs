using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public Dictionary<string, QuestStateData> questData;
    private string currentQuestId = "1";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadQuestData();
            SetupPNJsInScene();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadQuestData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("dialogue_config");

        if (jsonFile != null)
        {
            try
            {
                QuestDataWrapper wrapper = JsonUtility.FromJson<QuestDataWrapper>(jsonFile.text);

                if (wrapper != null && wrapper.questData != null)
                {
                    questData = new Dictionary<string, QuestStateData>();
                    foreach (var questState in wrapper.questData)
                    {
                        questData.Add(questState.questId, questState);
                    }
                }
                else
                {
                    Debug.LogError("Erreur : les données JSON sont nulles ou vides après la désérialisation.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Erreur lors de la désérialisation du JSON : {ex.Message}");
            }
        }
        else
        {
            Debug.LogError("Erreur : fichier JSON introuvable dans Resources !");
        }
    }

    public void SaveGameState()
    {
        QuestDataWrapper saveData = new QuestDataWrapper { questData = new List<QuestStateData>(questData.Values) };

        string savePath = Application.persistentDataPath + "/savegame.json";
        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(savePath, json);
    }

    public void LoadGameState()
    {
        string savePath = Application.persistentDataPath + "/savegame.json";

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            QuestDataWrapper loadedData = JsonUtility.FromJson<QuestDataWrapper>(json);

            if (loadedData != null && loadedData.questData != null)
            {
                questData = new Dictionary<string, QuestStateData>();
                foreach (var questState in loadedData.questData)
                {
                    questData.Add(questState.questId, questState);
                }
            }
            else
            {
                Debug.LogError("Erreur lors du chargement des données de sauvegarde.");
            }
        }
        else
        {
            LoadQuestData();
        }
    }

    public void SetupPNJsInScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (questData != null && questData.ContainsKey(currentQuestId))
        {
            var quest = questData[currentQuestId];
            foreach (var scene in quest.scenes)
            {
                if (scene.sceneName == currentSceneName)
                {
                    foreach (var pnjStatus in scene.pnjStatuses)
                    {
                        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
                        foreach (GameObject obj in allObjects)
                        {
                            if (pnjStatus.pnjName == "Compagnon" && pnjStatus.active == true && obj.name == "Compagnon" && obj.scene == SceneManager.GetActiveScene())
                            {
                                obj.SetActive(true);
                            } else if (pnjStatus.pnjName == "Roi démon" && pnjStatus.active == true && obj.name == "Roi démon" && obj.scene == SceneManager.GetActiveScene())
                            {
                                obj.SetActive(true);
                            }
                        }
                        GameObject pnjObject = GameObject.Find(pnjStatus.pnjName);
                        if (pnjObject != null)
                        {
                            DialogueSystem dialogueSystem = pnjObject.GetComponent<DialogueSystem>();
                            if (dialogueSystem != null)
                            {
                                dialogueSystem.UpdateStatus(pnjStatus);
                            }
                            else
                            {
                                Debug.LogWarning($"DialogueSystem manquant pour le PNJ {pnjStatus.pnjName} !");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"PNJ {pnjStatus.pnjName} introuvable dans la scène {currentSceneName}.");
                        }
                    }
                    return;
                }
            }
            Debug.LogWarning($"Aucune donnée trouvée pour la scène : {currentSceneName} dans la quête ID {currentQuestId}.");
        }
        else
        {
            Debug.LogWarning($"Données de quête introuvables ou inexistantes pour l'ID {currentQuestId}.");
        }
    }

    public void AdvanceQuestState()
    {
        int nextQuestId = int.Parse(currentQuestId) + 1;
        currentQuestId = nextQuestId.ToString();
        SetupPNJsInScene();
    }
}

[System.Serializable]
public class QuestDataWrapper
{
    public List<QuestStateData> questData;
}

[System.Serializable]
public class QuestStateData
{
    public string questId;
    public List<SceneData> scenes;
}

[System.Serializable]
public class SceneData
{
    public string sceneName;
    public List<PNJStatus> pnjStatuses;
}

[System.Serializable]
public class PNJStatus
{
    public string pnjName;
    public bool active;
    public int dialogueState;
    public bool action;
}
