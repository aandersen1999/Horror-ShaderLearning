using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }
    public AudioManager Audio_Manager { get; private set; }

    public AudioClip[] songs;
    private int counter = 0;

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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            Audio_Manager.OverrideBGM(songs[counter]);
            counter = counter == songs.Length - 1 ? 0 : counter + 1;
        }
    }
}
