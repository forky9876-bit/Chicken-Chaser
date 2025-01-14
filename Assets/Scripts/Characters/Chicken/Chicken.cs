using System;
using Interfaces;
using ScriptableObjects;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Characters.Chicken
{
    public abstract class Chicken : MonoBehaviour, ITrappable, IVisualDetectable
    {
        protected Animator animator;
        protected Rigidbody rb;
        
        [SerializeField] protected Transform head;
        [SerializeField] protected Transform foot;
        [SerializeField] protected ChickenStats stats;

        [SerializeField] private ParticleSystem landEffect;
        protected Collider bodyCollider;
        
        private ChickenAnimatorReceiver _animatorReceiver;

        protected float Visibility = 1;

        protected float airTime;
        
        public bool IsGrounded { get; private set; }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            _animatorReceiver = GetComponentInChildren<ChickenAnimatorReceiver>();
            _animatorReceiver.OnLandEffect += OnLanded;
        }

        // Update is called once per frame
        void Update()
        {
            HandleAnimations();
        }

        private void FixedUpdate()
        {
            float dt = Time.deltaTime;
            GroundCheck(dt);
            HandleMoving(dt);
        }


        protected abstract void HandleMoving(float deltaTime);

        protected virtual void HandleAnimations()
        {
            animator.SetFloat(StaticUtilities.MoveSpeedAnimID, GetCurrentSpeed());
            animator.SetBool(StaticUtilities.IsGroundedAnimID, IsGrounded);
        }

        public abstract float GetCurrentSpeed();

        private void GroundCheck(float deltaTime)
        {
            bool currentState = Physics.SphereCast(foot.position, stats.FootRadius, Vector3.down, out RaycastHit hit, stats.FootDistance,  StaticUtilities.GroundLayers);

            if (!currentState)
            {
                airTime += deltaTime;
            }

            if (currentState != IsGrounded)
            {
                IsGrounded = currentState;
                _animatorReceiver.OnLandEffect.Invoke(airTime);
                airTime = 0;
            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = IsGrounded?Color.green : Color.red;
            
            GizmosExtras.DrawWireSphereCast(foot.position , Vector3.down, 
                stats.FootDistance, stats.FootRadius);
        }
        
        public Vector3 GetDirection() => head.forward;

        public Transform GetTransform()
        {
            return transform;
        }

        public virtual bool CanBeTrapped()
        {
            return true;
        }

        public virtual void OnCaptured()
        {
           
        }

        public virtual void OnFreedFromCage()
        {
            enabled = true;
        }

        public virtual void OnPreCapture()
        {
            enabled = false;
        }

        public void AddVisibility(float visibility)
        {
            Visibility += visibility;
        }

        public void RemoveVisibility(float visibility)
        {
            Visibility = Mathf.Max(0, Visibility - visibility);
        }

        public float GetVisibility()
        {
            return Visibility;
        }


        private void OnLanded(float strength)
        {
            landEffect.emission.SetBurst(0,  new ParticleSystem.Burst()
            {
                count = Mathf.Clamp(Random.Range(5,15) * strength, 5, 50)
            });
            var main = landEffect.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.5f, Mathf.Clamp(strength / 5, 1, 2));
            landEffect.Play();
        }

        private void OnDestroy()
        {
            _animatorReceiver.OnLandEffect -= OnLanded;
        }

        public virtual void OnEscaped(Vector3 position)
        {
            
        }

        protected virtual void OnEnable()
        {
            bodyCollider ??= GetComponentInChildren<Collider>();
            bodyCollider.enabled = true;
        }

        protected virtual void OnDisable()
        {
            bodyCollider.enabled = false;
        }

    }
}
