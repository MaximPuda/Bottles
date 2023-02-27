using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxCell : MonoBehaviour
{
    public bool IsEmpty { get; private set; } = true;
    public bool IsPreinstalled { get; private set; }
    public ItemController Item { get; private set; }

    public void Initialize()
    {
        Item = GetComponentInChildren<ItemController>();

        if (Item == null)
        {
            IsEmpty = true;
            IsPreinstalled = false;
        }
        else
        {
            IsEmpty = false;
            IsPreinstalled = true;
            Item.OnColllect();
        }
    }

    public void AddItem(ItemController item)
    {
        Item = item;
        Item.OnColllect();
        Item.transform.parent = transform;
        Item.transform.localPosition = Vector3.zero;
        Item.transform.localScale = Vector3.one;
        Item.transform.localRotation = Quaternion.identity;

        IsEmpty = false;
    }

    public void HideItem()
    {
        Item.ActiveView.OnDestroyItem();
    }

    public void Clear()
    {
        if(Item != null)
        {
            Item.DestroyItem(true);
            Item = null;
            IsEmpty = true;
        }
    }
}
