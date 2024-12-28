using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

[RequireComponent(typeof(Body))]
public class IceBlock : MonoBehaviour
{
    #region Properties

    // Constants
    private const float gravityMultiplier = 0.75f;
    private const float instableTime = 1f;
    private const float skinWidth = 0.05f;
    private const float shakeStrenght = 0.0312f;

    // Public properties
    public float maxAirTime = 4f;
    public Animator animator;
    public GameObject startParticlePrefab;
    public GameObject destroyEffect;
    public LayerMask BreakWhenAbove;

    // Private properties
    private float airTime;
    private Body body;
    private ShakeEffect shakeEffect;

    #endregion

    #region Execution

    public void Start()
    {
        // Setting references
        body = GetComponent<Body>();
        body.GravityMultiplier = 0f;

        shakeEffect = GetComponent<ShakeEffect>();

        // Starting timer
        airTime = maxAirTime;

        // Creating start particles
        Instantiate(startParticlePrefab, transform.position, Quaternion.identity);

        // Checking for collision
        CheckForCollision();
    }

    public void Update()
    {
        if (!body.Grounded)
        {
            if (airTime > 0)
            {
                airTime -= Time.deltaTime;

                if (!shakeEffect.IsShaking && airTime <= instableTime)
                    shakeEffect.Shake(shakeStrenght, instableTime, false);
            }
            else
            {
                if (shakeEffect.IsShaking) shakeEffect.Stop();
                body.GravityMultiplier = gravityMultiplier;
            }

        }

        CheckForCollisionBelow();
    }

    #endregion

    #region Public Methods

    public void DestroyBlock()
    {
        if (destroyEffect) Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    #endregion

    #region Private Methods

    private void CheckForCollision()
    {
        // Setting parameters
        BoxCollider2D _collider = GetComponent<BoxCollider2D>();
        LayerMask _mask = Physics2D.GetLayerCollisionMask(gameObject.layer);
        Vector2 _size = _collider.size - new Vector2(skinWidth, skinWidth);

        // Disabling the collider. It prevent the collision with itself
        _collider.enabled = false;

        // Checking for collision
        Collider2D _hit = Physics2D.OverlapBox(transform.position, _size, 0f, _mask);

        // Re-enabling the collider
        _collider.enabled = true;

        if (_hit)
        {
            DestroyBlock();
        }
    }

    private void CheckForCollisionBelow()
    {
        if (body.Velocity.y >= 0) return;

        float _distance = 0.003f;
        RaycastHit2D _hit = new RaycastHit2D();

        if (body.GetCollisor(Vector2.down, _distance, ref _hit, BreakWhenAbove, true))
        {
            Player _player = _hit.collider.GetComponent<Player>();

            if (_player)
            {
                _player.Kill();
            }
            else
            {
                DestroyBlock();
            }
        }
    }

    #endregion
}