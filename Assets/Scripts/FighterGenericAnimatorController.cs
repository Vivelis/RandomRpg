using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    public class FighterGenericAnimatorController : MonoBehaviour
    {
        void Start()
        {
            //Takes the data from the previous scene and creates the fighters accordingly here.
            List<BattleFighter> fighters = new List<BattleFighter>();
            //After that, call the battleManager
            BattleManager bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            //Set them all to battle animations
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("BattleFighter")) {
                fighters.Add(obj.GetComponent<BattleFighter>());
            }

            foreach (BattleFighter fighter in fighters) {
                fighter.animator.SetBool("Fight", true);
            }
            bm.InitFightersInBattle();
        }
    }
}
