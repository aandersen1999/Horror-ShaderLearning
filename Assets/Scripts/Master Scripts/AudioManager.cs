using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public event Action OnBgmChange;

    [SerializeField] private AudioMixer mixer;
    public AudioMixer Mixer { get { return mixer; } }

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OverrideBGM(AudioClip bgm, OverrideType type=OverrideType.StopAndPlay)
    {
        if(bgm == null)
        {
            OnBgmChange?.Invoke();
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            return;
        }

        switch (type)
        {
            case OverrideType.StopAndPlay:
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                audioSource.clip = bgm;
                audioSource.Play();
                break;
            case OverrideType.StopAndHold:
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                audioSource.clip = bgm;
                break;
            default:
                break;
        }

        OnBgmChange?.Invoke();
    }
}

public enum OverrideType : byte
{
    StopAndPlay,
    StopAndHold,
    FadeOutAndPlay,
    FadeOutAndHold
}