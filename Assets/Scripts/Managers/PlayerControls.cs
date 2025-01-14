using UI;
using UnityEngine;

public static class PlayerControls
{
    private static PlayerChicken _chicken;
    private static Controls _controls;

    public static void Initialize(PlayerChicken owner)
    {
        //Bind our owner
        _chicken = owner;
        
        //Create the controls object
        _controls = new Controls();
        
        //When the Dash button is pressed, read the value as a button (true for down, false for up)
        _controls.Game.Dash.performed += context => owner.SetDashState(context.ReadValueAsButton());
        _controls.Game.Cluck.performed += context => owner.SetCluckState(context.ReadValueAsButton());
        _controls.Game.Jump.performed += context => owner.SetJumpState(context.ReadValueAsButton());

        //Read the value as a type of Vector2
        _controls.Game.Move.performed += context => owner.SetMoveDirection(context.ReadValue<Vector2>());
        _controls.Game.Look.performed += context => owner.SetLookDirection(context.ReadValue<Vector2>());
        
        // _ means discard (We don't care about the input)
        _controls.Game.EnableUI.performed += _ =>
        {
            Settings.OpenSettings(false);
            UseUIControls();
        };
        _controls.UI.DisableUI.performed += _ =>
        {
            Settings.CloseSettings();
            UseGameControls();
        };
    }

    public static void UseGameControls()
    {
        //Enable game, disable ui
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _controls.Game.Enable();
        _controls.UI.Disable();
    }

    public static void UseUIControls()
    {
        //Disable the player, and the game, enable the UI
        DisablePlayer();
        _controls.Game.Disable();
        _controls.UI.Enable();
    }

    //This function is a safety function to prevent escaping from some menus
    public static void DisableControls()
    {
        _controls.Disable();
    }

    public static void DisablePlayer()
    {
        //Disable all controls
        _controls.UI.Disable();
        _controls.Game.Disable();
        
        //If we disable the controls, Unity will not longer check to see when we stop our input.
        //Therefore, we need to send a message to all our inputs as if we've let go of them if we need to disable the player.
        _chicken.SetCluckState(false);
        _chicken.SetDashState(false);
        _chicken.SetJumpState(false);
        _chicken.SetLookDirection(Vector2.zero);
        _chicken.SetMoveDirection(Vector2.zero);
    }
}
