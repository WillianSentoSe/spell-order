using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInventoryUI : MonoBehaviour
{
    private ItemSlotUI primarySlot;
    private ExpandableItemSlotUI secondarySlots;

    #region Getters and Setters

    public ItemSlotUI PrimarySlot
    {
        get
        {
            if (primarySlot == null) primarySlot = transform.Find("Primary Slot").GetComponent<ItemSlotUI>();
            return primarySlot;
        }
    }

    public ExpandableItemSlotUI SecondarySlots
    {
        get
        {
            if (secondarySlots == null) secondarySlots = transform.Find("Secondary Slots").GetComponent<ExpandableItemSlotUI>();
            return secondarySlots;
        }
    } 

    #endregion

    public void UpdateSpellData(List<Spell> spells)
    {
        PrimarySlot.SetItem(spells[0]);
        // SecondarySlots.SetItems(spells as List<BaseItem>);
    }
}
