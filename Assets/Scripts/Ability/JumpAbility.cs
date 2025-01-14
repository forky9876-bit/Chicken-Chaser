using UnityEngine;
using Utilities;

//Forces us to have a RigidBody on us
[RequireComponent(typeof(Rigidbody))]
public class JumpAbility : AbstractAbility 
{
    [Header("Jump")] 
    [SerializeField] private float jumpForce;

    private Rigidbody _physicsBody;

    private void Awake()
    {
        //We need access to a rigidbody to jump
        _physicsBody = GetComponent<Rigidbody>();
    }

    protected override void Activate()
    {
        //Apply upwards velocity to ourselves. Impulse means instant
        _physicsBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    protected override int AbilityTriggerID()
    {
        return StaticUtilities.JumpAnimID;
    }

    public override bool CanActivate()
    {
        //We can only just if we're on the floor
        return Owner.GetIsGrounded() && base.CanActivate();
    }
}
