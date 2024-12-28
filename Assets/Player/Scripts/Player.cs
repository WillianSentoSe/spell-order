using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : SidescrollerPlayer
{
    #region Properties

    public static int Layer = 8;
    public GameObject deathEffect;

    private SpellInventory inventory;
    private PlayerEventController eventController;
    //private new PlayerInputController inputController;

    #endregion

    #region Getters and Setters

    public PlayerEventController Events {
        get { return eventController; }
    }

    public SpellInventory Inventory { 
        get { return inventory; }
    }

    //public PlayerInputController Input {
    //    get { return inputController; }
    //}

    #endregion

    protected override void Init()
    {
        base.Init();

        inventory = GetComponent<SpellInventory>();
        eventController = new PlayerEventController(this);
        //inputController = new PlayerInputController(this);
    }

    #region Execution

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Start()
    {
        // Chamando Start do componente pai
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        eventController.HandleItemCollision(body, pickableLayerMask);
    }

    #endregion

    #region Actions

    #endregion

    #region Public Methods

    public void Kill()
    {
        if (state != PlayerState.Dead)
        {
            state = PlayerState.Dead;

            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            Time.timeScale = 0f;
            animator.Play("Dead");
            if (deathEffect) Instantiate(deathEffect, transform.position, Quaternion.identity);

            body.StopAllForces();

            eventController.OnDeath?.Invoke();
        }
    }

    #endregion
}
