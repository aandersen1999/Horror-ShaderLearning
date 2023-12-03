using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    private GameData gameData;
    public GameData GameData { get { return gameData; } }

    public void NewGame()
    {
        gameData = new();
    }

    public void LoadGame()
    {

    }

    public void SaveGame()
    {

    }
}
