using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GridCell : MonoBehaviour
{
    [SerializeField] private Transform _currentItemContainer;
    [SerializeField] private bool _IsLocked;
    [SerializeField] private GridCellLocker _locker;

    [SerializeField] private float _sizeX;
    [SerializeField] private float _sizeY;
    [SerializeField] private bool _showBorders;

    public float SizeX => _sizeX;
    public float SizeY => _sizeY;

    public bool IsEmpty { get; private set; } = true;

    public event UnityAction<GridCell> CellFreeEvent;

    public ItemController CurrentItem { get; private set; }

    private void Awake()
    {
        if (_locker != null)
        {
            if (_IsLocked)
            {
                _locker.gameObject.SetActive(true);
                _locker.UnlockedEvent += Unlock;
            }
            else _locker.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!IsEmpty && _currentItemContainer.childCount == 0)
            OnCellFree();
    }
    private void OnDisable()
    {
        if (_locker != null)
            _locker.UnlockedEvent -= Unlock;
    }

    public bool AddItem(ItemController itemSender)
    {
        if (IsEmpty)
        {
            CurrentItem = itemSender;
            CurrentItem.transform.parent = _currentItemContainer;
            CurrentItem.transform.localPosition = Vector3.zero;
            CurrentItem.transform.localScale = Vector3.one;
            CurrentItem.PlaySpawnAnim();
            CurrentItem.Lock(_IsLocked);

            IsEmpty = false;
            return true;
        }

        return false;
    }

    public bool CheckItemMatch(ItemController itemSample)
    {
        if (CurrentItem == null || itemSample == null)
            return false;

        if (CurrentItem.Type == itemSample.Type &&
            CurrentItem.Color.Name == itemSample.Color.Name)
            return true;

        return false;
    }

    public void Clear()
    {
        CurrentItem.DestroyItem(true);
        CurrentItem = null;
        IsEmpty = true;
    }

    private void OnCellFree()
    {
        IsEmpty = true;
        CurrentItem = null;
        CellFreeEvent?.Invoke(this);
    }

    private void Unlock()
    {
        _IsLocked = false;
        _locker.gameObject.SetActive(false);
        CurrentItem.Lock(false);
    }

    private void OnDrawGizmos()
    {
        if (_showBorders)
            Gizmos.DrawWireCube(transform.position, new Vector3(_sizeX, _sizeY, 0f));
    }
}
