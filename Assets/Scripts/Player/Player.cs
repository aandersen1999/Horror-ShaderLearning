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

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public Vector2 intendedVel = Vector2.zero;
    private Vector2 horizontalVel = Vector2.zero;
    public Vector2 HorizontalVel { get { return horizontalVel; } }
    private float verticalVel = -10.0f;
    private float intendedMag = 0.0f;
    private PlayerState state = PlayerState.Idle;
    public PlayerState State { get { return state; } }

    private const float magDeadZone = .05f;

    private CharacterController cc;

    private InputActions control;
    private InputActions.PlayerMapActions actions;
    [SerializeField]private Interactable interactObject;
    public Interactable InteractObject { get { return interactObject; } }

    public event Action OnFoundInteractable;
    public event Action OnLostInteractable;

    #region Monobehavior
    private void Awake()
    {
        control = new InputActions();
        actions = control.PlayerMap;
    }

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        Assert.IsNotNull(cam);
        //Assert.IsNotNull(eyeSight);
        Assert.IsNotNull(cc);

        cam.fieldOfView = fov;
        camTransform = cam.transform;

        GameMaster.Instance.SetPlayer(this);
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (control != null)
        {
            control.Enable();
        }
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        if (control != null)
        {
            control.Disable();
        }
    }

    private void OnDestroy()
    {
        if (control != null)
        {
            control.Dispose();
        }
        OnFoundInteractable = null;
        OnLostInteractable = null;
    }

    private void Update()
    {
        CheckForObject();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(interactObject != null)
            {
                interactObject.Interact();
            }
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

    private void OnValidate()
    {
        cam.fieldOfView = fov;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(camTransform.position, camTransform.forward * checkObjectRange);
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * snapToGroundRange);

    }
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
        vel = Vector2.MoveTowards(vel, Vector2.zero, acceleration * Time.deltaTime);
        vel = Vector2.MoveTowards(vel, intendedVel, (acceleration * 2) * Time.deltaTime);
        SetHVel(vel);
    }

    private bool CheckCommonGroundCancels()
    {
        if (actions.Jump.triggered)
        {
            SetPlayerState(PlayerState.Jumping);
            return true;
        }
        if (!SnapToGround(out float dist))
        {
            SetPlayerState(PlayerState.FreeFall);
            return true;
        }

        verticalVel = -dist / Time.deltaTime;
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
            //GameMath.ApproachDelta(horizontalVel, Vector2.zero, acceleration);

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

        horizontalVel = Vector2.MoveTowards(horizontalVel, intendedAir, (acceleration / 4) * Time.deltaTime);
            //GameMath.ApproachDelta(horizontalVel, intendedAir, acceleration / 4);

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
        int layermask = 1 << 6;
        bool sawInteractableLastFrame = interactObject != null;

        interactObject = null;

        if (Physics.Raycast(camTransform.position, camTransform.TransformDirection(Vector3.forward), out RaycastHit hit, checkObjectRange, layermask))
        {
            if(hit.collider.gameObject != null && hit.collider.gameObject.TryGetComponent(out Interactable interact))
            {
                interactObject = interact;
                if (!sawInteractableLastFrame)
                    OnFoundInteractable?.Invoke();
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

    private bool SnapToGround(out float distance)
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, snapToGroundRange))
        {
            distance = hit.distance;
            return true;
        }
        distance = 0.0f;
        return false;
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