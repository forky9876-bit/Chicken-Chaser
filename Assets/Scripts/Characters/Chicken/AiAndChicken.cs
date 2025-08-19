using System.Collections;
using System.Diagnostics.Tracing;
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
    public static int chickensEscaped;
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
        Debug.Log("hey, i'm escapin' here", gameObject);
        MoveTo(position);
        visibility = 0;
        chickensEscaped++;
        StartCoroutine(CheckForEscape());

    }
    private IEnumerator CheckForEscape()
    {
        WaitUntil waitUntil;
        waitUntil = new WaitUntil(() => _agentNav.hasPath && _agentNav.remainingDistance <= _agentNav.stoppingDistance);
        yield return waitUntil;
        Destroy(gameObject);
    }

    public override void OnFreedFromCage()
    {
        enabled = true;
    }

    protected override void Move()
    {
        currentSpeed = Mathf.Max(_agentNav.remainingDistance - _agentNav.stoppingDistance + .2f, 0);
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
    protected override void SetComponentsActive(bool active)
    {
        base.SetComponentsActive(active);
        whatAreWeGoingToCallIt.SetBool(StaticUtilities.CluckAnimID, active);
        whatAreWeGoingToCallIt.enabled = active;
        _targetPractice.enabled = !active;
        _agentNav.enabled = active;

    }
    protected override void OnEnable()
    {
        MoveNow.OnPlayerCaught += MoveTo;
        MoveNow.OnPlayerEscaped += MoveTo;
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        MoveNow.OnPlayerCaught -= MoveTo;
        MoveNow.OnPlayerEscaped -= MoveTo;
        _agentNav.ResetPath();
        base.OnDisable();
    }
    void MoveTo(Vector3 position)
    {
        _agentNav.SetDestination(position);
        Debug.Log("moving to" + position);
    }
}
