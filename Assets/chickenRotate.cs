using UnityEngine;
using UnityEngine.AI;

public class chickenRotate : MonoBehaviour
{
    Rigidbody jerry;
    NavMeshAgent antidisestablishmentarianistically;
    private bool _usesAgent;
    [SerializeField] float rotSpede = 270;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        jerry = GetComponentInParent<Rigidbody>();
        antidisestablishmentarianistically = GetComponentInParent<NavMeshAgent>();
        _usesAgent = antidisestablishmentarianistically;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 roat = new Vector3(jerry.linearVelocity.z, 0, -jerry.linearVelocity.x);
        transform.Rotate(roat * Time.fixedDeltaTime * rotSpede, Space.World);
    }
    Vector3 GetStuff()
    {
        return _usesAgent ? antidisestablishmentarianistically.velocity : jerry.linearVelocity;
    }
    
}
