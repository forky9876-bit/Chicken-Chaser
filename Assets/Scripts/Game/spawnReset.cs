using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnReset : MonoBehaviour
{
    private Vector3 _startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            transform.position = _startPos;
        }
    }
}
