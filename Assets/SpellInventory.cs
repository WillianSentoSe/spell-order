using System.Collections.Generic;
using UnityEngine;

public class SpellInventory : MonoBehaviour
{
    #region Properties

    public Vector2 castPoint;
    public GameObject rod;

    private Queue<Spell> spells;
    private BasePlayer player;
    private SpriteRenderer playerSpriteRender;
    private SpriteRenderer rodSpriteRender;
    private InventoryActionManager actionManager;

    #endregion

    #region Getters and Setters

    public Spell NextSpell { get { return spells.Count > 0 ? spells.Peek() : null; } }
    public List<Spell> Spells { get { return new List<Spell>(spells.ToArray()); } }
    public InventoryActionManager Actions { get { return actionManager; } }

    #endregion

    #region Execution

    private void Awake()
    {
        actionManager = new InventoryActionManager(this);
    }

    private void OnEnable()
    {
        actionManager.OnDataChange += UpdateRodSprite;
    }

    private void Start()
    {
        spells = new Queue<Spell>();
        player = GetComponent<BasePlayer>();
        playerSpriteRender = player.spriteRenderer;
        if (rod) rodSpriteRender = rod.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        rodSpriteRender.flipX = playerSpriteRender.flipX;
    } 

    #endregion

    public void AddSpell(Spell _spell)
    {
        spells.Enqueue(_spell);
        Actions.OnDataChange?.Invoke(Spells);
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

                Actions.OnDataChange?.Invoke(Spells);
            }
        }
    }

    private void UpdateRodSprite(List<Spell> spells)
    {
        if (!rodSpriteRender) return;

        if (spells.Count > 0)
        {
            rodSpriteRender.enabled = true;
            rodSpriteRender.sprite = NextSpell.rodSprite;
        }
        else
        {
            rodSpriteRender.enabled = false;
        }
    }
}
