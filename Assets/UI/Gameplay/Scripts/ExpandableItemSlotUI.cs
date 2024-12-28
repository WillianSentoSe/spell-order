using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandableItemSlotUI : BaseItemSlotUI
{
    private List<BaseItem> items;

    public bool HasItem { get { return items != null && items.Count > 0; } }

    public void SetItems(List<BaseItem> items)
    {

    }
}
