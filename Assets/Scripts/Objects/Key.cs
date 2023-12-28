using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable, IDataPersistence
{
    [field:SerializeField] public string DataPersistanceName { get; set; }
    public KeyIDs KeyID;

    private new void Start()
    {
        
    }
}
