using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairDecent : Interactable
{
    public override void Interact()
    {
        GameMaster.Instance.ChangeScene(3);

        base.Interact();
    }
}
