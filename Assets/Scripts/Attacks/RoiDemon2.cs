using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoiDemon2 : Attack
{
    void Start() {
    }

    public override void AttackEffect(BattleFighter attacker, BattleFighter defender) {

        if (!PrecisionCheck(attacker, defender)) {
            return;
        }
        InflictDamage(attacker, defender);      
    }
}
