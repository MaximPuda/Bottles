using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{
    [SerializeField] private float _scaleFactor = 0.6f;

    public bool IsEmpty { get; private set; } = true;
    public ItemController CurrentItem { get; private set; }

    public void AddItem(ItemController item)
    {
        CurrentItem = item;
        CurrentItem.transform.parent = transform;
        CurrentItem.transform.localPosition = Vector3.zero;
        CurrentItem.transform.localScale = Vector3.one * _scaleFactor;
        CurrentItem.transform.localRotation = Quaternion.identity;

        IsEmpty = false;
    }

    public void HideItem()
    {
        CurrentItem.Hide();
    }

    public void RemoveItem()
    {
        if(CurrentItem != null)
        {
            CurrentItem.DestroyItem(true);
            CurrentItem = null;
            IsEmpty = true;
        }
    }
}
