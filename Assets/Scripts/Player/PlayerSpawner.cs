using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private int spawnerID;
    public int SpawnerID { get { return spawnerID; } }

    private void Start()
    {
        GameMaster gameMaster = GameMaster.Instance;

        if(spawnerID == gameMaster.spawnerIDForNextRoom)
        {
            if (gameMaster.PlayerInstance != null)
                gameMaster.PlayerInstance.ForceSetPositionAndRotation(transform.position, transform.rotation);
            else
                Instantiate(gameMaster.PlayerPrefab, transform.position, transform.rotation);

            ControllerManager.Instance.LockCursor(true);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position + Vector3.up, "Spawner.png");

        Vector3 pos = transform.position + Vector3.up;
        Vector3 dir = transform.forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(pos, dir);

        Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 200, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 160, 0) * Vector3.forward;

        Gizmos.DrawRay(pos + dir, right * .25f);
        Gizmos.DrawRay(pos + dir, left * .25f);
    }
/*
    private void OnValidate()
    {
        
        CompareIDs c = new();
        PlayerSpawner[] list = FindObjectsOfType<PlayerSpawner>();
        int lowestAvailableID = 0;
        //if it made it to the end of the list without seeing an available number
        bool endOfList = true;

        if (list.Length == 0)
        {
            spawnerID = 0;
            return;
        }

        Array.Sort(list, c);

        for(int i = 0; i < list.Length; i++)
        {
            if (list[i].SpawnerID != i)
            {
                lowestAvailableID = i;
                endOfList = false;
                break;
            }
        }
        
    }
*/

    private class CompareIDs : IComparer<PlayerSpawner>
    {
        public int Compare(PlayerSpawner x, PlayerSpawner y)
        {
            if (x.SpawnerID < y.SpawnerID)
                return -1;
            else if (x.SpawnerID == y.SpawnerID)
                return 0;
            return 1;
        }
    }
#endif
}
