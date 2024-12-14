using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class BattleZone : MonoBehaviour
{
    [Header("Param�tres de la zone de combat")]
    [SerializeField] private string battleSceneName;
    [SerializeField] private int minEnemies = 1;
    [SerializeField] private int maxEnemies = 3;
    [SerializeField] private List<GameObject> possibleEnemies;
    [SerializeField] private float encounterChance = 0.5f;
    [SerializeField] private float checkInterval = 1.0f;

    private float elapsedTime = 0.0f;

    private bool isPlayerInZone = false;

    public QuestManager questManager;

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= checkInterval)
        {
            Debug.Log("Verif combat");
            if (Random.value < encounterChance) {
                StartBattle();
            }
            elapsedTime = 0.0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() || other.CompareTag("Player"))
        {
            Debug.Log("Le joueur est dans la zone de combat !");
            isPlayerInZone = true;
            StartCoroutine(CheckForEncounter());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() || other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator CheckForEncounter()
    {
        while (isPlayerInZone)
        {
            //yield return new WaitForSeconds(checkInterval);
            if (elapsedTime < checkInterval) {
                yield return null;
            }

            Debug.Log("Verification d'un combat...");
            if (Random.value < encounterChance)
            {
                StartBattle();
                yield break;
            }
        }
    }

    void StartBattle()
    {
        if (string.IsNullOrEmpty(battleSceneName))
        {
            Debug.LogWarning("Aucune scene de combat definie !");
            return;
        }

        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
        List<string> selectedEnemies = new List<string>();

        Debug.Log("Generation des ennemis...");
        for (int i = 0; i < enemyCount; i++)
        {
            int randomIndex = Random.Range(0, possibleEnemies.Count);
            selectedEnemies.Add(possibleEnemies[randomIndex].name);
        }

        if (BattleData.Instance == null) {
            BattleData.Instance = new BattleData();
            BattleData.Instance.TestSave();
        }
        BattleData.Instance.SetBattleData(selectedEnemies);
        BattleData.Instance.previousScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        BattleData.Instance.previousPosition = GameObject.Find("Player").transform.position;
        BattleData.Instance.previousCameraRotation = GameObject.Find("Main Camera").transform.rotation;
        
        if (int.Parse(questManager.GetCurrentQuestId()) <= 4)
        {
            BattleData.Instance.compagnonState = 0;
        } else if (int.Parse(questManager.GetCurrentQuestId()) > 8)
        {
            BattleData.Instance.compagnonState = 2;
        }

        Debug.Log("Lancement du combat...");
        SceneManager.LoadScene(battleSceneName);
    }

    public void StartBattleSpe(int QuestId)
    {
        if (string.IsNullOrEmpty(battleSceneName))
        {
            Debug.LogWarning("Aucune scene de combat definie !");
            return;
        }

        if (QuestId == 5)
        {
            if (BattleData.Instance == null)
            {
                BattleData.Instance = new BattleData();
                BattleData.Instance.TestSave();
            }
            BattleData.Instance.previousScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            BattleData.Instance.previousPosition = GameObject.Find("Player").transform.position;
            BattleData.Instance.previousCameraRotation = GameObject.Find("Main Camera").transform.rotation;

            BattleData.Instance.compagnonState = 1;

            SceneManager.LoadScene(battleSceneName);
        } else if (QuestId == 8) {
            if (BattleData.Instance == null)
            {
                BattleData.Instance = new BattleData();
                BattleData.Instance.TestSave();
            }
            BattleData.Instance.previousScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            BattleData.Instance.previousPosition = GameObject.Find("Player").transform.position;
            BattleData.Instance.previousCameraRotation = GameObject.Find("Main Camera").transform.rotation;

            BattleData.Instance.compagnonState = 2;

            SceneManager.LoadScene(battleSceneName);
        }
    }
}