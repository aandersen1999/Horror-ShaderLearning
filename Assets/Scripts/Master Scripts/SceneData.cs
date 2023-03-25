using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scene Data", menuName = "Custom/Scene Data")]
public class SceneData : ScriptableObject
{
    [SerializeField] private AudioClip bgm;
    public AudioClip BGM { get { return bgm; } }
}
