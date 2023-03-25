using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericObject : Interactable
{
    [SerializeField] private bool playSound = false;
    [SerializeField] private bool displayMessage = false;

    [SerializeField] private AudioClip sfx;
    [SerializeField] private string message = string.Empty;
    private AudioSource sfxSource;

    protected new void Awake()
    {
        if (playSound)
        {
            if (sfx == null)
            {
                Debug.LogError($"No audio clip found. Turning off 'playSound' for object {gameObject}");

                playSound = false;
            }

            if (!TryGetComponent(out sfxSource))
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
            }
            sfxSource.clip = sfx;
        }
    }

    public override void Interact()
    {
        if (displayMessage)
        {
            GameMaster.Instance.UIMaster.DisplayEventMessage(message);
        }
        if (playSound)
        {
            if (sfxSource.isPlaying)
            {
                sfxSource.Stop();
            }
            sfxSource.Play();
        }

        base.Interact();
    }
}
