using System;
using NUnit.Framework.Constraints;
using UnityEngine;

public abstract class Checkin : MonoBehaviour
{
    [Header("theTitleForTheseTwoValues")]
    [SerializeField] protected float speed = Mathf.PI * 3f;
    [SerializeField] protected float maxSpeed = Mathf.PI * 3f;
    [Header("these are the values")]
    [SerializeField] protected float footRadius = 1;
    [SerializeField] protected float footDistance = 1;
    protected Rigidbody jerry;
    protected Animator whatAreWeGoingToCallIt;
    protected bool isGrounded;
    protected virtual void Awake()
    {
        jerry = GetComponent<Rigidbody>();
    }

    // FixedUpdate is called every physics interval
    void FixedUpdate()
    {
        Move();
    }
    protected abstract void Move();
}
