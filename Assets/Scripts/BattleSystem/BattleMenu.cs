using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    public bool menuActive;
    public List<BattleFighter> battleFighters = new List<BattleFighter>();
    public BattleFighter currentActor;
    public int menuWindow;
    public Vector2 cursorPos;
    Vector2 baseButtonPos;
    float buttonDistanceX;
    float buttonDistanceY;
    public Attack currentAttack = null;
    public BattleFighter currentTarget = null;
    public Attack baseAttack;
    public Attack fireballAttack;
    public Attack FleeAttack;

    //-Menus---------------------------------------
    public GameObject mainBmenu;
    //---------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        menuActive = false;
        mainBmenu.SetActive(false);
        menuWindow = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenMenu(BattleFighter actor) {
        currentActor = actor;
        menuActive = true;
        mainBmenu.SetActive(true);
        menuWindow = 0;
        currentAttack = null;
        currentTarget = null;
    }

    //Returns the precise selected action. Can be an attack or battle options such as run
    public void CloseMenu() {
        menuActive = false;
        mainBmenu.SetActive(false);
    }

    public void SelectBaseAttack() {
        Debug.Log("Selected base attack");
        currentAttack = baseAttack;
        SelectTarget(currentAttack.targetSetting);
    }

    public void SelectFireBall() {
        if (currentActor.name == "Compagnon") return;
        Debug.Log("Selected fireball");
        currentAttack = fireballAttack;
        SelectTarget(currentAttack.targetSetting);
    }

    public void SelectRunAway() {
        Debug.Log("Selected run away");
        currentAttack = FleeAttack;
        currentTarget = currentActor;
        CloseMenu();
    }

    public void SelectTarget(int setting) {
        //setting: 0 = ally, 1 = enemy, 2 = self, 3 = all
        
        if (setting == 2) {
            currentTarget = currentActor;
        } else {
            //random selection for now
            int randomIndex = Random.Range(0, battleFighters.Count);
            currentTarget = battleFighters[randomIndex];

            bool foundEnemy = false;
            foreach (BattleFighter fighter in battleFighters) {
                if (fighter.team != currentActor.team) {
                    foundEnemy = true;
                    break;
                }
            }

            if (foundEnemy) {
                while (!(setting == 3 || (setting == 0 && currentTarget.team == currentActor.team) || (setting == 1 && currentTarget.team != currentActor.team))) {
                    randomIndex = Random.Range(0, battleFighters.Count);
                    currentTarget = battleFighters[randomIndex];
                }
            } else {
                //Didn't find a suitable target so hits itself
                currentTarget = currentActor;
            }
            
            CloseMenu();
        }
    }
}
