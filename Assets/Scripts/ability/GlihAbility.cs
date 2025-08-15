using UnityEngine;
using Utilities;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Gravity))]
public class GlihAbility : AbstractAbility
{
    [Header("What we are doing here")]
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float jumpingForce;
    private Gravity gary;
    private Rigidbody jerry;
    private bool _active;
    private bool _firstAirActivation;
    void FixedUpdate()
    {
        if (_active)
        {
            gary.FixedUpate_MultiplyGravity(gravityMultiplier);
            if (owner.GetIsGrounded())
            {
                StopAbility();
            }
        }
        if (owner.GetIsGrounded())
        {
            _firstAirActivation = true;
        }
    }
    void Awake()
    {
        gary = GetComponent<Gravity>();
        jerry = GetComponent<Rigidbody>();
    }
    protected override void Activate()
    {
        if (_firstAirActivation)
        {
            jerry.linearVelocity = new Vector3(jerry.linearVelocity.x, 0f, jerry.linearVelocity.z);
            jerry.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
        }
        _firstAirActivation = false;
        _active = true;
    }
    public override bool CanActivate()
    {
        return base.CanActivate() && !owner.GetIsGrounded();
    }
    protected override int AbilityBoolID()
    {
        return StaticUtilities.GlihAnimID;
    }
    protected override void Deactivate()
    {
        _active = false;
    }
}
