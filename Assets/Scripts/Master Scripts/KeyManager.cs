using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : Singleton<KeyManager>
{
    public Dictionary<KeyIDs, bool> HasKeys = new();

    private void Start()
    {
        foreach(KeyIDs i in System.Enum.GetValues(typeof(KeyIDs)))
        {
            HasKeys[i] = false;
        }
    }
}

public enum KeyIDs
{
    Underground_Bedroom
}