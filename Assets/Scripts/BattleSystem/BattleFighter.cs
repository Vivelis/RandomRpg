using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFighter : MonoBehaviour
{
    public int team; //team of the fighter. 0 = player, 1 = AI
    public string name;
    public int maxHp;
    public int maxMp;

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
    public int level;
    public int exp;
    public int expToNextLevel;
    BattleDialogueBox battleDialogueBox;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        mp = maxMp;
        battleDialogueBox = GameObject.Find("DialogueText").GetComponent<BattleDialogueBox>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool callAttack(Attack attack, BattleFighter target) {
        
        //resource check
        if (attack.mpCost > mp) {
            return false;
        }

        mp -= attack.mpCost;
        attack.AttackEffect(this, target);
        return true;
    }

    public void takeDamage(int damage) {
        hp -= damage;

        if (hp <= 0) { //on death
            hp = 0;
            status = 0;
            battleDialogueBox.AddDialogue(name + " died.");
        }
    }

    public void takeHealing(int healing) {
        hp += healing;

        if (hp > maxHp) {
            hp = maxHp;
        }
    }

    public void gainExp(int exp) {
        this.exp += exp;
        while (this.exp >= expToNextLevel) {
            this.exp -= expToNextLevel;
            levelUp();
        }
    }

    public void levelUp() {
        level += 1;
        expToNextLevel = (int)Mathf.Round(expToNextLevel * 1.1f);
    }
}
