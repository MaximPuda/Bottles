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

    protected override void OnItemAdded(ItemController item)
    {
 
    }
}
