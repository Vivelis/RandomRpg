using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    // Load data from a GameData object
    // Called when the DataPersistenceManager is loading the game
    void LoadData(GameData data);

    // Save data to a GameData object
    // Called when the DataPersistenceManager is saving the game
    void SaveData(ref GameData data);
}
