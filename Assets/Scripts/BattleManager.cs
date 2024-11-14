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
    int winner = -1; //the team that won

    // UI ------------------------------------------------
    public Transform team0UIData; 
    public Transform team1UIData;

    public GameObject fighterUIDataPrefab;
    // ----------------------------------------------------

    // Start is called before the first frame update
    void Start() {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("BattleFighter")) {
            battleFighters.Add(obj.GetComponent<BattleFighter>());
        }
        Debug.Log("Battle Fighters: " + battleFighters.Count);

        //spawn the UI elements
        int team0Index = 0;
        int team1Index = 0;
        Vector3 spawnPositionOffset = new Vector3(-178, 36, 0); //this is the spawn position relative to ui border
        foreach (BattleFighter fighter in battleFighters) {
            GameObject fighterUIDataObj = Instantiate(fighterUIDataPrefab, fighter.transform.position, Quaternion.identity);
            FighterUIData fighterUIData = fighterUIDataObj.GetComponent<FighterUIData>();

            fighterUIData.fighter = fighter;

            if (fighter.team == 0) {
                fighterUIDataObj.transform.position = team0UIData.position + spawnPositionOffset + new Vector3(0, team0Index * 2.5f, 0);
            } else {
                //fighterUIData.Tranform = team1UIData + spawnPositionOffset + new Vector3(0, team0Index * 2.5f, 0);
            }
        }
    }

    // Update is called once per frame
    void Update() {
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
                battleState = 1;
                break;
            case 1:
                //ATB in progress
                ATBprogress();
                break;
            case 2:
                //fighterAction selection
                BattleFighter currentActor = battleFighters[turn];
                if (currentActor.team == 0) {
                    //player turn
                    ActionSelection(currentActor);
                } else {
                    //AI turn
                    AIActionSelection(currentActor);
                }
                battleState = 3;
                break;
            case 3:
                //fighterAction
                battleState = 4;
                break;
            case 4:
                //fighter state check
                int result = TurnCheck();
                if (result != -1) {
                    //team 0 wins
                    winner = result;
                    battleState = 5;
                } else {
                    battleState = 1;
                }
                break;
            case 5:
                //end of battle
                if (winner == 0) {
                    Debug.Log("Player wins!");
                    //code to gain experience/rewards here
                } else {
                    Debug.Log("AI wins!");
                }
                break;
        }
        //Debug.Log("Battle State: " + battleState);
    }

    void ATBprogress() {
        string currentATBs = "";

        //ATB in progress
        foreach (BattleFighter fighter in battleFighters) {
            //increase the atb of all fighters
            fighter.atb += fighter.speed;
        }

        //check if any fighter has reached 1000
        BattleFighter highestATBFighter = null;
        float highestATBValue = 0;

        foreach (BattleFighter fighter in battleFighters) {
            if (fighter.atb >= 1000) {
                //get the fighter with the highest atb if there are multiple ones above 1000
                if (highestATBFighter == null) {
                    highestATBFighter = fighter;
                    highestATBValue = fighter.atb;
                } else if (fighter.atb > highestATBValue) {
                    highestATBFighter = fighter;
                    highestATBValue = fighter.atb;
                }
            }
            currentATBs += fighter.name + ": " + fighter.atb + ", ";
        }

        //Debug.Log(currentATBs);

        if (highestATBFighter != null) {
            //highestATBFighter has reached 1000, move to state 2
            turn = battleFighters.IndexOf(highestATBFighter);
            battleState = 2;
            highestATBFighter.atb = 0;
        }
    }

    void AIActionSelection(BattleFighter currentActor) {
        Debug.Log("AI turn: " + currentActor.name + " did something");

    }
    void ActionSelection(BattleFighter currentActor) {
        Debug.Log("Player turn: " + currentActor.name + " did something");
    }

    int TurnCheck() {
        var team0Alive = false;
        var team1Alive = false;

        foreach (BattleFighter fighter in battleFighters) {
            if (fighter.hp > 0) {
                if (fighter.team == 0) {
                    team0Alive = true;
                } else {
                    team1Alive = true;
                }
            } else {
                fighter.status = 0;
            }
        }

        if (team0Alive == false) {
            return 1;
        } else if (team1Alive == false) {
            return 0;
        } else {
            return -1;
        }
    }
}
