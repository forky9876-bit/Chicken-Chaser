using UnityEngine;
using Utilities;
[RequireComponent(typeof(Rigidbody))]
public class JumpAbility : AbstractAbility
{
    [Header("Jump or whatever")]
    [SerializeField] private float jumpingForce;
    private Rigidbody jerry;
    void Awake()
    {
        jerry = GetComponent<Rigidbody>();
    }
    protected override void Activate()
    {
        jerry.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
    }
    public override bool CanActivate()
    {
        return base.CanActivate() && owner.GetIsGrounded();
    }
    protected override int AbilityTriggerID()
    {
        return StaticUtilities.JumpAnimID;
    }
}
