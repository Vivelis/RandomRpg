using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    int turn = 0; //this variable is used to determine whose turn it is
    int battleState = 0; //this variable is used to determine the state of the battle
    //0 = start of battle
    //1 = ATB in progress
    //2 = fighterAction selection
    //3 = fighterAction
    //4 = fighter state check
    //5 = end of battle
    public List<BattleFighter> battleFighters = new List<BattleFighter>(); //this is a list of all the fighters in the battle
    public AudioClip battleMusic; //music used during the battle

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Once the battle has started, any initiation code will occur in state 0.
        After this is done, the battle will loop from state 1 to 4 until all fighters of one team are dead.
        In state 1. All the action bars of the fighters will increase until one of them has reached 1000.
        Once one of the action bars has reached 1000, the battle will move to state 2. In state 2, the player or AI will be able to select a fighter to attack and move to state 3.
        In state 3, the attack is executed, and effects are applied. The battle will then move to state 4.
        In state 4, the state of the fighters will be checked. Once that is done, if any team won the battle, the battle goes to state 5, otherwise, it goes back to state 1 and waits for the next full action bar.
        */

        switch (battleState) {
            case 0:
                //start of battle
                //set music to battleMusic
                //spawn the fighters in the appropriate positions
                break;
            case 1:
                //ATB in progress
                ATBprogress();
                break;
            case 2:
                //fighterAction selection
                break;
            case 3:
                //fighterAction
                break;
            case 4:
                //fighter state check
                break;
            case 5:
                //end of battle
                break;
        }
    }

    void ATBprogress() {
        //ATB in progress
        foreach (BattleFighter fighter in battleFighters) {
            //increase the atb of all fighters
            fighter.atb += fighter.speed * Time.deltaTime / 2;
        }

        //check if any fighter has reached 1000
        BattleFighter highestATBFighter = null;
        float highestATBValue = 0;

        foreach (BattleFighter fighter in battleFighters) {
            if (fighter.atb >= 1000) {
                if (highestATBFighter == null) {
                    highestATBFighter = fighter;
                    highestATBValue = fighter.atb;
                } else if (fighter.atb > highestATBValue) {
                    highestATBFighter = fighter;
                    highestATBValue = fighter.atb;
                }

                turn = battleFighters.IndexOf(highestATBFighter);
            }
        }

        if (highestATBFighter != null) {
            //highestATBFighter has reached 1000, move to state 2
            battleState = 2;
        }
    }
}
