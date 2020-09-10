using UnityEngine;

public class IceProjectile : Projectile
{
    #region Properties

    public GameObject iceBlock;

    #endregion

    #region Protected Methods

    protected override void OnCollision(Collider2D _collider)
    {
        Vector2 _blockPosition = new Vector2(Mathf.Round(transform.position.x) - 0.5f * Mathf.Sign(direction.x), Mathf.Round(transform.position.y - 0.5f) + 0.5f);
        Instantiate(iceBlock, _blockPosition, Quaternion.identity);

        DestroyParticle();
    } 

    #endregion
}
