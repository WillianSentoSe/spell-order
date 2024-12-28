using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]
public class Body : MonoBehaviour
{
    #region Properties

    // Constants
    private const int rayCount = 3;
    private const float minGroundDistance = 0.01f;
    private const float skinWidth = 0.003f;
    private const float minDistance = 0.001f;
    private const float minSlopeAngle = 45;

    // Public Properties
    public float footDistance = 0.5f;
    public Vector2 contactPoint;
    public float groundOvertime;
    public bool pushable;
    public LayerMask contactLayers;
    public LayerMask pushableLayers;
    [HideInInspector] public bool ignoreSemiCollision;
    public event System.Action<Vector2> beforeMove;
    public event System.Action<Vector2> afterMove;

    // Protected Properties
    protected Vector2 velocity;
    protected bool grounded = false;
    protected float gravityMultiplier = 1f;
    protected bool affectedByGravity = true;
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

    public Bounds GetBounds(float _padding = skinWidth)
    {
        return new Bounds((Vector2)transform.position + boxCollider2D.offset, new Vector2(boxCollider2D.size.x - _padding, boxCollider2D.size.y - _padding));
    }

    #endregion

    #region Execution

    protected virtual void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        CheckGround();
    }

    public void FixedUpdate()
    {
        ApplyGravity();
        //CheckGround();
        Move(velocity * Time.fixedDeltaTime);

        // Debug.DrawRay(Foot, contactNormal, Color.yellow);
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
            transform.position = (Vector2) transform.position + Vector2.down * _groundDistance;
            velocity = new Vector2(velocity.x, 0);
            groundedTimeLeft = groundOvertime;
        }
    }

    /// <summary>
    /// Apply or reset the object's gravity force
    /// </summary>
    private void ApplyGravity()
    {
        if (affectedByGravity && !grounded)
        {
            velocity += Physics2D.gravity * gravityMultiplier * Time.fixedDeltaTime;
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
        Vector2 _direction = (_vertical? Vector2.up : Vector2.right) * Mathf.Sign(_distance);
        _distance = Mathf.Abs(_distance);

        // Before movement callback
        beforeMove?.Invoke(_direction * _distance);

        // Return if the movement is too small
        if (_distance < minDistance) return;

        // Checking for collision
        bool _collided = CheckDistance(_direction, ref _distance);

        if (_vertical && _collided && _direction.y > 0)
        {
            ResetGravity();
        }

        // Applying movement
        Vector2 _finalVelocity = _direction * _distance;
        transform.position = (Vector2)transform.position + _finalVelocity;

        // After movement callback
        afterMove?.Invoke(_finalVelocity);
    }

    public void ResetGravity()
    {
        Velocity = new Vector2(Velocity.x, 0f);
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
        RaycastHit2D? _hit = MultiCast(_direction, _distance, contactLayers);

        if (_hit.HasValue)
        {
            _distance = Mathf.Min(_distance, _hit.Value.distance - skinWidth);

            float _angle = Vector2.Angle(_hit.Value.normal, Vector2.up);
            if (Mathf.Abs(_angle) <= minSlopeAngle)
            {
                contactNormal = _hit.Value.normal;
                contactPoint = _hit.Value.point;
            }

            return true;
        }

        return false;
    }

    public bool GetCollisor(Vector2 _direction, float _distance, ref RaycastHit2D _hit, bool _debug = false)
    {
        return GetCollisor(_direction, _distance, ref _hit, contactLayers, _debug);
    }

    public Ray2D[] GetRays(Vector2 _direction)
    {
        Bounds bounds = GetBounds();
        Ray2D[] rays = new Ray2D[rayCount];
        Vector2 perpendicular = new Vector2(-_direction.y, -_direction.x);
        Vector2 start = (_direction == Vector2.right || _direction == Vector2.up) ? bounds.max : bounds.min;
        float size = (_direction == Vector2.right || _direction == Vector2.left) ? bounds.size.y : bounds.size.x;
        float spaccing = size / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            Vector2 _origin = start + perpendicular * spaccing * i;
            rays[i] = new Ray2D(_origin, _direction);
        }

        return rays;
    }

    public RaycastHit2D? MultiCast(Vector2 _direction, float _distance, LayerMask _layerMask)
    {
        float _minDistance = _distance;
        RaycastHit2D? _closestHit = null;

        boxCollider2D.enabled = false;

        foreach (var ray in GetRays(_direction))
        {
            RaycastHit2D _hit = Physics2D.Raycast(ray.origin, ray.direction, _minDistance + skinWidth, _layerMask);
            Debug.DrawRay(ray.origin, ray.direction * (_hit.collider != null? _hit.distance : _minDistance), Color.yellow);

            if (_hit.collider != null && _hit.distance <= _minDistance)
            {
                _minDistance = _hit.distance;
                _closestHit = _hit;

                if (_minDistance <= minDistance) break;
            }
        }

        boxCollider2D.enabled = true;

        return _closestHit;
    }

    public RaycastHit2D[] MultiCastAll(Vector2 _direction, float _distance, LayerMask _layerMask)
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        boxCollider2D.enabled = false;

        foreach (var ray in GetRays(_direction))
        {
           foreach(var hit in Physics2D.RaycastAll(ray.origin, ray.direction, _distance + skinWidth, _layerMask))
           {
                hits.Add(hit);
                hit.collider.enabled = false;
            }
        }

        boxCollider2D.enabled = true;

        hits.ForEach(hit => hit.collider.enabled = true);

        return hits.ToArray();
    }

    public bool GetCollisor(Vector2 _direction, float _distance, ref RaycastHit2D _outHit, LayerMask _layerMask, bool _debug = false)
    {
        RaycastHit2D? _hit = MultiCast(_direction, _distance, _layerMask);
        if (_hit != null) _outHit = _hit.Value;

        return _hit != null;
    }

    public void StopAllForces()
    {
        velocity = Vector2.zero;
        affectedByGravity = false;
        StopAllCoroutines();
    }

    public Collider2D GetCollider()
    {
        return boxCollider2D;
    }

    public static float HeightToJumpForce(float _height)
    {
        return Mathf.Sqrt(_height * -Physics2D.gravity.y * 2);
    }

    public Collider2D[] CheckColliders(LayerMask _layerMask)
    {
        return Physics2D.OverlapBoxAll(transform.position, boxCollider2D.size, 0, _layerMask);
    }

    public List<T> CheckCollidersOfType<T>(LayerMask _layerMask)
    {
        List<T> list = new List<T>();

        foreach (var collider in CheckColliders(_layerMask))
        {
            T colliderType = collider.GetComponent<T>();

            if (colliderType != null)
            {
                list.Add(colliderType);
            }
        }

        return list;
    }

    #endregion

    #region Misc

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawIcon(Foot, "icon_dot.png", false);
    } 

    #endregion
}
