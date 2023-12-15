using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : Interactable
{
    public AudioClip[] clips;

    private AudioSource source;

    protected new void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        base.Interact();
        if (!source.isPlaying)
        {
            int song = Random.Range(0, clips.Length);
            source.clip = clips[song];
            source.Play();
        }
        else
        {
            source.Stop();
        }
    }
}
