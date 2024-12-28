using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : BaseUI
{
    private SpellInventoryUI spellInventory;

    public SpellInventoryUI Inventory {
        get {
            if (spellInventory == null) spellInventory = transform.Find("Spell Inventory").GetComponent<SpellInventoryUI>();
            return spellInventory;
        }
    }

    protected override void SetActions()
    {
        //spellInventory = transform.Find("Spell Inventory").GetComponent<SpellInventoryUI>();
        //GameManager.main.Player.Inventory.Actions.OnDataChange += Inventory.UpdateSpellData;
    }
}