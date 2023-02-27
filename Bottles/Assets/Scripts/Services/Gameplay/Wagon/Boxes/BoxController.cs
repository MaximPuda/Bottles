using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(Animator), typeof(BoxView))]
public class BoxController : MonoBehaviour, IInteractable
{
    [SerializeField] private TypeNames[] _acceptedTypes;
    [SerializeField] private bool _typesEnable;
    [SerializeField] private ColorNames[] _acceptedColors;
    [SerializeField] private bool _colorsEnable;
    [SerializeField] private float _delayClosedAnimationStart = 1.1f;

    [SerializeField] private ParticleSystem _coinFx;

    private BoxCell[] _cells;
    private BoxView _view;
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
        _cells = GetComponentsInChildren<BoxCell>();
        if (_cells.Length > 0)
        {
            foreach (var cell in _cells)
            {
                cell.Initialize();
                if (!cell.IsEmpty)
                {
                    if (IsAllAccept(cell.Item))
                    {
                        if (_currentType == TypeNames.None)
                            _currentType = cell.Item.Type;

                        if (_currentColor == ColorNames.None)
                            _currentColor = cell.Item.Color.Name;

                        _itemsCollected++;
                    }
                    else cell.Clear();
                }
            }
        }

        _view = GetComponent<BoxView>();
        _view.Initialize(this);

        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    public bool Interact(ItemController itemSender)
    {
        if (IsAllAccept(itemSender))
        {
            foreach (var cell in _cells)
            {
                if (cell.IsEmpty)
                {
                    AddInCell(itemSender);
                    _itemsCollected++;

                    ItemAddedEvent?.Invoke(itemSender);
                    OnItemAdd();
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsAllAccept(ItemController item)
    {
        return (_acceptedTypes[0] == TypeNames.None || IsTypeAccept(item.Type)) &&
            (_acceptedColors[0] == ColorNames.None || IsColorAccept(item.Color.Name));
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

    private void AddInCell(ItemController item)
    {
        Vector3 sender = item.DropPosition;
        float minDistance = float.MaxValue;
        int nearestCellIndex = 0;

        // Вычислеям ближайшую свободную ячейку
        for (int i = 0; i < _cells.Length; i++)
        {
            if (_cells[i].IsEmpty)
            {
                float distanceToCell = (sender - _cells[i].transform.position).magnitude;
                if (distanceToCell < minDistance)
                {
                    minDistance = distanceToCell;
                    nearestCellIndex = i;
                }
            }
        }

        if (_cells[nearestCellIndex] != null)
            _cells[nearestCellIndex].AddItem(item);
    }

    private void OnItemAdd()
    {
        if (_itemsCollected == _cells.Length)
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
        int typeMatch = 0;
        int colorMatch = 0;
        int combo = 0;

        foreach (var cell in _cells)
        {
            if (cell.IsEmpty)
                continue;

            if (_currentType == TypeNames.Multi || _currentType == TypeNames.None)
                _currentType = cell.Item.Type;

            if (_currentColor == ColorNames.Multi || _currentColor == ColorNames.None)
                _currentColor = cell.Item.Color.Name;

            if (_typesEnable)
            {
                if (cell.Item.Type == _currentType || cell.Item.Type == TypeNames.Multi)
                    typeMatch++;
            }

            if (_colorsEnable)
            {
                if (cell.Item.Color.Name == _currentColor || cell.Item.Color.Name == ColorNames.Multi)
                    colorMatch++;
            }

            if (cell.Item.Type == TypeNames.Multi)
                _itemsToChangeType.Add(cell.Item);

            if (cell.Item.Color.Name == ColorNames.Multi)
                _itemsToChangeColor.Add(cell.Item);
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
        foreach (var cell in _cells)
        {
            if (!cell.IsPreinstalled && !cell.IsEmpty)
            {
                cell.Clear();
                _itemsCollected--;
            }
        }

        _currentType = TypeNames.None;
        _currentColor = ColorNames.None;

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
