using Characters;
using Characters.Chicken;
using Unity.Cinemachine;
using UnityEngine;

public class WinEffects : MonoBehaviour
{

    private CinemachineCamera _cam;

    private void Awake()
    {
        _cam = GetComponent<CinemachineCamera>();
    }

    void OnEnable()
    {
        PlayerChicken.OnPlayerEscaped += OnGameWon;
    }

    private void OnDisable()
    {
        PlayerChicken.OnPlayerEscaped -= OnGameWon;
    }

    private void OnGameWon(Vector3 _)
    {
        _cam.enabled = true;
       // transform.GetChild(0).gameObject.SetActive(true);
    }
}
