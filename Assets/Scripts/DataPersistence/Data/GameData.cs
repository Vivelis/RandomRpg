using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int score;
    public int health;
    public int lives;
    public float time;
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public SerializableDictionary<string, bool> collectedItems;

    public GameData()
    {
        score = 0;
        health = 100;
        lives = 3;
        time = 0;
        playerPosition = new Vector3(0, 0, 0);
        playerRotation = new Quaternion(0, 0, 0, 0);
        collectedItems = new SerializableDictionary<string, bool>();
    }
}
