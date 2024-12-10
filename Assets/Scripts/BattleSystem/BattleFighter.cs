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
    float animTimer = 0f;

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
        animTimer += Time.deltaTime;

        //refreshes the animator
        if (animTimer > 1.5f) {
            animator.SetBool("Attack1", false);
            animator.SetBool("Attack2", false);
            animator.SetBool("Attack3", false);
            animator.SetBool("Attack4", false);
            animator.SetBool("Boost1", false);
            animator.SetBool("Boost2", false);
        }
    }

    public bool callAttack(Attack attack, BattleFighter target) {
        
        //resource check
        if (attack.mpCost > mp) {
            battleDialogueBox.AddDialogue(name + " doesn't have enough MP to use " + attack.name);
            return false;
        }
        Debug.Log("mp before = " + mp);
        mp -= attack.mpCost;
        Debug.Log("mp after = " + mp);
        setAttackAnim(attack);
        attack.AttackEffect(this, target);
        return true;
    }

    public void takeDamage(int damage) {
        hp -= damage;

        if (hp <= 0) { //on death
            hp = 0;
            status = 0;
            battleDialogueBox.AddDialogue(name + " died.");
            animator.SetBool("Dead", true);
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
        //upgrade stats here
        maxHp += 1;
        maxMp += 1;
        hp += 1;
        mp += 1;
        attack += 1;
        defense += 1;
        magic += 1;
        magicDefense += 1;
        speed += 1;
        accuracy += 1;
    }

    public void setAttackAnim(Attack attack) {
        int ind = attack.attackAnimNb;

        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
        animator.SetBool("Attack4", false);
        animator.SetBool("Boost1", false);
        animator.SetBool("Boost2", false);

        if (ind < 4 && ind > 0) {
            animator.SetBool("Attack" + ind, true);
        } else {
            animator.SetBool("Boost" + (ind - 3), true);
        }

        animTimer = 0f;
    }
}
