using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(Animator), typeof(ItemsCollectorView))]
public class ItemsCollector : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemType _collectedType;
    [SerializeField] private ColorsName _collectedColor;
    //[SerializeField] private ItemType[] _collectedTypes;
    //[SerializeField] private ColorsName[] _collectedColors;
    [SerializeField] private int _itemsAmount;

    public int ItemsAmount => _itemsAmount;

    private Item[] _items;

    private ItemsCollectorView _view;
    private Collider2D _collider;

    private int _itemsCollected = 0;
    private ItemType _currentType;
    private ColorsName _currentColor;

    public event UnityAction<Item> ItemAddedEvent;
    public event UnityAction<int> AllItemsCollectedEvent;
    public event UnityAction ClearItemsEvent;

    public void Initialize()
    {
        _view = GetComponent<ItemsCollectorView>();
        _view.Initialize(this);

        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        _items = new Item[_itemsAmount];

        _currentType = _collectedType;
        _currentColor = _collectedColor;
    }

    public bool Interact(Item itemSender)
    {
        if ((_collectedType == ItemType.All || itemSender.Type == _collectedType)
            && (_collectedColor ==  ColorsName.All || itemSender.Color.ColorName == _collectedColor))
        {
            for (int i = 0; i < _itemsAmount; i++)
            {
                if (_items[i] == null)
                {
                    itemSender.OnColllect();
                    _items[i] = itemSender;
                    _itemsCollected++;

                    ItemAddedEvent?.Invoke(itemSender);
                    OnItemAdd();
                    return true;
                }
            }
        }

        return false;
    }

    private void OnItemAdd()
    {
        if (_itemsCollected == _items.Length)
        {
            int combo = GetCombo();

            if (combo > 0)
                AllItemsCollectedEvent?.Invoke(combo);
            else
                Clear();
        }
        else if(_itemsCollected > 1)
        {
            int combo = GetCombo();
            if (combo == 0)
                Clear();
        }
    }

    private bool CheckTypes(Item itemSender)
    {


        return false;
    }

    private int GetCombo()
    {
        int typeMatch = 1;
        int colorMatch = 1;
        int combo = 0;
        List<Item> itemsToChangeColor = new();

        _currentType = _items[0].Type;
        _currentColor = _items[0].Color.ColorName;
        if(_items[0].Color.ColorName == ColorsName.All)
            itemsToChangeColor.Add(_items[0]);

        for (int i = 1; i < _itemsCollected; i++)
        {
            if (_currentType == ItemType.All)
                _currentType = _items[i].Type;

            if (_currentColor == ColorsName.Empty)
            {
                if (_collectedColor == ColorsName.Empty)
                    _currentColor = ColorsName.Empty;
                else
                    _currentColor = ColorsName.None;
            }    

            if(_currentColor == ColorsName.All)
                _currentColor = _items[i].Color.ColorName;

            if (_items[i].Color.ColorName == ColorsName.All)
                itemsToChangeColor.Add(_items[i]);

            if(_items[i].Type == _currentType || _items[i].Type == ItemType.All)
                typeMatch++;

            if(_items[i].Color.ColorName == _currentColor || _items[i].Color.ColorName == ColorsName.All)
                colorMatch++;
        }

        if(itemsToChangeColor.Count > 0 && _currentColor != ColorsName.All)
        {
            foreach (var item in itemsToChangeColor)
            {
                item.SetColor(_currentColor);
            }

            itemsToChangeColor.Clear();
        }

        if (typeMatch == _itemsCollected)
            combo++;

        if (colorMatch == _itemsCollected)
            combo++;

        return combo;
    }

    private void Clear()
    {
        for (int i = 0; i < _items.Length; i++)
            _items[i] = null;

        _itemsCollected = 0;

        _currentType = _collectedType;
        _currentColor = _collectedColor;

        ClearItemsEvent?.Invoke();
    }
}
