using System;
using UnityEngine;
using Utilities;

public abstract class Checkin : MonoBehaviour
{
    [Header("theTitleForTheseTwoValues")]
    [SerializeField] protected float speed = Mathf.PI * 3f;
    [SerializeField] protected float maxSpeed = Mathf.PI * 3f;
    [Header("these are the values")]
    [SerializeField] protected float footRadius = 1;
    [SerializeField] protected float footDistance = 1;
    [Header("objectses")]
    [SerializeField] protected Transform asLongAsItHasTheWordHeadInItItIsFine;
    protected Rigidbody jerry;
    protected Animator whatAreWeGoingToCallIt;
    protected bool isGrounded;
    protected virtual void Awake()
    {
        jerry = GetComponent<Rigidbody>();
        whatAreWeGoingToCallIt = GetComponentInChildren<Animator>();
    }

    // FixedUpdate is called every physics interval
    void FixedUpdate()
    {
        HandleGroundState();
        Move();
        AsLongAsItHasTheWordAnimationOrAnimsOrSomethingLikeThat();
    }
    protected abstract void Move();
    private void HandleGroundState()
    {

    }
    protected virtual void HandleLanding()
    {

    }
    protected virtual void AsLongAsItHasTheWordAnimationOrAnimsOrSomethingLikeThat()
    {
        whatAreWeGoingToCallIt.SetFloat(StaticUtilities.MoveSpeedAnimID, jerry.linearVelocity.magnitude);
    }
    public abstract void OnFreedFromCage();
    public abstract void OnEscaped(Vector3 position);
    public abstract void OnCaptured();
}
