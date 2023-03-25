using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private float stepFrequency = .5f;
    [SerializeField] private List<AudioClip> footstepSFX = new List<AudioClip>();

    private CharacterController cc;
    private Player player;
    private AudioSource source;

    private float stepCycle = 0.0f;


    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (footstepSFX.Count == 0)
        {
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if(player.State == PlayerState.Walking || player.State == PlayerState.Decelerating)
        {
            stepCycle += player.HorizontalVel.magnitude * Time.deltaTime;
            if(stepCycle > stepFrequency)
            {
                PlayFootStep();
                stepCycle = 0.0f;
            }
        }
        else
        {
            stepCycle = 0.0f;
        }
    }

    private void PlayFootStep()
    {
        int n = Random.Range(0, footstepSFX.Count);

        source.clip = footstepSFX[n];
        source.PlayOneShot(source.clip);
    }
}
