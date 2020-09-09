using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpellCast : MonoBehaviour
{
    public Vector2 castPoint;

    private Queue<Spell> spells;
    private Player player;

    private void Start()
    {
        spells = new Queue<Spell>();
        player = GetComponent<Player>();
    }

    public void AddSpell(Spell _spell)
    {
        spells.Enqueue(_spell);
    }

    public void CastSpell()
    {
        if (spells.Count > 0)
        {
            Vector2 _point = (Vector2)transform.position + new Vector2(castPoint.x * Mathf.Sign(player.Direction.x), castPoint.y);

            Spell _spell = spells.Dequeue();
            _spell?.Cast(player, _point);
        }
    }
}
