using UnityEngine;

public class RotateTowardsVelocity : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float threshold = .2f;
    [SerializeField] float deltaSpeed = 200f;
    [SerializeField] Vector3 rotationMultipliers = new Vector3(1f, 0f, 1f);

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        if (rb == null)
        {
            enabled = false;
        }
    }

    void Update()
    {
        Quaternion targetRotation = transform.rotation;

        Vector3 vel = Vector3.Scale(rb.linearVelocity, rotationMultipliers);
        if (vel.magnitude > threshold)
        {
            targetRotation = Quaternion.LookRotation(vel, Vector3.up);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, deltaSpeed * Time.deltaTime);
    }
}
