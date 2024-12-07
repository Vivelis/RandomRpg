using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "FighterSave", menuName = "ScriptableObjects/FighterSave", order = 1)]
public class FighterSave : ScriptableObject
{
    public int team = 1; //team of the fighter. 0 = player, 1 = AI
    public string name = "";
    public int maxHp = 0;
    public int maxMp = 0;
    public int hp = 0;
    public int mp = 0;
    public int attack = 0;
    public int defense = 0;
    public int magic = 0;
    public int magicDefense = 0;
    public int speed = 0;
    public int accuracy = 0;
    public List<Attack> attacks = new List<Attack>(); //list of attacks
    public int status = 1; //1 = alive, 0 = dead
    public float atb = 0; //this is to track when the fighter will take its turn (see final fantasy ATB system for more detail)
    public int level = 1;
    public int exp = 0;
    public int expToNextLevel = 100;
    public GameObject prefab; //prefab of the fighter
}
