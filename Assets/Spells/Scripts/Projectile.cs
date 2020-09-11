using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Properties

    public const float destroyDelay = 2f;
    public float velocity = 5f;
    public GameObject hitEffectParticle;
    public SpriteRenderer spriteRenderer;

    [HideInInspector] public Vector2 direction;

    #endregion

    #region Execution

    public void Start()
    {
        spriteRenderer.flipX = direction.x < 0f;
    }

    private void Update()
    {
        transform.position = (Vector2)transform.position + velocity * direction * Time.deltaTime;
    }

    #endregion

    #region Events

    public void OnTriggerEnter2D(Collider2D _collider)
    {
        OnCollision(_collider);
    }

    #endregion

    #region Protected Methods

    protected virtual void OnCollision(Collider2D _collider)
    {
        DestroyParticle();
    }

    protected virtual void DestroyParticle()
    {
        // Stopping the particle
        velocity = 0f;

        // Disabling its components
        if (spriteRenderer) spriteRenderer.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        // Stopping particle emitions in children
        Util.DisableChildrenParticles(transform);

        // Creating the hit effect
        if (hitEffectParticle) Instantiate(hitEffectParticle, transform.position, Quaternion.identity);

        // Destroying the object
        Destroy(gameObject, destroyDelay);
    }

    #endregion
}
