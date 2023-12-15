using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : Singleton<ControllerManager>
{
    public InputActions Controller { get; private set; }
    public InputActions.PlayerMapActions ControllerActions { get; private set; }

    private new void Awake()
    {
        base.Awake();
        Controller = new();
        ControllerActions = Controller.PlayerMap;
    }

    private void OnEnable()
    {
        Controller?.Enable();
    }

    private void OnDisable()
    {
        Controller?.Disable();
    }

    private void OnDestroy()
    {
        Controller?.Dispose();
    }

    public void LockCursor(bool lockCursor)
    {
        Cursor.lockState = (lockCursor) ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
