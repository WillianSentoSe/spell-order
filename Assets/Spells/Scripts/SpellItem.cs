using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : MonoBehaviour, IPickable
{
    public Spell spell;

    public void OnPickup(SpellInventory inventory)
    {
        GameManager.main.Player.Inventory.AddSpell(spell);
        Destroy(gameObject);
    }
}
