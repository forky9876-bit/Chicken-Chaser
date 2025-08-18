using System;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class MoveNow : Checkin
{
    [Header("a header for looking, but you can call it whatever you want")]
    [SerializeField] private float ylookSpeedski;
    [SerializeField, Range(0, 90)] private float andThisIsOurPitchLimitSo = 30;
    private Vector3 _comeUpWithOneOfYourCreativeNamesYourself;
    private Vector2 _youCanCallItWhateverYouWant;
    public static Action OnPlayerRescued;
    public static Action<Vector3> OnPlayerCaught;
    public static Action<Vector3> OnPlayerEscaped;
    float pitch;
    [Header("hwat are we gonna call it?")]
    [SerializeField] private AbstractAbility jumpAbility;
    [SerializeField] private AbstractAbility dashAbility;
    [SerializeField] private AbstractAbility cluckAbility;
    [SerializeField] private AbstractAbility glihAbility;
    // Awake is called once before before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        HUD.Instance.BindPlayer(this);
        PIayerControIs.Initialize(this);
        PIayerControIs.UseGameControls();
    }
    protected override void Move()
    {
        Vector3 direction = _comeUpWithOneOfYourCreativeNamesYourself;
        if (isGrounded)
        {
            direction = Vector3.ProjectOnPlane(direction, slopeNormal);
        }
        jerry.AddForce(transform.rotation * direction * speed, ForceMode.Acceleration);

        Vector2 horizontalVelocity = new Vector2(jerry.linearVelocity.x, jerry.linearVelocity.z);
        currentSpeed = horizontalVelocity.magnitude;
        if (currentSpeed > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            jerry.linearVelocity = new Vector3(horizontalVelocity.x, jerry.linearVelocity.y, horizontalVelocity.y);
            currentSpeed = maxSpeed;
        }

        HandleLookingButYouCanCallItWhateverYopWant();
    }

    public void SetCluckState(bool v)
    {
        if (v)
        {
            cluckAbility.StartAbility();
        }
        else
        {
            cluckAbility.StopAbility();
        }
    }

    public void SetJumpState(bool v)
    {
        if (v)
        {
            if (isGrounded)
            {
                jumpAbility.StartAbility();
            }
            else
            {
                glihAbility.StartAbility();
            }
        }
        else
        {
            jumpAbility.StopAbility();
            glihAbility.StopAbility();
        }
    }

    public void SetDashState(bool v)
    {
        if (v)
        {
            dashAbility.StartAbility();
        }
        else
        {
            dashAbility.StopAbility();
        }
    }

    public void AnythingYouWant(Vector2 vector2)
    {
        _comeUpWithOneOfYourCreativeNamesYourself = new Vector3(vector2.x, 0f, vector2.y);
    }

    public void AnythingYouDontWant(Vector2 vector2)
    {
        _youCanCallItWhateverYouWant = vector2;
    }
    public void HandleLookingButYouCanCallItWhateverYopWant()
    {
        pitch = pitch - ylookSpeedski * SettingsManager.currentSettings.LookSensitivity * _youCanCallItWhateverYouWant.y * Time.fixedDeltaTime;
        float yaw = transform.localEulerAngles.y + ylookSpeedski * SettingsManager.currentSettings.LookSensitivity * _youCanCallItWhateverYouWant.x * Time.fixedDeltaTime;
        pitch = Mathf.Clamp(pitch, -andThisIsOurPitchLimitSo, andThisIsOurPitchLimitSo);
        transform.localEulerAngles = new Vector3(0, yaw, 0);
        asLongAsItHasTheWordHeadInItItIsFine.localEulerAngles = new Vector3(pitch, 0, 0);
    }
    public override void OnFreedFromCage()
    {
        OnPlayerRescued?.Invoke();
        cluckAbility.StopAbility();
    }

    public override void OnEscaped(Vector3 position)
    {
        OnPlayerEscaped?.Invoke(transform.position);
        print("yey win");
        NavMeshAgent agingt;
        agingt = gameObject.AddComponent<NavMeshAgent>();
        agingt.enabled = true;
        agingt.baseOffset = .16f;
        agingt.height = .32f;
        agingt.radius = .2f;
        agingt.agentTypeID = 0;
        agingt.SetDestination(position);
        whatAreWeGoingToCallIt.SetFloat(StaticUtilities.MoveSpeedAnimID, maxSpeed);
        enabled = false;
    }

    public override void OnCaptured()
    {
        Debug.Log("Oh no, what ever are we going to do");
        cluckAbility.StartAbility();
        OnPlayerCaught?.Invoke(transform.position);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        PIayerControIs.DisablePIayer();
        jumpAbility.ForceCancelAbility();
        dashAbility.ForceCancelAbility();
        cluckAbility.ForceCancelAbility();
        glihAbility.ForceCancelAbility();
    }
    public AbstractAbility GetClucking()
    {
        return cluckAbility;
    }
    public AbstractAbility GetToJumping()
    {
        return jumpAbility;
    }
    public AbstractAbility ADashOfPepper()
    {
        return dashAbility;
    }
    public AbstractAbility GetGLIH()
    {
        return glihAbility;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        PIayerControIs.UseGameControls();
    }
}
