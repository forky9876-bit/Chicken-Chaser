using Characters.Chicken;
using UnityEngine;

public class GameControls
{
    private static PlayerControls _playerControls;
    private static PlayerChicken _player;

    public static void Init(PlayerChicken player)
    {
        _playerControls = new PlayerControls();
        _player = player;
        
        _playerControls.Player.Cluck.performed += ctx => _player.Cluck(ctx.ReadValueAsButton());
        _playerControls.Player.Move.performed += ctx => _player.Move(ctx.ReadValue<Vector2>());
        _playerControls.Player.Look.performed += ctx => _player.Look(ctx.ReadValue<Vector2>());
        _playerControls.Player.Dash.performed += ctx => _player.Dash(ctx.ReadValueAsButton());
        _playerControls.Player.Jump.performed += ctx => _player.Jump(ctx.ReadValueAsButton());
        _playerControls.Player.Pause.performed += _ => EnableUI();
        
        _playerControls.UI.Unpause.performed += _ => EnableGame();
    }

    public static void EnableGame()
    {
        _playerControls.Player.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void EnableUI()
    {
        
    }

    public static void DisablePlayer()
    {
        
    }

}
