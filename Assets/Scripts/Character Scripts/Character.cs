using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float speed = 20.0f;
    

    private void Update()
    {
        transform.eulerAngles += speed * Time.deltaTime * Vector3.up;
    }
}
