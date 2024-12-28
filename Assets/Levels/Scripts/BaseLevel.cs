using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseLevel : MonoBehaviour
{
    private Tilemap tilemap;

    public virtual void Start()
    {
        
    }

    public Tilemap GetTilemap()
    {
        if (tilemap == null)
            tilemap = transform.Find("Grid/Tilemap").GetComponent<Tilemap>();

        return tilemap;
    }

    public void OnDrawGizmos()
    {
        Bounds bounds = GetTilemap().localBounds;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
