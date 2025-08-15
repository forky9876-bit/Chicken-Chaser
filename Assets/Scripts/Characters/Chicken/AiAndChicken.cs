using AI;
using Interfaces;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class AiAndChicken : Checkin, IDetector
{
    private FaceTarget _targetPractice;
    private AudioDetection _underscore;
    private NavMeshAgent _agentNav;
    [SerializeField] private HearStats it;
    protected override void Awake()
    {
        base.Awake();
        _agentNav = GetComponent<NavMeshAgent>();
        _targetPractice = GetComponent<FaceTarget>();
        _underscore = GetComponent<AudioDetection>();
        _agentNav.speed = maxSpeed;
        _agentNav.acceleration = speed;
        _underscore.SetStats(it);
    }
    public override void OnCaptured()
    {

    }

    public override void OnEscaped(Vector3 position)
    {

    }

    public override void OnFreedFromCage()
    {

    }

    protected override void Move()
    {
        currentSpeed = Mathf.Max(_agentNav.remainingDistance-_agentNav.stoppingDistance + .2f, 0);
    }
    void OnValidate()
    {
        if (_agentNav != null)
        {
            _agentNav.speed = maxSpeed;
            _agentNav.acceleration = speed;
        }
    }

    public void AddDetection(Vector3 location, float detection, EDetectionType type)
    {
        if (!enabled || detection < 1f)
        {
            return;
        }
        _agentNav.SetDestination(location);
        whatAreWeGoingToCallIt.SetBool(StaticUtilities.CluckAnimID, false);
    }
}
