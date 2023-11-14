using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private float degreesPerSecond = 90.0f;

    [SerializeField] private bool open = false;
    //private Transform doorTransform;

    private void Awake()
    {
        //doorTransform = transform.GetChild(0);
    }

    public override void Interact()
    {
        base.Interact();

        transform.localEulerAngles = (open) ? Vector3.up * -90.0f : Vector3.up * 0;
        open = !open;
    }
}
