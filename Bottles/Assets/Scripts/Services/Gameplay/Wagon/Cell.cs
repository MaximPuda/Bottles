using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{
    [SerializeField] private float _scaleFactor = 0.6f;

    public bool IsEmpty { get; private set; } = true;
    public Item CurrentItem { get; private set; }

    public void AddItem(Item item)
    {
        CurrentItem = item;
        var itemTrans = CurrentItem.transform;
        itemTrans.parent = transform;
        itemTrans.localPosition = Vector3.zero;
        itemTrans.localScale = Vector3.one * _scaleFactor;
        itemTrans.localRotation = Quaternion.identity;
        
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
           CurrentItem.Crash();
            CurrentItem = null;
            IsEmpty = true;
        }
    }
}
