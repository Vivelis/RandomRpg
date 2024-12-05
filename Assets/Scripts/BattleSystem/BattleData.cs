using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    public static BattleData Instance;

    private List<string> enemyNames = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBattleData(List<string> enemies)
    {
        enemyNames = enemies;
    }

    public List<string> GetBattleData()
    {
        return enemyNames;
    }
}