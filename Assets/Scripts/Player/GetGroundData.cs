using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGroundData : MonoBehaviour
{
    [SerializeField] private float groundDetectionRange = .5f;

    public GroundData Data { get; private set; }

    private void Awake()
    {
        Data = new GroundData();
    }

    private void Update()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hitInfo, groundDetectionRange))
        {
            Data.hitThisFrame = true;

            Data.distance = hitInfo.distance;
            Data.normal = hitInfo.normal;
        }
        else
        {
            Data.hitThisFrame = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * groundDetectionRange);
    }
}

public class GroundData
{
    public bool hitThisFrame;
    public float distance;
    public Vector3 normal;
}