using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBottlesBoxView : BoxView
{
    private BoxCell[] _cells;

    public override void Initialize(BoxController collector)
    {
        base.Initialize(collector);

        _cells = GetComponentsInChildren<BoxCell>();
    }

    protected override void OnAllItemsCollected(int combo)
    {
        Animator.SetTrigger("Close");
    }

    protected override void OnClearItems()
    {
        foreach (var cell in _cells)
            if (cell != null)
                cell.Clear();
    }

    protected override void OnItemAdded(ItemController item)
    {
        foreach (var cell in _cells)
        {
            if (cell.IsEmpty)
            {
                cell.AddItem(item);
                return;
            }
        }
    }
}
