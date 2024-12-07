using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float time;
    public string currentQuestId;
    public string currentScene;
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    public GameData()
    {
        time = 0;
        currentScene = "IntroductionScene";
        currentQuestId = "1";
        playerPosition = new Vector3(0, 0, 0);
        playerRotation = new Quaternion(0, 0, 0, 0);
    }
}
