using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFacingDirection : MonoBehaviour
{
    [SerializeField] private SpritesFacings sprites;

    private SpriteRenderer spr;
    private Camera cam;

    private FacingDirection currentFacingDir = FacingDirection.Front;

    private void Start()
    {
        spr = GetComponentInChildren<SpriteRenderer>();
        cam = Camera.main;
        spr.sprite = sprites.Front;
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
        FigureOutDirection(GetAngle());
    }

    private float GetAngle()
    {
        Vector2 locationDifference = new(transform.position.x - cam.transform.position.x, transform.position.z - cam.transform.position.z);
        locationDifference.Normalize();
        Vector2 facingDirection = new(transform.forward.x, transform.forward.z);
        return Vector2.SignedAngle(facingDirection, locationDifference);
    }

    private void FigureOutDirection(float angle)
    {
        float abs = Mathf.Abs(angle);
        FacingDirection newDirection;

        if(abs > 157.5f || abs < 22.5f)
        {
            if(abs > 157.5f) { newDirection = FacingDirection.Back; }
            else { newDirection = FacingDirection.Front; }
        }
        else if(Mathf.Sign(angle) < 0)
        {
            if(abs < 67.5f) { newDirection = FacingDirection.FrontLeft; }
            else if(abs < 112.5f) { newDirection = FacingDirection.Left; }
            else { newDirection = FacingDirection.BackLeft; }
        }
        else
        {
            if (abs < 67.5f) { newDirection = FacingDirection.FrontRight; }
            else if (abs < 112.5f) { newDirection = FacingDirection.Right; }
            else { newDirection = FacingDirection.BackRight; }
        }

        if(currentFacingDir != newDirection)
        {
            spr.sprite = GetSprite(newDirection);
            currentFacingDir = newDirection;
        }
    }

    private Sprite GetSprite(FacingDirection dir)
    {
        switch (dir)
        {
            case FacingDirection.Back:
                return sprites.Back;

            case FacingDirection.BackLeft:
                return sprites.BackRight;

            case FacingDirection.BackRight:
                return sprites.BackLeft;

            case FacingDirection.Front:
                return sprites.Front;

            case FacingDirection.FrontRight:
                return sprites.FrontLeft;

            case FacingDirection.FrontLeft:
                return sprites.FrontRight;

            case FacingDirection.Left:
                return sprites.Right;

            case FacingDirection.Right:
                return sprites.Left;

            default:
                return null;
        }
    }

    private void SwitchMainCamera(Camera cam)
    {
        this.cam = cam;
    }
}

[System.Serializable]
public struct SpritesFacings
{
    [field: SerializeField] public Sprite Back { get; private set; }
    [field: SerializeField] public Sprite BackLeft { get; private set; }
    [field: SerializeField] public Sprite BackRight { get; private set; }
    [field:SerializeField] public Sprite Front { get; private set; }
    [field: SerializeField] public Sprite FrontLeft { get; private set; }
    [field:SerializeField] public Sprite FrontRight { get; private set; }
    [field: SerializeField] public Sprite Left { get; private set; }
    [field: SerializeField] public Sprite Right { get; private set; }
}

public enum FacingDirection : byte
{
    Back,
    BackLeft,
    BackRight,
    Front,
    FrontLeft,
    FrontRight,
    Left,
    Right
}
