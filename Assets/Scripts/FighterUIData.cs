using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FighterUIData : MonoBehaviour
{
    public BattleFighter fighter;
    public TMP_Text nameText;
    public RectTransform ATBFill;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = fighter.name;
    }

    // Update is called once per frame
    void Update()
    {
        ATBFill.localScale = new Vector3(fighter.atb / 1000f, 1, 1);
    }
}
