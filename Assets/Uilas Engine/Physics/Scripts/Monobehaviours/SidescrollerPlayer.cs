using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class SidescrollerPlayer : PlayerBase
{
    #region Properties

    // Public Properties
    [Header("Sidescroller Properties")]
    public float jumpHeight = 3f;
    public bool smoothFallEnabled = true;
    public float maxSmoothFallVelocity = 1;
    public float accelerationTimeOnAir = 0.2f;
    public float waitingTimeForGround = 1;
    public int jumpCount = 1;
    public bool wallJumpEnabled = false;
    public float wallJumpForce = 3f;
    public float wallJumpTime = 0.5f;
    public float wallSlidingVelocity = 1f;
    public bool canPushObjects = false;
    public float pushVelocityMultiplayer = 0.1f;
    public const float ignoreSemiCollisionTime = 0.15f;
    //public float Direction { get { return spriteRenderer.flipX ? -1 : 1; } }

    // Private Properties
    private Vector2 input;
    private float jumpForce;
    private int jumpsLeft;
    private float jumpTimeLeft;
    private bool onWall;
    private bool pushingObject;
    private bool playingAnimation;
    private Coroutine waitingForGround;

    // References
    public static SidescrollerActions actions;

    #endregion

    #region Execution

    protected void OnEnable()
    {
        SetActions();
    }

    public override void Start()
    {
        base.Start();

        jumpForce = Mathf.Sqrt(jumpHeight * -Physics2D.gravity.y * 2);
        body.beforeMove += PushObjects;
    }

    public virtual void Update()
    {
        HandleSmoothFall();
        HandleJumpCount();
        HandleMovement();
        HandleAnimations();
        HandleWallJump();
    }

    protected void OnDisable()
    {
        DisableActions();
    } 

    #endregion

    #region Actions

    protected void SetActions()
    {
        actions = new SidescrollerActions();

        actions.Gameplay.Enable();

        //actions.Gameplay.Move.started += OnMoveStart;
        actions.Gameplay.Move.canceled += OnMoveStop;

        actions.Gameplay.Move.performed += OnMovePerformed;

        actions.Gameplay.Jump.performed += OnJumpPerformed;
        actions.Gameplay.Jump.canceled += OnJumpCanceled;

        actions.Gameplay.Down.started += OnDownStart;

        actions.Gameplay.PrimaryItem.performed += OnPrimaryItemPerformed;

        actions.Gameplay.Restart.performed += OnRestartPerformed;
    }

    protected void DisableActions()
    {
        actions.Gameplay.Disable();
    }

    //protected virtual void OnMoveStart(InputAction.CallbackContext _context)
    //{
    //    horizontalInput = _context.ReadValue<Vector2>().x;
    //}

    protected virtual void OnMovePerformed(InputAction.CallbackContext _context)
    {
        input = _context.ReadValue<Vector2>();
    }

    protected virtual void OnMoveStop(InputAction.CallbackContext _context)
    {
        input = Vector2.zero;
    }

    protected virtual void OnJumpPerformed(InputAction.CallbackContext _context)
    {
        if (state != PlayerState.Enabled)
        {
            return;
        }

        if (input.y > -0.75f)
        {
            if (jumpsLeft > 0)
            {
                Jump();
            }
            else
            {
                if (waitingForGround != null)
                {
                    StopCoroutine(waitingForGround);
                }

                waitingForGround = StartCoroutine(WaitForGroundToJump());
            }
        }
        else
        {
            StartCoroutine(IgnoreSemiCollision(ignoreSemiCollisionTime));
        }
    }

    protected virtual void OnJumpCanceled(InputAction.CallbackContext _context)
    {
        if (body.Velocity.y > 0 && !onWall)
        {
            body.Velocity = new Vector2(body.Velocity.x, body.Velocity.y * 0.5f);
        }
    }

    protected virtual void OnDownStart(InputAction.CallbackContext _context)
    {
        StartCoroutine(IgnoreSemiCollision(ignoreSemiCollisionTime));
    }

    protected virtual void OnPrimaryItemPerformed(InputAction.CallbackContext _context)
    {

    }

    protected virtual void OnRestartPerformed(InputAction.CallbackContext _context)
    {

    }

    #endregion

    #region Public Methods

    public void SetState(PlayerState _state, float _time = -1)
    {
        if (_time > 0)
        {
            StartCoroutine(WaitingToChangeState(_state, _time));
        }
        else
        {
            state = _state;
        }
    }

    public void SetAnimation(string _animationName, float _duration)
    {
        StopCoroutine("WaitingAnimation");
        StartCoroutine(WaitingAnimation(_animationName, _duration));
    }


    #endregion

    #region Private Methods

    private void HandleSmoothFall()
    {
        if (smoothFallEnabled && !onWall)
        {
            bool falling = body.Velocity.y < 0;

            if (falling && body.Velocity.y > -maxSmoothFallVelocity)
            {
                body.GravityMultiplier = 0.5f;
            }
            else
            {
                body.GravityMultiplier = 1f;
            }
        }
    } 

    private void HandleJumpCount()
    {
        if (body.GroundedOvertime || onWall)
        {
            // if the Player was recently grounded, reset it's jump count
            jumpsLeft = jumpCount;
        }
        else if (jumpsLeft == jumpCount)
        {
            // otherwise, it miss one jump
            jumpsLeft -= 1;
        }
    }

    private void HandleMovement()
    {
        if (state == PlayerState.Enabled)
        {
            Vector2 _targetVelocity = new Vector2(input.x * moveSpeed, body.Velocity.y);
            float _accelerationTime = body.Grounded ? accelerationTime : accelerationTimeOnAir;

            if (input.x != 0 && body.Grounded)
                direction = new Vector2(input.x > 0? 1f : -1f, 0f);

            body.Velocity = Vector2.SmoothDamp(body.Velocity, _targetVelocity, ref acceleration, _accelerationTime);
        }
    }

    private void HandleWallJump()
    {
        if (!wallJumpEnabled || input.x == 0 || state != PlayerState.Enabled)
        {
            onWall = false;
            return;
        }

        float _distance = 0.1f;
        onWall = !body.Grounded && body.CheckDistance(Vector2.right * Direction, ref _distance);

        if (onWall && body.Velocity.y < 0)
        {
            body.Velocity = new Vector2(body.Velocity.x, -wallSlidingVelocity);
        }
    }

    private void HandleAnimations()
    {
        bool _grounded = body.Grounded;

        if (state == PlayerState.Enabled)
        {
            spriteRenderer.flipX = Direction.x < 0;
        }

        if (playingAnimation)
            return;

        if (_grounded)
        {
            if (Mathf.Abs(input.x) <= 0.05f)
            {
                animator.Play("Idle");
            }
            else
            {
                if (pushingObject)
                {
                    animator.Play("Pushing");
                }
                else
                {
                    animator.Play("Running");
                }
            }
        }
        else
        {
            if (body.Velocity.y > 0)
            {
                animator.Play("Jumping");
            }
            else if (onWall)
            {
                animator.Play("Wall Sliding");
            }
            else
            {
                animator.Play("Falling");
            }
        }
    }

    private void Jump()
    {
        float _horizontalSpeed = 0;

        if (wallJumpEnabled && onWall)
        {
            _horizontalSpeed = -Direction.x * moveSpeed * wallJumpForce;
            SetState(PlayerState.Disabled);
            SetState(PlayerState.Enabled, wallJumpTime);
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        if (jumpsLeft > 0) jumpsLeft -= 1;

        // Prevents the Player to jump multiple times after leaving the ground
        body.GroundedOvertime = false;

        body.Velocity = new Vector2(body.Velocity.x + _horizontalSpeed, jumpForce);
    }

    private void PushObjects(Vector2 _velocity)
    {
        if (state != PlayerState.Enabled || _velocity.y != 0 || _velocity.x == 0 || canPushObjects == false || body.Grounded == false || input.x == 0)
        {
            return;
        }

        Vector2 _direction = _velocity.normalized;
        float _distance = _velocity.magnitude;
        RaycastHit2D _hit = new RaycastHit2D();

        if (body.GetCollisor(_direction, _distance, ref _hit))
        {
            Body _objectBody = _hit.collider.gameObject.GetComponent<Body>();

            if (_objectBody && _objectBody.pushable && _objectBody.Grounded)
            {
                pushingObject = true;
                _objectBody.Move(_velocity * pushVelocityMultiplayer);
            }
            else
            {
                pushingObject = false;
            }

        }
        else
        {
            pushingObject = false;
        }
    }

    private IEnumerator WaitForGroundToJump()
    {
        float _time = waitingTimeForGround;

        while (!body.Grounded)
        {
            _time -= Time.deltaTime;

            if (_time <= 0)
            {
                yield break;
            }

            yield return null;
        }

        Jump();
    }

    private IEnumerator IgnoreSemiCollision(float _duration)
    {
        body.ignoreSemiCollision = true;

        yield return new WaitForSeconds(_duration);

        body.ignoreSemiCollision = false;
    }

    #endregion

    #region Enumerators

    private IEnumerator WaitingToChangeState(PlayerState _state, float _sec)
    {
        yield return new WaitForSeconds(_sec);

        SetState(_state);
    }

    private IEnumerator WaitingAnimation(string _animationName, float _duration)
    {
        playingAnimation = true;
        animator.Play(_animationName);

        yield return new WaitForSeconds(_duration);

        playingAnimation = false;
    }

    #endregion
}