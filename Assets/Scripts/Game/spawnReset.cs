using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnReset : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] sureenablere;
    private Vector3 _startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }
    public void Respawn()
    {
        transform.position = _startPos;
        foreach (MonoBehaviour sure in sureenablere)
        {
            sure.enabled = true;
        }
    }
}
