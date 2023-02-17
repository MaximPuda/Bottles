using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GridCell : MonoBehaviour
{
    [SerializeField] private Transform _currentItemContainer;

    [SerializeField] private float _sizeX;
    [SerializeField] private float _sizeY;
    [SerializeField] private bool _showBorders;

    private GridCellLocker _locker;
    private bool _isLocked;
    public float SizeX => _sizeX;
    public float SizeY => _sizeY;

    public bool IsEmpty { get; private set; } = true;

    public event UnityAction<GridCell> CellFreeEvent;

    public ItemController CurrentItem { get; private set; }

    public void Initialize(Transform locker)
    {
        _locker = GetComponentInChildren<GridCellLocker>();

        if (_locker != null)
        {
            _locker.UnlockedEvent += Unlock;
            AddLocker(locker);
        }
    }

    private void Update()
    {
        if (_currentItemContainer.childCount == 0)
            OnCellFree();
    }
    
    private void OnDisable()
    {
        if (_locker != null)
            _locker.UnlockedEvent -= Unlock;
    }

    internal void AddLocker(Transform locker)
    {
        if (locker == null)
            return;

        Transform instance = Instantiate(locker, _locker.transform);
        locker.localPosition = Vector3.zero;

        _isLocked = true;
        _locker.gameObject.SetActive(true);
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
            CurrentItem.Lock(_isLocked);

            IsEmpty = false;
            return true;
        }

        return false;
    }

    public void HideItem(bool hide) => CurrentItem.Hide(hide);

    public bool CheckItemFullMatch(ItemController itemSample)
    {
        if (CurrentItem == null || itemSample == null)
            return false;

        if (CurrentItem.Type == itemSample.Type &&
            CurrentItem.Color.Name == itemSample.Color.Name)
            return true;

        return false;
    }

    public bool CheckColor(ItemController itemSample)
    {
        if (CurrentItem == null || itemSample == null)
            return false;

        return CurrentItem.Color.Name == itemSample.Color.Name;
    }

    public bool CheckType(ItemController itemSample)
    {
        if (CurrentItem == null || itemSample == null)
            return false;

        return CurrentItem.Type == itemSample.Type;
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
        _isLocked = false;
        _locker.gameObject.SetActive(false);
        CurrentItem.Lock(false);
    }

    private void OnDrawGizmos()
    {
        if (_showBorders)
            Gizmos.DrawWireCube(transform.position, new Vector3(_sizeX, _sizeY, 0f));
    }
}
