using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartBoxView : BoxView
{
    protected override void OnAllItemsCollected(int combo)
    {
        base.OnAllItemsCollected(combo);

        foreach (var cell in Cells)
                cell.HideItem();
    }
}
