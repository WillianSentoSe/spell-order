using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SidescrollerPlayerInputController : BaseInputController
{
    protected PlayerInputActions actions;
    protected SidescrollerPlayer player;
    protected Vector2 leftAxis;

    public Vector2 LeftAxis { get { return leftAxis; } }
    public PlayerInputActions Actions { get { return actions; } }

    public SidescrollerPlayerInputController(SidescrollerPlayer player)
    {
        this.player = player;
    }

    protected override void Init()
    {
        actions = new PlayerInputActions();
    }

    protected override void SetActions()
    {
        actions.Gameplay.Move.started += OnMoveStart;
        actions.Gameplay.Move.canceled += OnMoveStop;
        actions.Gameplay.Move.performed += OnMovePerformed;
        actions.Gameplay.Jump.performed += OnJumpPerformed;
        actions.Gameplay.Jump.canceled += OnJumpCanceled;
        actions.Gameplay.Down.started += OnDownStart;
    }

    protected override void OnEnable()
    {
        Actions.Gameplay.Enable();
    }

    protected override void OnDisable()
    {
        Actions.Gameplay.Disable();
    }

    protected virtual void OnMoveStart(InputAction.CallbackContext _context)
    {

    }

    protected virtual void OnMovePerformed(InputAction.CallbackContext _context)
    {
        leftAxis = _context.ReadValue<Vector2>();
    }

    protected virtual void OnMoveStop(InputAction.CallbackContext _context)
    {
        leftAxis = Vector2.zero;
    }

    protected virtual void OnJumpPerformed(InputAction.CallbackContext _context)
    {
        player.Jump();
    }

    protected virtual void OnJumpCanceled(InputAction.CallbackContext _context)
    {
        player.CancelJump();
    }

    protected virtual void OnDownStart(InputAction.CallbackContext _context)
    {
        player.StartCoroutine(player.IgnoreSemiCollision());
    }
}
