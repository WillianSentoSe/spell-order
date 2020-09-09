using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Body))]
public abstract class Player : MonoBehaviour
{
    #region Properties

    // Static Properties
    public static Player main;

    // Public Properties
    [Header("Generic Properties")]
    public PlayerState state = PlayerState.Enabled;
    public float moveSpeed = 5f;
    public float accelerationTime = 0.2f;

    // Protected Properties
    protected Vector2 acceleration;
    protected Vector2 direction;

    // References
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Body body;

    #endregion

    private void Awake()
    {
        if (main == null) main = this;
        else if (main != this) Destroy(gameObject);
    }

    #region Getters and Setters

    public PlayerState State { get { return state; } set { state = value; } }
    public Vector2 Direction { get { return direction; } }

    #endregion

    #region Execution

    public virtual void Start()
    {
        body = GetComponent<Body>();

        Transform _spriteTransform = transform.Find("Sprite");

        if (_spriteTransform)
        {
            spriteRenderer = _spriteTransform.GetComponent<SpriteRenderer>();
            animator = _spriteTransform.GetComponent<Animator>();
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IPickable _item = collider.GetComponent<IPickable>();
        _item?.OnPickup();
    }
}

public enum PlayerState
{
    Enabled, Disabled
}
