using UnityEngine;

public class ROTATE : MonoBehaviour
{
    [SerializeField] float spede;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.Rotate(Vector3.up, spede*Time.deltaTime, Space.World);
    }
}
