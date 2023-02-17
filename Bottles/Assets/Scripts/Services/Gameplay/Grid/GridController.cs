using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridController : Controller
{
    [SerializeField] private GridCell _cell;
    [SerializeField] private int _sameItems = 4;
    [SerializeField] private int _poolCapacity = 50;
    [SerializeField] private int _maxRowLength = 7;
    [SerializeField] private float _showHideDelay = 0.01f;
    [SerializeField] private bool _outlineEnabled = true;
    [SerializeField] private float _outlineOffset = 0.2f;

    public event UnityAction AllCellPurchasedEvent;
    
    private List<GridCell>[] _grid;

    private float _cellSizeX;
    private float _cellSizeY;

    private bool _isCleared;

    private Level _level;
    private ItemPool _itemPool;
    private GridSpawner _spawner;

    private int _currentRowIndex;

    private LineRenderer _outline;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _outline = GetComponentInChildren<LineRenderer>();

        _level = ((GamePlayService)CurrentService).LevelCTRL.CurrentLevel;

        InitPoolAndSpawner();
        Create();
        PaintOutline();
    }

    private void InitPoolAndSpawner()
    {
        ItemController prefab = _level.ItemPrefab;
        if (prefab == null)
        {
            Debug.LogWarning("Item prefab is not assigned to Grid");
            return;
        }

        _itemPool = new ItemPool("MainItemPool", _poolCapacity, _level.ItemPrefab, transform);
        _spawner = new GridSpawner(_itemPool, this, _level.ItemTypes, _level.ItemColors);
    }

    private void Create()
    {
        if (_cell == null)
        {
            Debug.LogWarning("Cell prefab is not assigned to Grid");
            return;
        }

        _cellSizeX = _cell.SizeX;
        _cellSizeY = _cell.SizeY;

        _grid = new List<GridCell>[_level.Grid.Length];

        float offsetY = 0;
        for (int row = 0; row < _grid.Length; row++)
        {
            string rowName = "Row " + row;
            Transform container = new GameObject(rowName).transform;
            container.parent = transform;
            container.localPosition = new Vector2(0, offsetY);

            _grid[row] = new List<GridCell>();

            for (int cell = 0; cell < _level.Grid[row].Cells.Length; cell++)
            {
                var settings = _level.Grid[row].Cells[cell];
                AddCell(row, container, settings);
            }

            ArrangeRow(row);

            offsetY += _cellSizeY;
        }
    }

    private void OnDisable()
    {
        foreach (var row in _grid)
            foreach (var cell in row)
                cell.CellFreeEvent -= AddItemInCell;
    }

    private bool AddCell(int row, Transform container = null, GridCellSettings settings = null)
    {
        var newCell = GameObject.Instantiate(_cell, container);
        _grid[row].Add(newCell);

        if (settings != null)
        {
            ItemController item = _spawner.GetItem(settings.ItemType, settings.ItemColor);
            newCell.Initialize(settings.Locker);
            newCell.AddItem(item);
            newCell.HideItem(true);
        }    

        newCell.CellFreeEvent += AddItemInCell;

        return true;
    }

    public void ShowItems()
    {
        StartCoroutine(ShowItemsCoroutine(_showHideDelay));
    }   

    public void ArrangeRow(int index)
    {
        float startX = _cellSizeX * (_grid[index].Count - 1) / 2 * -1;
        float offsetX = startX;

        for (int x = 0; x < _grid[index].Count; x++)
        {
            Transform cell = _grid[index][x].transform;
            cell.localPosition = new Vector2(offsetX, 0);
            offsetX += _cellSizeX;
        }
    }

    [ContextMenu("Fill Grid")]
    public void FillGrid()
    {
        if (_itemPool == null)
            return;

        StartCoroutine(Fill());
        _isCleared = false;
    }

    public void AddItemInCell(GridCell cell)
    {
        if (!_isCleared)
        {
            if (cell.IsEmpty)
            {
                var newItem = _spawner.GetItem();
                if (newItem != null)
                    cell.AddItem(newItem);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "itemSample" ></ param >
    /// < returns > Возвращает true, если на поле уже есть хотя бы один ItemController c таким же типом и цветом, что и itemSampler</returns>
    public bool CheckItemInGird(ItemController itemSample)
    {
        if (itemSample == null)
            return false;

        foreach (var row in _grid)
            if(row != null)
                foreach (var cell in row)
                    if (cell != null)
                        if(cell.CheckItemFullMatch(itemSample))
                            return true;

        int types = 0;
        int colors = 0;

        foreach (var row in _grid)
            if (row != null)
                foreach (var cell in row)
                    if (cell != null)
                    {
                        if (cell.CheckType(itemSample))
                        types++;

                        if (cell.CheckColor(itemSample))
                        colors++;
                    }

        return types >= _sameItems || colors >= _sameItems;
    }

    [ContextMenu("Add new cell")]
    public void AddNewCell()
    {
        for (int i = _currentRowIndex; i < _grid.Length; i++)
        {
            var row = _grid[_currentRowIndex];
            if (row.Count < _maxRowLength)
            {
                Transform container = row[0].transform.parent;
                AddCell(_currentRowIndex, container);
                ArrangeRow(_currentRowIndex);
                PaintOutline();

                _currentRowIndex++;
                if (_currentRowIndex >= _grid.Length)
                    _currentRowIndex = 0;

                break;
            }

            _currentRowIndex++;
            if (i == _currentRowIndex)
            {
                AllCellPurchasedEvent?.Invoke();
                Debug.Log("All cell are Purchased");
                break;
            }
        }
    }

    [ContextMenu("Paint outline")]
    private void PaintOutline()
    {
        if (!_outlineEnabled)
            return;

        _outline.positionCount = 0;
        List<Vector3> positions = new List<Vector3>();

        //Получение точек обводки левой стороны поля
        foreach (var row in _grid)
        {
            Vector3 cellPosition = row[0].transform.position;

            Vector3 point1 = new Vector3(cellPosition.x - _cellSizeX / 2, cellPosition.y - _cellSizeY / 2);
            positions.Add(point1);

            Vector3 point2 = new Vector3(cellPosition.x - _cellSizeX / 2, cellPosition.y + _cellSizeY / 2);
            positions.Add(point2);
        }

        //Отзеркаливание точек по горизонтали
        for (int i = positions.Count - 1; i >= 0 ; i--)
        {
            Vector3 point = new Vector3(positions[i].x * -1f, positions[i].y);
            positions.Add(point);
        }

        _outline.positionCount = positions.Count;
        _outline.SetPositions(positions.ToArray());
        _outline.Simplify(0); //Удаляет дублирующие точки
    }

    [ContextMenu("Clear grid")]
    public void ClearGrid()
    {
        StartCoroutine(Clear());
        _isCleared = true;
    }

    #region Coroutines
    private IEnumerator ShowItemsCoroutine(float delay)
    {
        foreach (var row in _grid)
            foreach (var cell in row)
            {
                cell.HideItem(false);
                yield return new WaitForSeconds(_showHideDelay);
            }
    }    
    
    private IEnumerator Fill()
    {
        foreach (var row in _grid)
            foreach (var cell in row)
            {
                AddItemInCell(cell);
                yield return new WaitForSeconds(_showHideDelay);
            }
    }
    
    private IEnumerator Clear()
    {
        for (int row = 0; row < _grid.Length; row++)
        {
            foreach (var cell in _grid[row])
            {
                if (!cell.IsEmpty)
                {
                    cell.Clear();

                    yield return new WaitForSeconds(_showHideDelay);
                }
            }
        }
    }
    #endregion
}
