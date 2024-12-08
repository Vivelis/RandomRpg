using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public string attackName;
    public float attackScale;
    public float magicScale;
    public int mpCost;
    public int accuracy;
    public int targetSetting; //setting: 0 = ally, 1 = enemy, 2 = self, 3 = all
    public BattleDialogueBox battleDialogueBox;
    public int attackAnimNb;

    void start () {
    }

    public virtual void AttackEffect(BattleFighter attacker, BattleFighter defender)
    {
        Debug.Log("Attack effect");
    }

    public virtual int DamageCalculation(BattleFighter attacker, BattleFighter defender)
    {
        // Base damage calculations
        float baseMeleeDmg = attacker.attack * attackScale;
        float baseMagicDmg = attacker.magic * magicScale;
    
        // Tuning constant for scaling defense reduction
        const float k = 50f;
    
        // Calculate reduction factors
        float meleeReduction = defender.defense / (defender.defense + k);
        float magicReduction = defender.magicDefense / (defender.magicDefense + k);
    
        // Apply reductions
        int reducedMeleeDmg = (int)(baseMeleeDmg * (1 - meleeReduction));
        int reducedMagicDmg = (int)(baseMagicDmg * (1 - magicReduction));
    
        // Total damage
        int totalDmg = reducedMeleeDmg + reducedMagicDmg;
    
        return totalDmg;
    }

    public void InflictDamage(BattleFighter attacker, BattleFighter defender) {
        int damage = DamageCalculation(attacker, defender);
        defender.takeDamage(damage);
        battleDialogueBox = GameObject.Find("DialogueText").GetComponent<BattleDialogueBox>();
        battleDialogueBox.AddDialogue(attacker.name + " dealt " + damage + " damage to " + defender.name + "!");
    }

    public bool PrecisionCheck(BattleFighter attacker, BattleFighter defender) {
        if (Random.Range(1, 101) <= accuracy) {
            return true;
        }
        battleDialogueBox = GameObject.Find("DialogueText").GetComponent<BattleDialogueBox>();
        battleDialogueBox.AddDialogue(attacker.name + " missed!");
        return false;
    }
}
