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

    private bool isPlayerInZone = false;

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
            yield return new WaitForSeconds(checkInterval);

            Debug.Log("V�rification d'un combat...");
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
            Debug.LogWarning("Aucune sc�ne de combat d�finie !");
            return;
        }

        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
        List<string> selectedEnemies = new List<string>();

        Debug.Log("G�n�ration des ennemis...");
        for (int i = 0; i < enemyCount; i++)
        {
            int randomIndex = Random.Range(0, possibleEnemies.Count);
            selectedEnemies.Add(possibleEnemies[randomIndex].name);
        }

        BattleData.Instance.SetBattleData(selectedEnemies);

        SceneManager.LoadScene(battleSceneName);
    }
}