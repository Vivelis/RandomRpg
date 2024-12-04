using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FighterUIData : MonoBehaviour
{
    public BattleFighter fighter;
    public TMP_Text nameText;
    public RectTransform ATBFill;
    public TMP_Text hpText;
    public TMP_Text mpText;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = fighter.name + " " + "lvl: " + fighter.level;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ATBFill.localScale = new Vector3(fighter.atb / 1000f, 1, 1);
        hpText.text = fighter.hp + " / " + fighter.maxHp;
        mpText.text = fighter.mp + " / " + fighter.maxMp;
    }
}
