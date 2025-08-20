using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Spedomete : MonoBehaviour
{
    [SerializeField] Checkin checkin;
    [SerializeField] Quaternion q;
    [SerializeField] Vector3 v;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = quaternion.Euler(0, 0, checkin.currentSpeed/checkin.maxSpeed*3);
    }
}
