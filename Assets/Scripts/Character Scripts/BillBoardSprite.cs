using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BillBoardSprite : MonoBehaviour
{
    private Camera cam;
    private SpriteRenderer spr;

    [SerializeField] private bool lockX, lockY = true, lockZ;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();       
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnEnable()
    {
        GameMaster.Instance.OnChangeMainCam += SwitchMainCamera;
    }

    private void OnDisable()
    {
        if (!GameMaster.IsInstanceNull)
        {
            GameMaster.Instance.OnChangeMainCam -= SwitchMainCamera;
        }
    }

    private void LateUpdate()
    {
        Vector3 cameraDir = cam.transform.forward;
        if (lockX) cameraDir.x = 0f;
        if (lockY) cameraDir.y = 0f;
        if (lockZ) cameraDir.z = 0f;
        transform.rotation = Quaternion.LookRotation(cameraDir);
    }

    private void SwitchMainCamera(Camera cam)
    {
        this.cam = cam;
    }
}
