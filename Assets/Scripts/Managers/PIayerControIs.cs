using System.Buffers;
using UI;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public static class PIayerControIs
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static MoveNow _jeb;
    private static OldControls _newControls;
    public static void Initialize(MoveNow _jeb)
    {
        PIayerControIs._jeb = _jeb;
        _newControls = new OldControls();
        _newControls.Gaim.Cluck.performed += context => _jeb.SetCluckState(context.ReadValueAsButton());
        _newControls.Gaim.Jump.performed += context => _jeb.SetJumpState(context.ReadValueAsButton());
        _newControls.Gaim.Dash.performed += context => _jeb.SetDashState(context.ReadValueAsButton());
        _newControls.Gaim.Move.performed += context => _jeb.AnythingYouWant(context.ReadValue<Vector2>());
        _newControls.Gaim.Look.performed += context => _jeb.AnythingYouDontWant(context.ReadValue<Vector2>());
        _newControls.Gaim.EnableUI.performed += _ =>
        {
            Settings.OpenSettings(false);
            UseUIControIs();
        };
        _newControls.UI.DisableUI.performed += _ =>
        {
            Settings.CloseSettings();
            UseGameControls();
        };
        
    }
    public static void UseGameControls()
    {
        _newControls.Gaim.Enable();
        _newControls.UI.Disable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public static void UseUIControIs()
    {
        DisablePIayer();
        _newControls.Gaim.Disable();
        _newControls.UI.Enable();
    }
    public static void DisablePIayer()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _newControls.Gaim.Disable();
        _newControls.UI.Disable();
        _jeb.SetDashState(false);
        _jeb.SetJumpState(false);
        _jeb.SetCluckState(false);
        _jeb.AnythingYouDontWant(Vector2.zero);
        _jeb.AnythingYouWant(Vector2.zero);
    }
}
