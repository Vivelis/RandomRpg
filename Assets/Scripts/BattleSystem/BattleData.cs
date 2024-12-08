using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    public static BattleData Instance;

    private List<string> enemyNames = new List<string>();
    private List<FighterSave> fighterSaves = new List<FighterSave>();

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

    public void SetFighterSaves(List<FighterSave> saves)
    {
        fighterSaves = saves;
    }

    public List<FighterSave> GetFighterSaves()
    {
        return fighterSaves;
    }

    public FighterSave GetFighterSave(string name)
    {
        foreach (FighterSave save in fighterSaves)
        {
            if (save.name == name)
                return save;
        }
        return null;
    }

    public void TestSave()
    {
        FighterSave Lyra = new FighterSave();
        Lyra.name = "Lyra";
        Lyra.level = 1;
        Lyra.exp = 0;
        Lyra.expToNextLevel = 100;
        Lyra.maxHp = 15;
        Lyra.hp = 15;
        Lyra.maxMp = 20;
        Lyra.mp = 20;
        Lyra.attack = 5;
        Lyra.defense = 5;
        Lyra.magic = 10;
        Lyra.magicDefense = 8;
        Lyra.speed = 5;
        Lyra.accuracy = 5;

        fighterSaves.Add(Lyra);

        FighterSave Compagnon = new FighterSave();
        Compagnon.name = "Compagnon";
        Compagnon.level = 1;
        Compagnon.exp = 0;
        Compagnon.expToNextLevel = 100;
        Compagnon.maxHp = 20;
        Compagnon.hp = 20;
        Compagnon.maxMp = 10;
        Compagnon.mp = 10;
        Compagnon.attack = 8;
        Compagnon.defense = 6;
        Compagnon.magic = 3;
        Compagnon.magicDefense = 4;
        Compagnon.speed = 4;
        Compagnon.accuracy = 5;

        fighterSaves.Add(Compagnon);
    }

    //heals the party to full health
    public void HealParty()
    {
        foreach (FighterSave save in fighterSaves)
        {
            save.hp = save.maxHp;
            save.mp = save.maxMp;
        }
    }
}