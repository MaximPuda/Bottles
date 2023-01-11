using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyTypesBoxView : ItemsCollectorView
{
    [SerializeField] private SpriteRenderer _currentTypeRenderer;
    [SerializeField] private Sprite _defaultTypeSprite;
    [SerializeField] private BoxTypeSprite[] _boxTypeSprites;

    private Cell[] _cells;

    public override void Initialize(ItemsCollector collector)
    {
        base.Initialize(collector);

        _cells = GetComponentsInChildren<Cell>();
        _currentTypeRenderer.sprite = _defaultTypeSprite;
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

        _currentTypeRenderer.sprite = _defaultTypeSprite;
    }

    protected override void OnItemAdded(ItemController item)
    {
        foreach (var cell in _cells)
        {
            if(cell.IsEmpty)
            {
                cell.AddItem(item);
                SetTypeSprite(item);
                return;
            }    
        }
    }

    private void SetTypeSprite(ItemController item)
    {
        foreach (var typeSprite in _boxTypeSprites)
        {
            if (typeSprite.Type == item.Type)
                _currentTypeRenderer.sprite = typeSprite.Sprite;
        }
    }
}
