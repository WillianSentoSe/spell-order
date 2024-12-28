using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Body))]
public abstract class BasePlayer : MonoBehaviour
{
    #region Properties

    // Static Properties
    // public static PlayerBase main;

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
    public SpriteRenderer spriteRenderer;
    protected Body body;
    protected LayerMask pickableLayerMask;

    #endregion

    #region Getters and Setters

    public PlayerState State { get { return state; } set { state = value; } }
    public Vector2 Direction { get { return direction; } }

    #endregion

    protected virtual void Init() { }

    #region Execution

    private void Awake()
    {
        Init();
    }

    public virtual void Start()
    {
        body = GetComponent<Body>();

        pickableLayerMask = LayerMask.GetMask("Pickable");
        Transform _spriteTransform = transform.Find("Sprite");

        if (_spriteTransform)
        {
            spriteRenderer = _spriteTransform.GetComponent<SpriteRenderer>();
            animator = _spriteTransform.GetComponent<Animator>();
        }
    }

    public virtual void Update() {}

    #endregion

    public enum PlayerState
    {
        Enabled, Disabled, Dead
    }
}