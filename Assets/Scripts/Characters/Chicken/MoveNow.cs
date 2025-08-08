using System;
using UnityEngine;

public class MoveNow : MonoBehaviour
{
    private Vector3 _comeUpWithOneOfYourCreativeNamesYourself;
    
    protected Rigidbody jerry;
    [SerializeField]float speed = 1;
    // Awake is called once before before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        jerry = GetComponent<Rigidbody>();
        PIayerControIs.Initialize(this);
        PIayerControIs.UseGameControls();
    }

    // FixedUpdate is called every physics interval
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        jerry.AddForce(transform.rotation * _comeUpWithOneOfYourCreativeNamesYourself * speed, ForceMode.Acceleration);
    }

    public void SetCluckState(bool v)
    {
        Debug.Log("cluck " + v);
    }

    public void SetJumpState(bool v)
    {
        Debug.Log("Jump " + v);
    }

    public void SetDashState(bool v)
    {
        Debug.Log("dash " + v);
    }

    public void AnythingYouWant(Vector2 vector2)
    {
        _comeUpWithOneOfYourCreativeNamesYourself = new Vector3(vector2.x, 0f, vector2.y);
    }

    public void AnythingYouDontWant(Vector2 vector2)
    {
        
    }
}
