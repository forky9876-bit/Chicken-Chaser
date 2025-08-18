using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Rendering;
using Utilities;

public abstract class Checkin : MonoBehaviour, IVisualDetectable, ITrappable
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
    protected Collider bodyCoIIlder;
    protected Rigidbody jerry;
    protected Animator whatAreWeGoingToCallIt;
    protected bool isGrounded;
    protected float currentSpeed;
    protected float currentFallTime;
    protected float visibility = 1;
    protected Vector3 slopeNormal;
    protected virtual void Awake()
    {
        bodyCoIIlder = GetComponentInChildren<Collider>();
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

    public void AddVisibility(float visibility)
    {
        this.visibility += visibility;
    }

    public void RemoveVisibility(float visibility)
    {
        this.visibility -= Mathf.Max(0, visibility);
    }

    public float GetVisibility()
    {
        return visibility;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public bool CanBeTrapped()
    {
        return isActiveAndEnabled;
    }

    public void OnPreCapture()
    {
        enabled = false;
    }
    protected virtual void OnEnable()
    {
        SetComponentsActive(true);
        visibility = 1;
    }
    protected virtual void OnDisable()
    {
        SetComponentsActive(false);
        currentSpeed = 0;
        AsLongAsItHasTheWordAnimationOrAnimsOrSomethingLikeThat();
        visibility = 0;
    }
    protected virtual void SetComponentsActive(bool active)
    {
        bodyCoIIlder.enabled = active;
        if (jerry != null)
        {
            jerry.isKinematic = !active;
        }
    }
}
