using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Spedomete : MonoBehaviour
{
[SerializeField] float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
