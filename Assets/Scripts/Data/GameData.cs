using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Dictionary<KeyIDs, bool> HasKeysSave = new();
    public Dictionary<string, IDataPersistence> DataPersistence = new();

    public GameData()
    {
        foreach(KeyIDs id in System.Enum.GetValues(typeof(KeyIDs)))
        {
            HasKeysSave[id] = false;
        }
    }
}
