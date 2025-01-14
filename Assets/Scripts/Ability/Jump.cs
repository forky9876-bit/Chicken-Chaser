using System;
using UnityEngine;
using Utilities;

namespace Ability
{
    public class Jump : Ability
    {
        
        [SerializeField] private float jumpForce = 10;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        protected override void ActivateAbility()
        {
            AnimationController.SetTrigger(StaticUtilities.JumpAnimID);
        }

        protected override void ExecuteAbility()
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        public override bool CanActivate()
        {
            return base.CanActivate() && Owner.IsGrounded;
        }
    }
}
