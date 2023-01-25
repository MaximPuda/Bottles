using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GridRow
{
    private List<GridCell> _cells; 
    public int Count => _cells.Count;
    public Transform Container { get; private set; }
    public bool IsFull { get; private set; }

    public event UnityAction EmptyCellEvent;

    private float _cellSizeX;
    private int _maxLength;
    private int _fullCellsCount;

    public GridRow(string name, Transform parent, float cellSizeX, float positionY, int maxLength)
    {
        _cells = new List<GridCell>();
        _cellSizeX = cellSizeX;
        _maxLength = maxLength;

        Container = new GameObject(name).transform;
        Container.parent = parent;
        Container.localPosition = new Vector2(0, positionY);
    }

    public bool AddCell(GridCell prefab)
    {
        if (Count < _maxLength)
        {
            var newCell = GameObject.Instantiate(prefab, Container);
            _cells.Add(newCell);

            newCell.CellFreeEvent += OnCellFree;

            IsFull = false;
            EmptyCellEvent?.Invoke();

            Arrange();
            return true;
        }

        return false;
    }

    public bool CanAddCell()
    {
        return Count < _maxLength;
    }

    public void Arrange()
    {
        float startX = _cellSizeX * (_cells.Count - 1) / 2 * -1;
        float offsetX = startX;

        for (int x = 0; x < _cells.Count; x++)
        {
            Transform cell = _cells[x].transform;
            cell.localPosition = new Vector2(offsetX, 0);
            offsetX += _cellSizeX;
        }
    }

    public void AddItem(ItemController item)
    {
        if (item != null)
        {
            foreach (var cell in _cells)
            {
                if (cell.IsEmpty)
                {
                    cell.AddItem(item);
                    _fullCellsCount++;
                    break;
                }
            }
        }

        if (_fullCellsCount >= Count)
            IsFull = true;
    }

    private void OnCellFree(GridCell cell)
    {
        _fullCellsCount--;
        IsFull = false;
        EmptyCellEvent?.Invoke();
    }

    public void Clear(float delay)
    {
        foreach (var cell in _cells)
        {
            if (!cell.IsEmpty)
                cell.Clear();
        }

        IsFull = false;
    }

    public IEnumerator ClearRow(float delay)
    {
        yield return new WaitForSeconds(1f);

        foreach (var cell in _cells)
        {
            if (!cell.IsEmpty)
                cell.Clear();
            yield return new WaitForSeconds(delay);
        }

        IsFull = false;
        _fullCellsCount = 0;
    }

    public Vector3 GetCellPosition(int index)
    {
        return _cells[index].transform.position;
    }
}
