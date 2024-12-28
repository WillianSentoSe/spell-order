using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerInputController : SidescrollerPlayerInputController
{
    public PlayerInputController(Player player) : base(player)
    {
        this.player = player;
        actions = new PlayerInputActions();
    }

    protected override void SetActions()
    {
        base.SetActions();

        actions.Gameplay.PrimaryItem.performed += OnPrimaryItemPerformed;
        actions.Gameplay.Restart.performed += OnRestartPerformed;
    }

    protected void DisableActions()
    {
        actions.Gameplay.Disable();
    }

    protected virtual void OnPrimaryItemPerformed(InputAction.CallbackContext _context)
    {
        //inventory?.CastSpell();
    }

    protected virtual void OnRestartPerformed(InputAction.CallbackContext _context)
    {
        GameManager.main.RestartGame();
    }
}
