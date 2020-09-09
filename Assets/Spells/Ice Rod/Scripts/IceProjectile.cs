using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : Projectile
{
    public GameObject iceBlock;
    public SpriteRenderer spriteRenderer;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Grid grid = FindObjectOfType<Grid>();
        Vector2 _blockPosition = new Vector2(Mathf.Round(transform.position.x) - 0.5f * Mathf.Sign(direction.x), Mathf.Round(transform.position.y - 0.5f) + 0.5f);
        
        Instantiate(iceBlock, _blockPosition, Quaternion.identity);

        if (spriteRenderer) spriteRenderer.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        velocity = 0f;

        Destroy(gameObject, destroyDelay);
    }
}
