using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;

    public static string GetBulletTypeName(BulletType type)
    {
        switch(type)
        {
            case BulletType.LightPistol:
                return "9mm Ammo";
            case BulletType.HeavyPistol:
                return ".45 Ammo";
            case BulletType.Shotgun:
                return "12 gauge";
            default:
                return "Unknown";
        }
    }
}

public enum BulletType : byte
{
    LightPistol,
    HeavyPistol,
    Shotgun,
}

public struct WeaponArgs
{

}