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
            //After that, call the battleManager
            BattleManager bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            bm.InitFightersInBattle();
        }
    }
}
