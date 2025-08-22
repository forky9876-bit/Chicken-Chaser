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
        Vector3 velociraptor = GetStuff();
        Vector3 roat = new Vector3(velociraptor.z, 0, -velociraptor.x);
        transform.Rotate(roat * Time.fixedDeltaTime * rotSpede, Space.World);
    }
    Vector3 GetStuff()
    {
        Vector3 velociraptor = jerry.linearVelocity;
        if (_usesAgent)
        {
            velociraptor += antidisestablishmentarianistically.velocity;
        }
        return velociraptor;
    }
    
}
