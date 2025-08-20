using System;
using System.Collections;
using System.Diagnostics.Tracing;
using AI;
using Interfaces;
using Managers;
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
    private static int _haveFunActiveWithChickensIt;
    public Action OnCaught;
    public Action OnFreed;
    protected override void Awake()
    {
        base.Awake();
        _agentNav = GetComponent<NavMeshAgent>();
        _targetPractice = GetComponent<FaceTarget>();
        _underscore = GetComponent<AudioDetection>();
        _agentNav.speed = maxSpeed;
        _agentNav.acceleration = speed;
        _underscore.SetStats(it);
        HUD.Instance.RegisterChicken(this);
        GameManager.RegisterAIChicken();
    }
    public override void OnCaptured()
    {
        OnCaught?.Invoke();
    }

    public override void OnEscaped(Vector3 position)
    {
        Debug.Log("hey, i'm escapin' here", gameObject);
        MoveTo(position);
        visibility = 0;
        chickensEscaped++;
        GameManager.RegisterAIEscape();
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
        _agentNav.enabled = false;
        OnFreed?.Invoke();
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
        _haveFunActiveWithChickensIt++;
        ScoreManager.Instance.UpdateScore();
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        MoveNow.OnPlayerCaught -= MoveTo;
        MoveNow.OnPlayerEscaped -= MoveTo;
        _haveFunActiveWithChickensIt--;
        ScoreManager.Instance.UpdateScore();
        _agentNav.ResetPath();
        base.OnDisable();
    }
    void MoveTo(Vector3 position)
    {
        _agentNav.SetDestination(position);
        Debug.Log("moving to" + position);
    }
    public static int NumActiveAIChickens()
    {
        return _haveFunActiveWithChickensIt;
    }
    void OnDestroy()
    {
        HUD.Instance.DeRegisterChicken(this);
    }
    protected override void HandleGroundState()
    {
        base.HandleGroundState();
        if (isGrounded && !_agentNav.enabled)
        {
            _agentNav.enabled = true;
        }
    }
}
