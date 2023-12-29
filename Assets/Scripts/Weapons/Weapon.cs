using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ushort clip;

    [SerializeField] private WeaponInfo weaponInfo;

    private Transform camTrans;

    private void OnEnable()
    {
        camTrans = Camera.main.transform;
    }

    protected virtual void FireWeapon(LayerMask mask)
    {
        clip--;

        if(Physics.Raycast(camTrans.position, camTrans.forward, out RaycastHit hit, Mathf.Infinity, mask))
        {

        }
    }

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