using UnityEngine;
using System.Collections.Generic;

public class BattleManagerData : MonoBehaviour
{
    [Header("Prefab des ennemis")]
    [SerializeField] private List<GameObject> enemyPrefabs;

    private void Start()
    {
        var selectedEnemies = BattleData.Instance.GetBattleData();

        foreach (var enemyName in selectedEnemies)
        {
            GameObject prefab = enemyPrefabs.Find(e => e.name == enemyName);
            if (prefab != null)
            {
                Instantiate(prefab, GetRandomPosition(), Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"Prefab pour l'ennemi '{enemyName}' non trouvé !");
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
    }
}
