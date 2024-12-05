using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : Attack
{
    void Start() {
    }

    public override void AttackEffect(BattleFighter attacker, BattleFighter defender) {
        int randomNumber = Random.Range(0, 101);
        if (randomNumber > 50) {
            Debug.Log(attacker.name + " fled!");
            BattleManager battleManager = GameObject.FindObjectOfType<BattleManager>();
            if (battleManager.battleState == 3) {
                battleManager.endBattle = true;
            }
        } else {
            Debug.Log(attacker.name + " failed to flee!");
        }

    }
}
