using System;
using System.Collections;
using UnityEngine;

namespace Ability
{
    public class Dash : Ability
    {
        [SerializeField] private float distance;
        [SerializeField] private float time;
        [SerializeField] private ParticleSystem feathers;

        private Vector3 _originalDirection;
        private Rigidbody _rb;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        protected override void ExecuteAbility()
        {
            
            print("Dash started");
            _originalDirection = Owner.GetDirection();
            _originalDirection.y = 0;
            _originalDirection.Normalize();
            StartCoroutine(DashTimer());
        }

        private IEnumerator DashTimer()
        {
            _rb.useGravity = false;
            float currentTime = 0;
            feathers.Play();
            _rb.linearVelocity = Vector3.zero;
            while (currentTime < time)
            {
                float t = Time.deltaTime;
                currentTime += t;
                _rb.linearVelocity = _originalDirection * (distance * t);
                //_rb.AddForce(_originalDirection * (distance), ForceMode.Acceleration);
                yield return null;
            }
            feathers.Stop();
            _rb.useGravity = true;
        }

        public override void ForceCancelAbility()
        {
            base.ForceCancelAbility();
            _rb.useGravity = true;
            feathers.Stop();
            StopAllCoroutines();
        }
    }
}
