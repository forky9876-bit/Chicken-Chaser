using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Spedomete : MonoBehaviour
{
    [SerializeField] Checkin checkin;
    [SerializeField] float min = -180;
    [SerializeField] float max = 180;
    [SerializeField] float offset = 90;

    // Update is called once per frame
    void Update()
    {
        float percentage = checkin.currentSpeed / checkin.maxSpeed;
        float angle = Mathf.Lerp(-min, -max, percentage) + offset;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
