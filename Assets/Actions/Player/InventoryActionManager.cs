using System.Collections.Generic;
using UnityEngine;

public class InventoryActionManager : BaseEventController
{
    public System.Action<List<Spell>> OnDataChange;
    public System.Action<List<Spell>> OnItemRemoved;
    public System.Action<Spell> OnItemAdded;

    private SpellInventory inventory;

    public InventoryActionManager(SpellInventory inventory) : base()
    {
        this.inventory = inventory;
    }

}
