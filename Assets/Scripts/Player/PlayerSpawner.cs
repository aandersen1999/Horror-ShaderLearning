using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private int spawnerID;

    private void Start()
    {
        GameMaster gameMaster = GameMaster.Instance;

        if(spawnerID == gameMaster.spawnerIDForNextRoom)
        {
            if (gameMaster.PlayerInstance != null)
                gameMaster.PlayerInstance.ForceSetPositionAndRotation(transform.position, transform.rotation);
            else
                Instantiate(gameMaster.PlayerPrefab, transform.position, transform.rotation);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position + Vector3.up, "Spawner.png", true);

        Vector3 pos = transform.position + Vector3.up;
        Vector3 dir = transform.forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(pos, dir);

        Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 200, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 160, 0) * Vector3.forward;

        Gizmos.DrawRay(pos + dir, right * .25f);
        Gizmos.DrawRay(pos + dir, left * .25f);
    }
#endif
}
