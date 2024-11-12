using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFighter : MonoBehaviour
{
    public int team; //team of the fighter
    public string name;
    public int hp;
    public int mp;
    public int attack;
    public int defense;
    public int magic;
    public int magicDefense;
    public int speed;
    public int accuracy;
    public List<Attack> attacks = new List<Attack>(); //list of attacks
    public int status = 1; //1 = alive, 0 = dead
    public float atb = 0; //this is to track when the fighter will take its turn (see final fantasy ATB system for more detail)

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    bool callAttack(Attack attack, BattleFighter target) {
        
        //resource check
        if (attack.mpCost > mp) {
            return false;
        }
        attack.AttackEffect(this, target);
        return true;
    }

    void takeDamage(int damage) {
        hp -= damage;

        if (hp <= 0) { //on death
            hp = 0;
            status = 0;
        }
    }
}
