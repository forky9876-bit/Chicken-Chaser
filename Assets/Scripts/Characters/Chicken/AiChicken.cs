using System;
using AI;
using Interfaces;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Characters.Chicken
{
    public class AiChicken : Chicken, IDetector
    {
        private NavMeshAgent _agent;
        private FaceTarget _faceTarget;
        private AudioDetection _audioDetection;

        [SerializeField] private HearStats activeHearing;
        [SerializeField] private HearStats inactiveHearing;
        

        protected override void Awake()
        {
            base.Awake();
            _agent = GetComponent<NavMeshAgent>();
            _faceTarget = GetComponent<FaceTarget>();
            _audioDetection = GetComponent<AudioDetection>();

            _agent.acceleration = stats.MoveSpeed;
            _agent.speed = stats.MaxSpeed;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _faceTarget.enabled = false;
            _agent.enabled = true;
            animator.enabled = true;
            _audioDetection.SetStats(activeHearing);
            animator.SetBool(StaticUtilities.CluckAnimID, true);


            PlayerChicken.OnPlayerCaught += MoveTo;
            PlayerChicken.OnPlayerEscaped += MoveTo;
        }

        private void MoveTo(Vector3 obj)
        {
            _agent.SetDestination(obj);
        }

        protected override void OnDisable()
        {
            _agent.ResetPath();

            base.OnDisable();
            _faceTarget.enabled = true;
            _agent.enabled = false;
            animator.enabled = false;
            _audioDetection.SetStats(inactiveHearing);
            
            
            PlayerChicken.OnPlayerCaught -= MoveTo;
            PlayerChicken.OnPlayerEscaped -= MoveTo;
        }


        protected override void HandleMoving(float deltaTime)
        {
            
        }

        public override float GetCurrentSpeed()
        {
            return _agent.velocity.magnitude;
        }

        public void AddDetection(Vector3 location, float detection, EDetectionType type)
        {
            if (!enabled || detection < 1) return;
            print("AI Move" + location);
            _agent.SetDestination(location);
            animator.SetBool(StaticUtilities.CluckAnimID, false);

        }
    }
}
