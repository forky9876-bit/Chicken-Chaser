using System;
using Managers;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Characters.Chicken
{
    public class PlayerChicken : Chicken
    {
        private Vector3 _moveDirection;
        private Vector2 _lookDirection;
        
        [Header("Looking")] 
        [SerializeField, Range(0, 20)] private float yawSpeed = 20f;
        [SerializeField, Range(0, 20)] private float pitchSpeed = 20f;
        [SerializeField, Range(0, 90)] private float maxPitch;

        [SerializeField] private Ability.Ability[] abilities;

        public static Action<Vector3> OnPlayerCaught;
        public static Action<Vector3> OnPlayerEscaped;
        public static Action OnPlayerRescued;

        private float _oldVisibility;
        private float _currentSpeed;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Awake()
        {
            base.Awake();
            GameControls.Init(this);
        }

        private void Start()
        {
            HUDManager.Instance.BindAbilities(abilities);
        }


        // Update is called once per frame
        void LateUpdate()
        {
            HandleLooking(Time.deltaTime);
        }

    
        public void Move(Vector2 readValue)
        {
            _moveDirection = new Vector3(readValue.x, 0f, readValue.y);
        }

        public void Look(Vector2 readValue)
        {
            _lookDirection = readValue;
        }
        
        public void Cluck(bool readValueAsButton)
        {
            abilities[0].InputStateChanged(readValueAsButton);
        }


        public void Dash(bool readValueAsButton)
        {
            abilities[1].InputStateChanged(readValueAsButton);

        }

        public void Jump(bool readValueAsButton)
        {
            abilities[2].InputStateChanged(readValueAsButton);

        }

        private void HandleLooking(float deltaTime)
        {
            float currentPitch = head.localEulerAngles.x;
            float currentYaw = transform.localEulerAngles.y;

            currentPitch += _lookDirection.y * pitchSpeed * deltaTime;
            currentYaw += _lookDirection.x * yawSpeed * deltaTime;

            if (currentPitch > maxPitch && currentPitch < 180) currentPitch = maxPitch;
            else if (currentPitch < 360 - maxPitch && currentPitch > 180) currentPitch = 360 - maxPitch;

            head.localEulerAngles = new Vector3(currentPitch, 0, 0);
            transform.localEulerAngles = new Vector3(0, currentYaw, 0);
        }

        protected override void HandleMoving(float deltaTime)
        {
            Vector3 direction = _moveDirection;
            if (IsGrounded)
            {
                direction = Vector3.ProjectOnPlane(direction, Vector3.up);
            }

            rb.AddForce(transform.rotation * direction * stats.MoveSpeed, ForceMode.Acceleration);
            
            LimitSpeed();
        }

        private void LimitSpeed()
        {
            Vector2 horizontalVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z);
            _currentSpeed = horizontalVelocity.magnitude;
            if (_currentSpeed >= stats.MaxSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * stats.MaxSpeed;
                rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.y);
                _currentSpeed = horizontalVelocity.magnitude;
            }
        }



        public override float GetCurrentSpeed() => _currentSpeed;


        protected override void OnEnable()
        {
            base.OnEnable();
            rb.isKinematic = false;
            bodyCollider.enabled = true;
            GameControls.EnableGame();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            rb.isKinematic = true;
            bodyCollider.enabled = false;
            
            GameControls.DisablePlayer();

            foreach (Ability.Ability ability in abilities)
            {
                ability.ForceCancelAbility();
            }
        }

        public override void OnCaptured()
        {
            base.OnCaptured();
            OnPlayerCaught?.Invoke(transform.position);
            
            animator.SetFloat(StaticUtilities.MoveSpeedAnimID,0);
            abilities[0].InputStateChanged(true);

            _oldVisibility = Visibility;
            
            Visibility = 0;
        }

        public override void OnFreedFromCage()
        {
            base.OnFreedFromCage();
            OnPlayerRescued?.Invoke();
            abilities[0].InputStateChanged(false);
            Visibility = _oldVisibility;
        }

        public override void OnEscaped(Vector3 position)
        {
            base.OnEscaped(position);
            OnPlayerEscaped?.Invoke(position);

            NavMeshAgent agent = gameObject.AddComponent<NavMeshAgent>();
            agent.SetDestination(position);
            enabled = false;

        }
    }
}
