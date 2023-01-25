using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GridCell : MonoBehaviour
{
    [SerializeField] private Transform _currentItemContainer;
    
    [SerializeField] private float _sizeX;
    [SerializeField] private float _sizeY;
    [SerializeField] private bool _showSize;
    
    [SerializeField] private float _delayCellFree = 1f;
    
    public float SizeX => _sizeX;
    public float SizeY => _sizeY;

    public bool IsEmpty { get; private set; } = true;

    public event UnityAction<GridCell> CellFreeEvent;

    public ItemController CurrentItem { get; private set; }

    private void Update()
    {
        if (!IsEmpty && _currentItemContainer.childCount == 0)
            OnCellFree();
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

            IsEmpty = false;
            return true;
        }

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

    private void OnDrawGizmos()
    {
        if (_showSize)
            Gizmos.DrawWireCube(transform.position, new Vector3(_sizeX, _sizeY, 0f));
    }
}
