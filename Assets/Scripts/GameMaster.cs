using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }
    public AudioManager Audio_Manager { get; private set; }

    [SerializeField] private AudioClip bgm;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        Audio_Manager = GetComponent<AudioManager>();
    }

    private void Start()
    {
        Audio_Manager.OverrideBGM(bgm);
    }


    private void Update()
    {
        
    }
}
