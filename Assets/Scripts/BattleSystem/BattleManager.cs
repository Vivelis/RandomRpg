using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    int turn = 0; //this variable is used to determine whose turn it is
    public int battleState = 0; //this variable is used to determine the state of the battle
    //0 = start of battle
    //1 = ATB in progress
    //2 = fighterAction selection
    //3 = fighterAction
    //4 = fighter state check
    //5 = end of battle
    
    public bool endBattle = false;
    public List<BattleFighter> battleFighters = new List<BattleFighter>(); //this is a list of all the fighters in the battle
    public AudioClip battleMusic; //music used during the battle
    int winner = -1; //the team that won

    public Canvas canvas;

    // UI ------------------------------------------------
    public Transform team0UIData; 
    public Transform team1UIData;
    public GameObject fighterUIDataPrefab;
    public BattleMenu battleMenu;
    public BattleDialogueBox battleDialogueBox;
    // ----------------------------------------------------

    BattleFighter currentActor;
    BattleFighter currentTarget;
    Attack currentAttack;
    bool attackUIstarted = false;

    public GameObject skeletonPrefab;

    // Start is called before the first frame update
    void Start() {
        battleDialogueBox = GameObject.Find("DialogueText").GetComponent<BattleDialogueBox>();
        battleDialogueBox.AddDialogue("Battle start!");

        InitEnemies();
        InitFightersInBattle();
        //spawn the basic UI elements
        InitUI();
    }

    // Update is called once per frame
    void Update() {
        if (endBattle && battleState < 5) battleState = 5;
        
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
                battleState = 1;
                break;
            case 1:
                //ATB in progress
                ATBprogress();
                break;
            case 2:
                //fighterAction selection
                currentActor = battleFighters[turn];
                if (currentActor.team == 0) {
                    //player turn
                    ActionSelection(currentActor);
                } else {
                    //AI turn
                    AIActionSelection(currentActor);
                    battleState = 3;
                }
                break;
            case 3:
                //fighterAction
                ExecuteAction(currentActor, currentTarget, currentAttack);
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
                    battleDialogueBox.AddDialogue("Player wins!");
                    //code to gain experience/rewards here
                    //check each dead enemy and add their experience to the player
                } else if (winner == 1) {
                    battleDialogueBox.AddDialogue("AI wins!");
                } else {
                    battleDialogueBox.AddDialogue("Fled successfully!");
                }
                CloseBattleScene();
                break;
        }
    }

    void InitUI() {
        int team0Index = 0;
        int team1Index = 0;
        Vector3 spawnPositionOffset = new Vector3(-300, 50, 0); //this is the spawn position relative to ui border
        foreach (BattleFighter fighter in battleFighters) {
            GameObject fighterUIDataObj = Instantiate(fighterUIDataPrefab, fighter.transform.position, Quaternion.identity);
            FighterUIData fighterUIData = fighterUIDataObj.GetComponent<FighterUIData>();
            fighterUIData.transform.SetParent(canvas.transform, false);

            fighterUIData.fighter = fighter;

            if (fighter.team == 0) {
                fighterUIDataObj.transform.position = team0UIData.position + spawnPositionOffset + new Vector3(0, team0Index * -50f, 0);
                team0Index++;
            } else {
                fighterUIDataObj.transform.position = team1UIData.position + spawnPositionOffset + new Vector3(0, team1Index * -50f, 0);
                team1Index++;
            }
        }

        //battleMenu initiation
        battleMenu.battleFighters = battleFighters;
    }

    void InitEnemies() {
        //randomly spawns enemies
        int numberOfSkeletons = Random.Range(1, 4); // Generate a random number of skeletons between 1 and 4.
        for (int i = 0; i < numberOfSkeletons; i++) {
            GameObject skeleton = Instantiate(skeletonPrefab);
            BattleFighter bf = skeleton.GetComponent<BattleFighter>();
            bf.team = 1;
            bf.speed = Random.Range(bf.speed - 1, bf.speed + 2);

            if (bf.speed <= 0) {
                bf.speed = 1;
            }
            //battleFighters.Add(bf);
        }
    }

    public void InitFightersInBattle() {
        //Initialize the fighter list
        int team0Index = 0;
        int team1Index = 0;
        Vector3 baseSpawnPosition = new Vector3(3, 1, 0); //subsequent units will spawn at +-2.5 Z on the sides

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("BattleFighter")) {
            BattleFighter bf = obj.GetComponent<BattleFighter>();
            battleFighters.Add(bf);
            bf.animator.SetBool("Fight", true);
            if (bf.team == 0) {
                if (team0Index == 0) {
                    obj.transform.position = baseSpawnPosition;
                } else if (team0Index == 1) {
                    obj.transform.position = baseSpawnPosition + new Vector3(0, 0, 2.5f);
                } else {
                    obj.transform.position = baseSpawnPosition + new Vector3(0, 0, -2.5f);
                }
                team0Index++;
            }  else {
                obj.transform.rotation = Quaternion.Euler(0, 180, 0);
                if (team1Index == 0) {
                    obj.transform.position = baseSpawnPosition;
                } else if (team1Index == 1) {
                    obj.transform.position = baseSpawnPosition + new Vector3(0, 0, 2.5f);
                } else {
                    obj.transform.position = baseSpawnPosition + new Vector3(0, 0, -2.5f);
                }
                Vector3 position = obj.transform.position;
                obj.transform.position = new Vector3(-position.x, position.y, position.z);
                team1Index++;
            }
        }

        InitParty();
    }

    //sets the stats of the party
    public void InitParty() {
        if (BattleData.Instance == null) {
            Debug.Log("BattleData not found or null");
            return;
        }
        Debug.Log("Init party");
        //for testing:
        BattleData.Instance.TestSave();

        foreach (BattleFighter fighter in battleFighters) {
            if (fighter.team == 0) {
                FighterSave fSave = null;
                if (fighter.name == "Lyra") {
                    fSave = BattleData.Instance.GetFighterSave("Lyra");
                } else if (fighter.name == "Compagnon") {
                    fSave = BattleData.Instance.GetFighterSave("Compagnon");
                }

                if (fSave != null) {
                    fighter.level = fSave.level;
                    fighter.exp = fSave.exp;
                    fighter.expToNextLevel = fSave.expToNextLevel;
                    fighter.maxHp = fSave.maxHp;
                    fighter.hp = fSave.hp;
                    fighter.maxMp = fSave.maxMp;
                    fighter.mp = fSave.mp;
                    fighter.attack = fSave.attack;
                    fighter.defense = fSave.defense;
                    fighter.magic = fSave.magic;
                    fighter.magicDefense = fSave.magicDefense;
                    fighter.speed = fSave.speed;
                    fighter.accuracy = fSave.accuracy;

                    Debug.Log("Loaded fighter: " + fighter.name);
                }
            }
        }
    }

    void ATBprogress() {
        string currentATBs = "";

        //ATB in progress
        foreach (BattleFighter fighter in battleFighters) {
            //increase the atb of all fighters
            fighter.atb += fighter.speed * Time.deltaTime * 45;
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
        if (currentActor.attacks.Count > 0) {

            int randomIndex = Random.Range(0, currentActor.attacks.Count);
            currentAttack = currentActor.attacks[randomIndex];
            if (currentAttack == null) {
                Debug.Log("currentAttack is null, list is empty!");
            }

            int randomTargetIndex = Random.Range(0, battleFighters.Count);
            currentTarget = battleFighters[randomTargetIndex];

            //prevents attacks on allies
            while (currentTarget.team == currentActor.team && TurnCheck() == -1) {
                randomTargetIndex = Random.Range(0, battleFighters.Count);
                currentTarget = battleFighters[randomTargetIndex];
            }
            battleDialogueBox.AddDialogue(currentActor.name + " used " + currentAttack.attackName + " on " + currentTarget.name + "!");
        } else {
            //Debug.Log(currentActor.name + " has no attacks available.");
        }
        battleState = 4;
    }
    
    void ActionSelection(BattleFighter currentActor) {
        if (attackUIstarted == false) { //initiate the attack UI
            currentTarget = null;
            currentAttack = null;

            battleMenu.OpenMenu(currentActor);
            attackUIstarted = true;
        } else { //wait for the player to select an action
            if (battleMenu.currentAttack != null && battleMenu.currentTarget != null) {
                currentAttack = battleMenu.currentAttack;
                currentTarget = battleMenu.currentTarget;
                battleState = 3;
                attackUIstarted = false;
            }
        }
    }

    void ExecuteAction(BattleFighter currentActor, BattleFighter currentTarget, Attack currentAttack) {
        if (currentActor == null || currentTarget == null || currentAttack == null) {
            Debug.Log("Invalid action");
            return;
        }
        if (currentActor == null) Debug.Log("currentActor is null");
        if (currentTarget == null) Debug.Log("currentTarget is null");
        if (currentAttack == null) Debug.Log("currentAttack is null");

        bool result = currentActor.callAttack(currentAttack, currentTarget);
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

        //update the menu
        //battleMenu.battleFighters = battleFighters;

        if (team0Alive == false) {
            return 1;
        } else if (team1Alive == false) {
            return 0;
        } else {
            return -1;
        }
    }
    
    void GainExp() {
        int totalExp = 0;
        foreach (BattleFighter fighter in battleFighters) {
            if (fighter.team == 1) {
                totalExp += fighter.level * 10;
            }
        }

        foreach (BattleFighter fighter in battleFighters) {
            if (fighter.team == 0) {
                fighter.gainExp(totalExp);
            }
        }
    }

    public void CloseBattleScene() {
        //Transfer fighter data
        //SceneManager.LoadScene(""); //Load the previous scene
        Debug.Log("Close battle scene");
        battleState = 6; //temporary    
    }
}