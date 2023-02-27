using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyTypesBoxView : BoxView
{
    [SerializeField] private SpriteRenderer _currentTypeRenderer;
    [SerializeField] private Sprite _defaultTypeSprite;
    [SerializeField] private BoxTypeSprite[] _boxTypeSprites;

    public override void Initialize(BoxController collector)
    {
        base.Initialize(collector);

        foreach (var cell in Cells)
        {
            if (!cell.IsEmpty)
            {
                SetTypeSprite(cell.Item);
                break;
            }
        }
    }


    protected override void OnClearItems()
    {
        base.OnClearItems();
    }

    protected override void OnItemAdded(ItemController item)
    {
        base.OnItemAdded(item);

        if (_currentTypeRenderer.sprite == null ||
            _currentTypeRenderer.sprite == _defaultTypeSprite)
            SetTypeSprite(item);
    }

    protected override void OnAllItemsCollected(int combo)
    {
        base.OnAllItemsCollected(combo);

        foreach (var cell in Cells)
            cell.HideItem();
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
