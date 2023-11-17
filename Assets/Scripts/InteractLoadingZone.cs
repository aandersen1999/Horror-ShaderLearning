using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractLoadingZone : Interactable
{
    [SerializeField] private SceneReference loadingZoneDestination;
    [SerializeField] private int spawnerID;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "LoadingZone.png", true);
    }
#endif

    public override void Interact()
    {
        GameMaster.Instance.ChangeScene(loadingZoneDestination, spawnerID);

        base.Interact();
    }

}
