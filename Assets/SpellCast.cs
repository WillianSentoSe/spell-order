using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpellCast : MonoBehaviour
{
    public Vector2 castPoint;
    public GameObject rod;

    private Queue<Spell> spells;
    private PlayerBase player;
    private SpriteRenderer playerSpriteRender;
    private SpriteRenderer rodSpriteRender;


    private void Start()
    {
        spells = new Queue<Spell>();
        player = GetComponent<PlayerBase>();
        playerSpriteRender = player.spriteRenderer;
        if (rod) rodSpriteRender = rod.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        rodSpriteRender.flipX = playerSpriteRender.flipX;
    }

    public void AddSpell(Spell _spell)
    {
        spells.Enqueue(_spell);
        UpdateRodSprite(_spell);
    }

    public void CastSpell()
    {
        if (spells.Count > 0)
        {
            Vector2 _point = (Vector2)transform.position + new Vector2(castPoint.x * Mathf.Sign(player.Direction.x), castPoint.y);

            Spell _spell = spells.Peek();

            if (_spell.CanCast(player))
            {
                _spell.Cast(player, _point);
                spells.Dequeue();

                Spell _nextSpell = null;
                if (spells.Count > 0) _nextSpell = spells.Peek();

                UpdateRodSprite(_nextSpell);
            }
        }
    }

    private void UpdateRodSprite(Spell _spell)
    {
        if (!rodSpriteRender) return;
        Sprite _newSprite = null;
        if (_spell) _newSprite = _spell.rodSprite;
        rodSpriteRender.sprite = _newSprite;
    }
}
