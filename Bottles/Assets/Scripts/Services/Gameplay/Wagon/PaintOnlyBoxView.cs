using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintOnlyBoxView : ItemsCollectorView
{
    [SerializeField] private SpriteRenderer _main;
    [SerializeField] private SpriteRenderer _closed;
    [SerializeField] private Cell[] _fills;

    private ColorPalette _currentColor;
    protected override void OnAllItemsCollected(int combo)
    {
        _main.enabled = false;

        foreach (var fill in _fills)
            if (fill != null)
            {
                fill.GetComponent<SpriteRenderer>().enabled = false;
            }

        Animator.SetTrigger("Close");
    }

    protected override void OnClearItems()
    {
        foreach (var fill in _fills)
            if (fill != null)
            {
                fill.RemoveItem();
                fill.GetComponent<SpriteRenderer>().enabled = false;
            }
    }

    protected override void OnItemAdded(ItemController item)
    {
        for (int i = 0; i < _fills.Length; i++)
        {
            if (_fills[i].IsEmpty)
            {
                _fills[i].AddItem(item);
                _fills[i].HideItem();
                var fillRenderer = _fills[i].GetComponent<SpriteRenderer>();
                fillRenderer.color = item.Color.Color;
                fillRenderer.enabled = true;

                return;
            }
        }
    }
}
