using System;
using Managers;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class PlayerChicken : Chicken
{
    private Vector3 _moveDirection;
    private Vector2 _lookDirection;
    
    [Header("Looking")] 
    [SerializeField , Range(0,90)] private float pitchLimit;
    [SerializeField, Range(0,180)] private float yawLimit;
    [SerializeField] private float lookSpeed;

    [Header("Abilities")]
    [SerializeField] private AbstractAbility jumpAbility;
    [SerializeField] private AbstractAbility cluckAbility;
    [SerializeField] private AbstractAbility dashAbility;

    [Header("Effects")]
    [SerializeField] private GameObject lossCam;
    
    //Observable player events
    public static Action<Vector3> OnPlayerCaught;
    public static Action<Vector3> OnPlayerEscaped;
    public static Action OnPlayerRescued; // It's not out of the picture that another chicken can rescue the player.
    
    protected override void Awake()
    {
        base.Awake();
        HudManager.Instance.BindPlayer(this);
        PlayerControls.Initialize(this);
        PlayerControls.UseGameControls();
    }

    private void OnEnable()
    {
        //Enable components for good measure
        PhysicsBody.isKinematic = false;
        BodyCollider.enabled = true;
        
        SettingsManager.SaveFile.onLookSenseChanged += OnLookSensChanged;
        lookSpeed = SettingsManager.currentSettings.LookSensitivity;
    }

    private void OnDisable()
    {
        
        //Disable components for good measure
        PhysicsBody.isKinematic = true;
        BodyCollider.enabled = false;
        
        PlayerControls.DisablePlayer();  
        jumpAbility.ForceCancelAbility();
        cluckAbility.ForceCancelAbility();
        dashAbility.ForceCancelAbility();
        
        SettingsManager.SaveFile.onLookSenseChanged -= OnLookSensChanged;
    }

    private void OnLookSensChanged(float val)
    {
        lookSpeed = val;
    }

    protected override void HandleMovement()
    {
        Vector3 direction = _moveDirection;
        if (IsGrounded)
        {
            //If we're grounded, then the direction we want to move should be projected onto the plane.
            //Doing this will help us move up steep slopes easier.
            direction = Vector3.ProjectOnPlane(_moveDirection, slopeNormal);
        }
            
        PhysicsBody.AddForce(transform.rotation * direction * stats.Speed, ForceMode.Acceleration);

        //Note: we don't care about falling speed, only XZ speed.
        Vector2 horizontalVelocity = new Vector2(PhysicsBody.linearVelocity.x, PhysicsBody.linearVelocity.z);
        currentSpeed = horizontalVelocity.magnitude; 

        //Check if our speed is exceeding the max speed
        if (currentSpeed > stats.MaxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * stats.MaxSpeed;
            //Limit the speed, but be sure to keep the gravity speed.
            PhysicsBody.linearVelocity = new Vector3(horizontalVelocity.x, PhysicsBody.linearVelocity.y, horizontalVelocity.y);
            
            //Lock the speed to prevent weird bugs
            currentSpeed = stats.MaxSpeed;
        }
        
        HandleLooking();
    }

    public override void OnFreedFromCage()
    {
        enabled = true;
        PlayerControls.UseGameControls();
        OnPlayerRescued?.Invoke();

        //Stop Using the cluck ability
        cluckAbility.StopUsingAbility();

        lossCam.SetActive(false);
    }

    public override void OnEscaped(Vector3 position)
    {
        //Optional Debug
        Debug.Log("Player won the game!");
        
        //Notify anyone who wants to know when we escape
        OnPlayerEscaped?.Invoke(transform.position);
        
        //Disable the controls
        PlayerControls.DisableControls();
        
        //Create an AI to take over for us
        NavMeshAgent agent = gameObject.AddComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.baseOffset = 0.16f;
        agent.height = 0.32f;
        agent.radius = 0.2f;
        agent.agentTypeID = 0;
        agent.SetDestination(position);
        
        //Enable the animations
        AnimatorController.SetFloat(StaticUtilities.MoveSpeedAnimID, stats.MaxSpeed);

        //Prevent any weirdness
        enabled = false;
        
        GameManager.PlayUISound(stats.EscapedSound);
    }

    public override void OnCaptured()
    {
        //Optional debug line
        Debug.Log("Player has been captured");
        
        //Fix the players hopping animations
        AnimatorController.SetFloat(StaticUtilities.MoveSpeedAnimID, 0);
        cluckAbility.StartUsingAbility(); //Cluck to bring all chickens to you (done later)

        //Unlock the mouse and make it visible
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
        //Make humans ignore us
        Visibility = 0;
        
        //Notify our subscribers (?) if we have any
        OnPlayerCaught?.Invoke(transform.position);
        
        lossCam.SetActive(true);
        
        GameManager.PlayUISound(stats.CaughtSound);
    }

    public void SetDashState(bool state)
    {
        if(state) dashAbility.StartUsingAbility();
        else dashAbility.StopUsingAbility();
    }

    public void SetCluckState(bool state)
    {
        if(state) cluckAbility.StartUsingAbility();
        else cluckAbility.StopUsingAbility();
    }

    public void SetJumpState(bool state)
    {
        if(state) jumpAbility.StartUsingAbility();
        else jumpAbility.StopUsingAbility();
    }

    public void SetMoveDirection(Vector2 direction)
    {
        //In unity, Y is up, so we need to convert to vector3, and have WS affect the forward (Z) axis.
        _moveDirection = new Vector3(direction.x, 0, direction.y);
    }

    public void SetLookDirection(Vector2 direction)
    {
        _lookDirection = direction;
    }

    private void HandleLooking()
    {
        //Caching the Time.deltaTime is important if you're using it more than once. It saves RAM.
        float timeShift = Time.deltaTime;
        float pitchChange = head.localEulerAngles.x - lookSpeed * _lookDirection.y * timeShift;
        float yawChange = transform.localEulerAngles.y + lookSpeed * _lookDirection.x * timeShift;
        
        //Apply limits so we don't Gimbal Lock ourselves
        // (Quaternion rotation would correct this but this does the job)
        if (pitchChange > pitchLimit && pitchChange < 180) pitchChange = pitchLimit;
        else if (pitchChange < 360-pitchLimit && pitchChange > 180) pitchChange = -pitchLimit;
        if (yawChange > yawLimit && yawChange < 180) yawChange = yawLimit;
        else if (yawChange < 360-yawLimit && yawChange > 180) yawChange = -yawLimit;

        //Apply the modifications to each part, be sure to use LOCAL euler angles, so that other systems work correctly.
        transform.localEulerAngles = new Vector3(0, yawChange, 0);
        head.localEulerAngles = new Vector3(pitchChange, 0, 0);
    }

    public AbstractAbility GetCluckAbility()
    {
        return cluckAbility;
    }
    
    public AbstractAbility GetJumpAbility()
    {
        return jumpAbility;
    }
    
    public AbstractAbility GetDashAbility()
    {
        return dashAbility;
    }
}
