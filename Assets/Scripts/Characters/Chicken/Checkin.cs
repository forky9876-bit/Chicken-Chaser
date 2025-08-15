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
    [SerializeField] protected Transform andThisOneIsGoingToBeForOurFeet;
    protected Rigidbody jerry;
    protected Animator whatAreWeGoingToCallIt;
    protected bool isGrounded;
    protected float currentSpeed;
    protected float currentFallTime;
    protected Vector3 slopeNormal;
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
        bool didWeHitSomethingIDontKnow = Physics.SphereCast(andThisOneIsGoingToBeForOurFeet.position, footRadius, Vector3.down, out RaycastHit slope, footDistance, StaticUtilities.GroundLayers);
        if (didWeHitSomethingIDontKnow != isGrounded)
        {
            isGrounded = didWeHitSomethingIDontKnow;
            whatAreWeGoingToCallIt.SetBool(StaticUtilities.IsGroundedAnimID, isGrounded);
            if (currentFallTime > 0)
            {
                HandleLanding();
                currentFallTime = 0;
            }
        }
        if (isGrounded)
        {
            slopeNormal = slope.normal;
        }
        else
        {
            currentSpeed += Time.fixedDeltaTime;
        }
    }
    protected virtual void HandleLanding()
    {

    }
    protected virtual void AsLongAsItHasTheWordAnimationOrAnimsOrSomethingLikeThat()
    {
        whatAreWeGoingToCallIt.SetFloat(StaticUtilities.MoveSpeedAnimID, currentSpeed);
    }
    public abstract void OnFreedFromCage();
    public abstract void OnEscaped(Vector3 position);
    public abstract void OnCaptured();
    public bool GetIsGrounded()
    {
        return isGrounded;
    }
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
    public Vector3 GetLookDirection()
    {
        return asLongAsItHasTheWordHeadInItItIsFine.forward;
    }
}
