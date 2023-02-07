using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(Animator), typeof(ItemsCollectorView))]
public class ItemsCollector : MonoBehaviour, IInteractable
{
    [SerializeField] private TypeNames[] _acceptedTypes;
    [SerializeField] private bool _typesEnable;
    [SerializeField] private ColorNames[] _acceptedColors;
    [SerializeField] private bool _colorsEnable;
    [SerializeField] private int _itemsAmount;
    [SerializeField] private float _delayClosedAnimationStart = 1.1f;

    [SerializeField] private ParticleSystem _coinFx;

    public int ItemsAmount => _itemsAmount;

    private ItemController[] _items;
    private ItemsCollectorView _view;
    private Collider2D _collider;

    private int _itemsCollected = 0;
    private TypeNames _currentType;
    private ColorNames _currentColor;

    private List<ItemController> _itemsToChangeColor = new();
    private List<ItemController> _itemsToChangeType = new();

    public event UnityAction<ItemController> ItemAddedEvent;
    public event UnityAction<int> AllItemsCollectedEvent;
    public event UnityAction ClearItemsEvent;

    public void Initialize()
    {
        _view = GetComponent<ItemsCollectorView>();
        _view.Initialize(this);

        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        _items = new ItemController[_itemsAmount];
    }

    public bool Interact(ItemController itemSender)
    {
        if ((_acceptedTypes[0] == TypeNames.None || IsTypeAccept(itemSender.Type)) &&
            (_acceptedColors[0] == ColorNames.None || IsColorAccept(itemSender.Color.Name)))
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

    private bool IsTypeAccept(TypeNames senderType)
    {
        foreach (var type in _acceptedTypes)
            if (senderType == type)
                return true;

        return false;
    }

    private bool IsColorAccept(ColorNames senderColorsName)
    {
        foreach (var color in _acceptedColors)
            if (senderColorsName == color)
                return true;

        return false;
    }

    private void OnItemAdd()
    {
        if (_itemsCollected == _items.Length)
        {
            int combo = GetCombo();

            if (combo > 0)
            {
                _coinFx.Play();
                StartCoroutine(AllCollectedInvoke(combo));
            }
            else
                Clear();
        }
        else if (_itemsCollected > 1)
        {
            int combo = GetCombo();
            if (combo == 0)
                Clear();
        }
    }

    private int GetCombo()
    {
        int typeMatch = 1;
        int colorMatch = 1;
        int combo = 0;

        _currentType = _items[0].Type;
        _currentColor = _items[0].Color.Name;

        if (_items[0].Type == TypeNames.Multi)
            _itemsToChangeType.Add(_items[0]);

        if (_items[0].Color.Name == ColorNames.Multi)
            _itemsToChangeColor.Add(_items[0]);

        for (int i = 1; i < _itemsCollected; i++)
        {
            if (_currentType == TypeNames.Multi)
                _currentType = _items[i].Type;

            if (_currentColor == ColorNames.Multi)
                _currentColor = _items[i].Color.Name;

            if (_typesEnable)
            {
                if (_items[i].Type == _currentType || _items[i].Type == TypeNames.Multi)
                    typeMatch++;
            }

            if (_colorsEnable)
            {
                if (_items[i].Color.Name == _currentColor || _items[i].Color.Name == ColorNames.Multi)
                    colorMatch++;
            }

            if (_items[i].Type == TypeNames.Multi)
                _itemsToChangeType.Add(_items[i]);

            if (_items[i].Color.Name == ColorNames.Multi)
                _itemsToChangeColor.Add(_items[i]);
        }

        if (_itemsToChangeType.Count > 0 && _currentType != TypeNames.Multi)
        {
            foreach (var item in _itemsToChangeType)
                item.SetType(_currentType);

            _itemsToChangeType.Clear();
        }

        if (_itemsToChangeColor.Count > 0 && _currentColor != ColorNames.Multi)
        {
            foreach (var item in _itemsToChangeColor)
                item.SetColor(_currentColor);

            _itemsToChangeColor.Clear();
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

        ClearItemsEvent?.Invoke();
    }

    private IEnumerator AllCollectedInvoke(int combo)
    {
        yield return new WaitForSeconds(_delayClosedAnimationStart);
        AllItemsCollectedEvent?.Invoke(combo);
    }

    public void SetParticleForceField(ParticleSystemForceField field)
    {
        if (field == null)
            return;

        _coinFx.externalForces.AddInfluence(field);
    }    
}
