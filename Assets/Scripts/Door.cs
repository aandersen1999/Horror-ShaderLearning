using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : Interactable
{
    [SerializeField] private float degreesPerSecond = 90.0f;

    [SerializeField] private bool open = false;
    [SerializeField] private bool locked = false;
    //private Transform doorTransform;

    public UnityEvent OnUnlock;
    public UnityEvent OnOpen;
    public UnityEvent OnClose;

    public override void Interact()
    {
        if (!locked)
        {
            if (open)
            {
                transform.localEulerAngles = Vector3.up * 0;
                OnClose?.Invoke();
            }
            else
            {
                transform.localEulerAngles = Vector3.up * -90.0f;
                OnOpen?.Invoke();
            }
            open = !open;
        }
        else
        {
            UIMaster.Instance.DisplayEventMessage("It's locked");
            if (playAudioOnInteract)
            {
                if (sfxSource.isPlaying)
                    sfxSource.Stop();
                sfxSource.Play();
            }
                

        }
            
        base.Interact();
    }
}
