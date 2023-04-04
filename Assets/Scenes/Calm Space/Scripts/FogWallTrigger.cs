using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWallTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 playerStartPosition = new(103.5f, 51.99f, 12.09f);

    private bool detectedPlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.ForceSetPositionAndRotation(playerStartPosition, Quaternion.identity);
        }
    }

    
}
