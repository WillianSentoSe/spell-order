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

    // Public properties
    public float maxAirTime = 4f;
    public Animator animator;

    // Private properties
    private float airTime;
    private Body body;

    #endregion

    #region Execution

    public void Start()
    {
        // Setting references
        body = GetComponent<Body>();
        body.GravityMultiplier = 0f;

        // Starting timer
        airTime = maxAirTime;

        CheckForCollision();
    }

    public void Update()
    {
        if (!body.Grounded)
        {
            if (airTime > 0)
            {
                airTime -= Time.deltaTime;

                if (airTime <= instableTime) animator.Play("Instable");
            }
            else
            {
                animator.Play("Estable");
                body.GravityMultiplier = gravityMultiplier;
            }
        }
    }

    #endregion

    #region Public Methods

    public void DestroyBlock()
    {
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

    #endregion
}
