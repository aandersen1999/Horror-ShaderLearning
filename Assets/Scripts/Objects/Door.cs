using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    [SerializeField] private DoorAudioClips audioClips = new();

    public override void Interact()
    {
        if (!locked)
        {
            if (open)
            {
                transform.localEulerAngles = Vector3.up * 0;
                PlayAudioClip(audioClips.CloseAudio);
                OnClose?.Invoke();
            }
            else
            {
                transform.localEulerAngles = Vector3.up * -90.0f;
                PlayAudioClip(audioClips.OpenAudio);
                OnOpen?.Invoke();
            }
            open = !open;
        }
        else
        {
            UIMaster.Instance.DisplayEventMessage("It's locked");
            PlayAudioClip(audioClips.LockedAudio);
        }
            
        base.Interact();
    }

}

[Serializable]
struct DoorAudioClips
{
    [field:SerializeField] public AudioClip LockedAudio { get; private set; }
    [field: SerializeField] public AudioClip OpenAudio { get; private set; }
    [field: SerializeField] public AudioClip CloseAudio { get; private set; }
}