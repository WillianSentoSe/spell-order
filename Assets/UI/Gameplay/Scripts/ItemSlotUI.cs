using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : BaseItemSlotUI
{
    private BaseItem item;
    private Image icon;

    public virtual bool HasItem { get { return item != null; } }

    public void SetItem(BaseItem newItem)
    {
        item = newItem;
        GetIcon().sprite = item.sprite;
    }

    public override void Clear()
    {
        item = null;
    }

    public Image GetIcon()
    {
        if (icon == null) icon = transform.Find("Icon").GetComponent<Image>();
        return icon;
    }
}
