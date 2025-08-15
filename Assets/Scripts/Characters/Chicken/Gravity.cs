using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float baseGravityScale = 1;
    float _currentGravity = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //* We don't want to use the rigidbody's gravity, instead we use our own.
        rb.useGravity = false;
    }

    public void FixedUpate_MultiplyGravity(float multiplier)
    {
        _currentGravity *= multiplier;
    }

    void FixedUpdate()
    {
        //force.force = Physics.gravity * baseGravityScale;
        rb.AddForce(Physics.gravity * baseGravityScale * _currentGravity, ForceMode.Acceleration);
        _currentGravity = 1f;
    }
}
