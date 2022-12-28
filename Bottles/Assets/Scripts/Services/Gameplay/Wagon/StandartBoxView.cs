using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartBoxView : ItemsCollectorView
{
    private Cell[] _cells;

    public override void Initialize(ItemsCollector collector)
    {
        base.Initialize(collector);

        _cells = GetComponentsInChildren<Cell>();
    }

    protected override void OnAllItemsCollected(int combo)
    {
        Animator.SetTrigger("Close");
        foreach (var cell in _cells)
            cell.HideItem();
    }

    protected override void OnClearItems()
    {
        foreach (var cell in _cells)
            if (cell != null)
                cell.RemoveItem();
    }

    protected override void OnItemAdded(ItemController item)
    {
        foreach (var cell in _cells)
        {
            if(cell.IsEmpty)
            {
                cell.AddItem(item);
                return;
            }    
        }
    }
}
