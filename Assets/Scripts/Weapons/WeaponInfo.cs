using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewWeaponInfo", menuName = "Custom/Weapon Info")]
public class WeaponInfo : ScriptableObject
{
    [SerializeField] private string weaponName;
    [SerializeField] private ushort ammoClipSize = 10;

    public string WeaponName { get { return weaponName; } }
    public ushort AmmoClipSize { get { return ammoClipSize; } }
}
