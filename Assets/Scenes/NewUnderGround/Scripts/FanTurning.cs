using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTurning : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.localEulerAngles +=  Vector3.up * (speed * Time.deltaTime);
    }
}
