using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public event Action<WeaponState> OnWeaponStateChange;

    private InputActions controller;
    private InputActions.PlayerMapActions actions;
    private WeaponState state;
    [SerializeField] private Weapon heldWeapon;
    [SerializeField] private LayerMask weaponMask;
    public LayerMask WeaponMask { get { return weaponMask; } }

    [SerializeField] private Light muzzleFlash;
    [SerializeField] private float muzzleFlashIntensity;

    private float muzzleFlashDecay;

    private void Awake()
    {
        muzzleFlashDecay = muzzleFlashIntensity * 6.6f;
    }

    private void Start()
    {
        controller = ControllerManager.Instance.Controller;
        actions = controller.PlayerMap;
        muzzleFlash.intensity = 0.0f;
    }

    private void Update()
    {
        float intensity = muzzleFlash.intensity;
        intensity = Mathf.MoveTowards(intensity, 0.0f, muzzleFlashDecay * Time.deltaTime);
        muzzleFlash.intensity = intensity;

        switch (state)
        {
            case WeaponState.Idle:
                if(actions.Fire.triggered)
                {
                    muzzleFlash.intensity = muzzleFlashIntensity;
                }
                break;
            default:
                break;
        }
    }

    private void SwitchState(WeaponState state)
    {
        this.state = state;
        OnWeaponStateChange?.Invoke(state);
    }
}

public enum WeaponState : byte
{
    Idle,
    Firing,
    Reload,
}