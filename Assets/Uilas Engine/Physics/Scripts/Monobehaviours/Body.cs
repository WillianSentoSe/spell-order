using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Body : MonoBehaviour
{
    #region Properties

    // Constants
    private const int rayCount = 5;
    private const float minGroundDistance = 0.1f;
    private const float skinWidth = 0.003f;
    private const float minDistance = 0.001f;
    private const float minSlopeAngle = 45;

    // Public Properties
    public float footDistance = 0.5f;
    public Vector2 contactPoint;
    public float groundOvertime;
    public bool pushable;
    public LayerMask contactLayers;
    [HideInInspector] public bool ignoreSemiCollision;
    public event System.Action<Vector2> beforeMove;
    public event System.Action<Vector2> afterMove;

    // Protected Properties
    protected Vector2 velocity;
    protected bool grounded = false;
    protected float gravityMultiplier = 1f;
    protected bool skipGroundCheck = false;
    protected Vector2 contactNormal = Vector2.up;

    // Private Properties
    private float groundedTimeLeft;

    // References
    protected BoxCollider2D boxCollider2D;
    protected Rigidbody2D rb2d; 

    #endregion

    #region Getters and Setters

    public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
    public bool Grounded { get { return grounded; } }
    public bool GroundedOvertime { get { return groundedTimeLeft > 0; } set { groundedTimeLeft = value ? groundOvertime : 0; } }
    public float GravityMultiplier { get { return gravityMultiplier; } set { gravityMultiplier = value; } }
    public Vector2 Foot { get { return (Vector2)transform.position - new Vector2(0f, footDistance); } }
    public Vector2 Normal { get { return contactNormal; } }

    #endregion

    #region Execution

    protected virtual void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        ApplyGravity();
        CheckGround();
        Move(velocity * Time.deltaTime);

        Debug.DrawRay(Foot, contactNormal, Color.yellow);
    }

    #endregion

    #region Physics and Movement

    /// <summary>
    /// Move the object to the ground and set it grounded if it is close enough from the ground
    /// </summary>
    private void CheckGround()
    {
        if (groundedTimeLeft > 0)
        {
            groundedTimeLeft -= Time.deltaTime;
        }

        // Ignore the ground verification if the object is rising
        if (velocity.y > 0)
        {
            grounded = false;
            return;
        }

        // Checking distance from the ground
        float _groundDistance = minGroundDistance;
        grounded = CheckDistance(Vector2.down, ref _groundDistance);

        if (grounded)
        {
            rb2d.position += Vector2.down * _groundDistance;
            velocity = new Vector2(velocity.x, 0);
            groundedTimeLeft = groundOvertime;
        }
    }

    /// <summary>
    /// Apply or reset the object's gravity force
    /// </summary>
    private void ApplyGravity()
    {
        if (!grounded)
        {
            velocity += Physics2D.gravity * gravityMultiplier * Time.deltaTime;
        }
    }

    /// <summary>
    /// Move the object in any direction
    /// </summary>
    /// <param name="_velocity"></param>
    public void Move(Vector2 _velocity)
    {
        Vector2 _normalizedDirection;

        if (grounded)
        {
            _normalizedDirection = Vector2.Perpendicular(contactNormal) * -Mathf.Sign(Vector2.SignedAngle(_velocity, contactNormal)) * _velocity.magnitude;
        }
        else
        {
            _normalizedDirection = _velocity;
        }

        if (_normalizedDirection.y > 0)
        {
            SimpleMove(_normalizedDirection.y, true);
            SimpleMove(_normalizedDirection.x, false);
        }
        else
        {
            SimpleMove(_normalizedDirection.x, false);
            SimpleMove(_normalizedDirection.y, true);
        }
    }

    /// <summary>
    /// Move the object in one direction
    /// </summary>
    /// <param name="_distance"></param>
    /// <param name="_vertical"></param>
    protected void SimpleMove(float _distance, bool _vertical = false)
    {
        // Setting the direction of the movement
        Vector2 _direction = _vertical? Vector2.up : Vector2.right;
        _direction = _direction * Mathf.Sign(_distance);
        _distance = Mathf.Abs(_distance);

        // Before movement callback
        beforeMove?.Invoke(_direction * _distance);

        // Return if the movement is too small
        if (_distance < minDistance)
            return;

        // Checking for collision
        bool _collided = CheckDistance(_direction, ref _distance);

        if (_vertical && _collided && _direction.y > 0)
        {
            Velocity = new Vector2(Velocity.x, 0f);
        }

        // Applying movement
        Vector2 _finalVelocity = _direction * _distance;
        rb2d.position += _finalVelocity;

        // After movement callback
        afterMove?.Invoke(_finalVelocity);
    }

    #endregion

    #region Utility

    /// <summary>
    /// Check for collision at given direction and distance
    /// </summary>
    /// <param name="_direction"></param>
    /// <param name="_distance"></param>
    /// <returns>True if there are any collision and False otherwise</returns>
    public bool CheckDistance(Vector2 _direction, ref float _distance)
    {
        RaycastHit2D _hit = new RaycastHit2D();

        if (GetCollisor(_direction, _distance, ref _hit))
        {
            _distance = Mathf.Min(_distance, _hit.distance - skinWidth);

            float _angle = Vector2.Angle(_hit.normal, Vector2.up);
            if (Mathf.Abs(_angle) <= minSlopeAngle)
            {
                contactNormal = _hit.normal;
                contactPoint = _hit.point;
            }

            return true;
        }

        return false;
    }

    public bool GetCollisor(Vector2 _direction, float _distance, ref RaycastHit2D _hit)
    {
        Vector2 _startingPoint;
        Vector2 _spreadDirection;

        float _padding = 0.003f;
        Bounds _bounds = boxCollider2D.bounds;
        _bounds.size -= new Vector3(_padding * 2, _padding * 2, 0f);

        float _size;
        bool _collided = false;
        float _minDistance = _distance + _padding;

        // Setting the spread direction of the rays
        _spreadDirection = new Vector2(-_direction.y, -_direction.x);   // Rotating the direction 90 degree counter-clockwise

        // Setting the starting point
        if (_direction == Vector2.right || _direction == Vector2.up)
        {
            _startingPoint = _bounds.max ;                  // Start on the upper right corner of the colider 
        }
        else
        {
            _startingPoint = _bounds.min;                  // Start on the lower left corner of the colider 
        }

        // Setting the size of the area covered by the rays
        if (_direction == Vector2.right || _direction == Vector2.left)
        {
            _size = _bounds.size.y;                               // Horizontal rays spread across the height
        }
        else
        {
            _size = _bounds.size.x;                               // Vertical rays spread across the width
        }

        // Setting the spacing between the rays
        float _spacing = _size / (rayCount - 1);

        // Disabling the collider. It prevents the raycast from hitting itself
        boxCollider2D.enabled = false;

        // Creating the rays
        for (int i = 0; i < rayCount; i++)
        {
            // Setting the start point of the ray based on the index
            Vector2 _relativePosition = _startingPoint + _spreadDirection * i * _spacing;

            // Just for debug
            Debug.DrawLine(_relativePosition, _relativePosition + _direction * (_distance + skinWidth + _padding), Color.white);

            // Checking for collision from the current ray
            RaycastHit2D _aux = Physics2D.Raycast(_relativePosition, _direction, _distance + skinWidth + _padding, contactLayers);

            if (_aux && !_aux.collider.gameObject.Equals(gameObject))
            {
                bool _ignoreSemiCollision = _aux.collider.gameObject.layer == 11 && (_direction.y >= 0 || ignoreSemiCollision);

                if (!_ignoreSemiCollision && _aux.distance - skinWidth + _padding < _minDistance)
                {
                    _minDistance = _aux.distance - skinWidth + _padding;
                    _hit = _aux;
                    _collided = true;
                }
            }
        }

        // Enabling the collider
        boxCollider2D.enabled = true;

        return _collided;
    }

    #endregion

    #region Misc

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawIcon(Foot, "icon_dot.png", false);
    } 

    #endregion
}
