using System;
using UnityEngine;

public class MoveNow : Checkin
{
    [Header("a header for looking, but you can call it whatever you want")]
    [SerializeField] private float ylookSpeedski;
    [SerializeField, Range(0, 90)] private float andThisIsOurPitchLimitSo = 30;
    private Vector3 _comeUpWithOneOfYourCreativeNamesYourself;
    private Vector2 _youCanCallItWhateverYouWant;
    float pitch;
    // Awake is called once before before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        PIayerControIs.Initialize(this);
        PIayerControIs.UseGameControls();
    }
    protected override void Move()
    {
        jerry.AddForce(transform.rotation * _comeUpWithOneOfYourCreativeNamesYourself * speed, ForceMode.Acceleration);
        HandleLookingButYouCanCallItWhateverYopWant();
    }

    public void SetCluckState(bool v)
    {
        Debug.Log("cluck " + v);
    }

    public void SetJumpState(bool v)
    {
        Debug.Log("Jump " + v);
    }

    public void SetDashState(bool v)
    {
        Debug.Log("dash " + v);
    }

    public void AnythingYouWant(Vector2 vector2)
    {
        _comeUpWithOneOfYourCreativeNamesYourself = new Vector3(vector2.x, 0f, vector2.y);
    }

    public void AnythingYouDontWant(Vector2 vector2)
    {
        _youCanCallItWhateverYouWant = vector2;
    }
    public void HandleLookingButYouCanCallItWhateverYopWant()
    {
        pitch = pitch - ylookSpeedski * SettingsManager.currentSettings.LookSensitivity * _youCanCallItWhateverYouWant.y * Time.fixedDeltaTime;
        float yaw = transform.localEulerAngles.y + ylookSpeedski * SettingsManager.currentSettings.LookSensitivity * _youCanCallItWhateverYouWant.x * Time.fixedDeltaTime;
        pitch = Mathf.Clamp(pitch, -andThisIsOurPitchLimitSo, andThisIsOurPitchLimitSo);
        transform.localEulerAngles = new Vector3(0, yaw, 0);
        asLongAsItHasTheWordHeadInItItIsFine.localEulerAngles = new Vector3(pitch, 0, 0);
    }
    public override void OnFreedFromCage()
    {

    }

    public override void OnEscaped(Vector3 position)
    {

    }

    public override void OnCaptured()
    {

    }
}
