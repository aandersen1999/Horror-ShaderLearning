using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{


    public override void Interact()
    {
        GameMaster.Instance.ChangeScene(2);

        base.Interact();
    }
}
