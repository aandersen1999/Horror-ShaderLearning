using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    [Header("Player Stat Attributes")]
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float stamina = 100.0f;
    [SerializeField] private float staminaDrain = 14.0f;
    [SerializeField] private float staminaGain = 14.0f;

    [SerializeField] private float walkSpeed = 8.0f;
    [SerializeField] private float runSpeed = 16.0f;
    [SerializeField] private float jumpHeight = 12.0f;
    [SerializeField] private float acceleration = 32.0f;

    [SerializeField] private bool lockMovement = false;
    [SerializeField] private float gravity = 15.0f;
    [SerializeField] private float terminalGravityVel = -15.0f;

    [Header("Camera Attributes")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform camTransform;
    [SerializeField, Range(60.0f, 100.0f)] private float fov = 60.0f;
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float maxLookAngle = 85.0f;

    [SerializeField] private bool lockCamera = false;

    [Header("Raycast Ranges")]
    [SerializeField] private float snapToGroundRange = 5.0f;
    [SerializeField] private float checkObjectRange = 3.0f;
    [SerializeField] private LayerMask checkObjectsMask;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public Vector2 intendedVel = Vector2.zero;
    private Vector2 horizontalVel = Vector2.zero;
    public Vector2 HorizontalVel { get { return horizontalVel; } }
    public Vector3 HorizontalVelV3 { get { return new Vector3(horizontalVel.x, 0, horizontalVel.y); } }
    private float verticalVel = -10.0f;
    private float intendedMag = 0.0f;
    private PlayerState state = PlayerState.Idle;
    public PlayerState State { get { return state; } }

    private const float magDeadZone = .05f;

    private CharacterController cc;
    private GroundData groundData;

    private InputActions control;
    private InputActions.PlayerMapActions actions;
    [SerializeField]private Interactable interactObject;
    [SerializeField] private Light flashLight;
    public Interactable InteractObject { get { return interactObject; } }

    public event Action<Interactable> OnFoundInteractable;
    public event Action<Interactable> OnInteractWithObject;
    public event Action OnLostInteractable;

    public event Action<float> OnHealthChanged;
    public event Action<PlayerState> OnStateChange;


    #region Monobehavior
    private void Awake()
    {
        control = new InputActions();
        actions = control.PlayerMap;
        groundData = GetComponent<GetGroundData>().Data;
    }

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        Assert.IsNotNull(cam);
        Assert.IsNotNull(cc);

        cam.fieldOfView = fov;
        camTransform = cam.transform;

        GameMaster.Instance.SetPlayer(this);
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        control?.Enable();
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        control?.Disable();
    }

    private void OnDestroy()
    {
        control?.Dispose();
        OnFoundInteractable = null;
        OnLostInteractable = null;

        if(!GameMaster.IsInstanceNull)
            GameMaster.Instance.PreviousPlayerInfo = new(health, flashLight.enabled);
    }

    private void Update()
    {
        CheckForObject();
        if (actions.Interact.triggered)
        {
            if(interactObject != null)
            {
                interactObject.Interact();
            }
        }
        if (actions.ToggleFlashLight.triggered)
        {
            flashLight.enabled = !flashLight.enabled;
        }


        AffectedGravity();

        Vector2 _move = actions.Move.ReadValue<Vector2>();
        Vector2 _look = actions.Look.ReadValue<Vector2>();

        if (!lockCamera)
        {
            yaw = transform.localEulerAngles.y + _look.x * mouseSensitivity;
            pitch -= _look.y * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = Vector3.up * yaw;
            camTransform.localEulerAngles = Vector3.right * pitch;
        }

        UpdateJoystick(_move);

        switch (state)
        {
            case PlayerState.Idle:
                IdleAction();
                break;
            case PlayerState.Walking:
                WalkingAction();
                break;
            case PlayerState.Decelerating:
                DecelerateAction();
                break;

            case PlayerState.FreeFall:
                FreeFallAction();
                break;
            case PlayerState.Jumping:
                JumpingAction();
                break;
            default:
                break;
        }

        cc.Move(CalcMovement() * Time.deltaTime);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        cam.fieldOfView = fov;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(camTransform.position, camTransform.forward * checkObjectRange);
    }
#endif
#endregion

    private void SetPlayerState(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.Jumping:
                SetYVelOnHSpeed(jumpHeight, .25f);
                break;

            default:
                break;
        }
        state = newState;
        OnStateChange?.Invoke(state);
    }

    private void SetYVelOnHSpeed(float yValue, float multiplier)
    {
        verticalVel = yValue + horizontalVel.magnitude * multiplier;
    }

    private void AffectedGravity()
    {
        verticalVel = (cc.isGrounded) ? -1.0f : Mathf.MoveTowards(verticalVel, terminalGravityVel, gravity * Time.deltaTime);
    }

    private Vector3 CalcMovement()
    {
        return new Vector3(horizontalVel.x, verticalVel, horizontalVel.y);
    }

    private void UpdateJoystick(Vector2 move)
    {
        intendedMag = move.SqrMagnitude();
        if (intendedMag < magDeadZone)
        {
            intendedMag = 0;
            intendedVel = Vector2.zero;
            return;
        }

        Vector3 output = new(move.x, 0, move.y);
        output = transform.TransformDirection(output) * walkSpeed;
        intendedVel = new Vector2(output.x, output.z);
    }

    private void SetHVel(Vector2 vel)
    {
        horizontalVel = vel;
    }

    #region ground
    private void UpdateWalking()
    {
        Vector2 vel = horizontalVel;
        float slopeMultiplier = SetSlopeSpeedMultiplier();
        float accel = acceleration;

        //Not actually what I want to do, but just for proof of concept stuff
        if (transform.position.y < -.25f)
        {
            slopeMultiplier *= .5f;
            accel *= .5f;
        }
            

        vel = Vector2.MoveTowards(vel, Vector2.zero, accel* Time.deltaTime);
        vel = Vector2.MoveTowards(vel, intendedVel * slopeMultiplier, (accel * 2) * Time.deltaTime);
        
        SetHVel(vel);
    }

    private bool CheckCommonGroundCancels()
    {
        if (actions.Jump.triggered)
        {
            SetPlayerState(PlayerState.Jumping);
            return true;
        }
        if (!groundData.hitThisFrame)
        {
            SetPlayerState(PlayerState.FreeFall);
            return true;
        }

        verticalVel = -groundData.distance / Time.deltaTime;
        return false;
    }

    private void IdleAction()
    {
        if (CheckCommonGroundCancels())
            return;

        if (intendedMag != 0)
        {
            SetPlayerState(PlayerState.Walking);
            return;
        }
    }

    private void WalkingAction()
    {
        if (CheckCommonGroundCancels())
            return;

        if (intendedMag == 0)
        {
            SetPlayerState(PlayerState.Decelerating);
            return;
        }

        UpdateWalking();
    }

    private void DecelerateAction()
    {
        if (CheckCommonGroundCancels())
            return;

        bool stopped = false;

        horizontalVel = Vector2.MoveTowards(horizontalVel, Vector2.zero, acceleration * Time.deltaTime);

        if (intendedMag != 0)
        {
            SetPlayerState(PlayerState.Walking);
        }

        if (horizontalVel == Vector2.zero)
        {
            stopped = true;
        }

        if (stopped)
        {
            SetPlayerState(PlayerState.Idle);
        }
        SetHVel(horizontalVel);
    }
    #endregion

    #region air
    private bool CheckCommonAirCancels()
    {
        if (cc.isGrounded)
        {
            if (intendedMag != 0)
            {
                SetPlayerState(PlayerState.Walking);
            }
            else
            {
                if (horizontalVel == Vector2.zero)
                {
                    SetPlayerState(PlayerState.Idle);
                }
                else
                {
                    SetPlayerState(PlayerState.Decelerating);
                }
            }
            return true;
        }
        return false;
    }

    private void UpdateAir()
    {
        Vector2 intendedAir = intendedVel.normalized * walkSpeed;

        horizontalVel = Vector2.MoveTowards(horizontalVel, intendedAir, (acceleration * .25f) * Time.deltaTime);

        SetHVel(horizontalVel);
    }

    private void FreeFallAction()
    {
        if (CheckCommonAirCancels())
            return;

        UpdateAir();
    }

    private void JumpingAction()
    {
        if (CheckCommonAirCancels())
        {
            return;
        }

        if (verticalVel < 0)
        {
            SetPlayerState(PlayerState.FreeFall);
            return;
        }

        UpdateAir();
    }
    #endregion

    #region raycast checks
    private void CheckForObject()
    {
        bool sawInteractableLastFrame = interactObject != null;
        Interactable previousFrameInteractable = interactObject;
        interactObject = null;

        if (Physics.Raycast(camTransform.position, camTransform.TransformDirection(Vector3.forward), out RaycastHit hit, checkObjectRange, checkObjectsMask))
        {
            if(hit.collider.gameObject != null && hit.collider.gameObject.TryGetComponent(out Interactable interact))
            {
                interactObject = interact;
                if (!sawInteractableLastFrame && interactObject != previousFrameInteractable)
                    OnFoundInteractable?.Invoke(interactObject);
            }
            else if (sawInteractableLastFrame)
            {
                OnLostInteractable?.Invoke();
            }
        }
        else if (sawInteractableLastFrame)
        {
            OnLostInteractable?.Invoke();
        }
    }

    private float GetGroundAngle()
    {
        Vector3 v3Hvel = new(horizontalVel.x, 0, horizontalVel.y);
        return Vector3.Angle(v3Hvel.normalized, groundData.normal);
    }

    private float SetSlopeSpeedMultiplier()
    {
        float angle = GetGroundAngle();

        float lerp = Mathf.InverseLerp(45, 135, angle);
        lerp = 1 - lerp;
        lerp += .5f;
        return lerp;
        
    }

    #endregion

    #region misc
    public void ForceSetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        //This is so dumb, but it does work and it's the only way I can think of to make it work...
        cc.enabled = false;
        transform.SetPositionAndRotation(pos, rot);
        cc.enabled = true;
    }
    #endregion

    public void SetPlayerInfo(PreviousPlayerInfo p)
    {
        health = p.Health;
        flashLight.enabled = p.FlashLightOn;
    }
}

public enum PlayerState : byte
{
    Idle,
    Walking,
    Decelerating,
    Crouching,
    Sliding,

    Jumping,
    FreeFall,


}

public struct PreviousPlayerInfo
{
    public float Health { get; private set; }
    public bool FlashLightOn { get; private set; }

    public PreviousPlayerInfo(float Health, bool FlashLightOn)
    {
        this.Health = Health;
        this.FlashLightOn = FlashLightOn;
    }
}