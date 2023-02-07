using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridController : Controller
{
    [SerializeField] private GridCell _cell;
    [SerializeField] private int _maxRowLength = 7;
    [SerializeField] private float _showHideDelay = 0.01f;
    [SerializeField] private bool _outlineEnabled = true;
    [SerializeField] private float _outlineOffset = 0.2f;

    public event UnityAction AllCellPurchasedEvent;
    
    private GridRow[] _grid;
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

        Create();
        PaintOutline();
        InitPoolAndSpawner();
        //FillGrid();
    }

    private void InitPoolAndSpawner()
    {
        ItemController prefab = _level.ItemPrefab;
        if (prefab == null)
        {
            Debug.LogWarning("Item prefab is not assigned to Grid");
            return;
        }

        _itemPool = new ItemPool("MainItemPool", 50, _level.ItemPrefab, transform);

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

        _grid = new GridRow[_level.Grid.Length];

        float offsetY = 0;
        for (int row = 0; row < _grid.Length; row++)
        {
            string rowName = "Row " + row;
            _grid[row] = new GridRow(rowName, transform, _cellSizeX, offsetY, _maxRowLength);
            _grid[row].EmptyCellEvent += FillGrid;

            for (int x = 0; x < _level.Grid[row].CellsAmount; x++)
                _grid[row].AddCell(_cell);

            offsetY += _cellSizeY;
        }
    }

    private void OnDisable()
    {
        for (int row = 0; row < _grid.Length; row++)
        {
            _grid[row].EmptyCellEvent -= FillGrid;
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

    private IEnumerator Fill()
    {
        for (int row = 0; row < _grid.Length; row++)
        {
            while (!_grid[row].IsFull)
            {
                AddItemInRow(_grid[row]);

                yield return new WaitForSeconds(_showHideDelay);
            }
        }
    }

    public void AddItemInRow(GridRow row)
    {
        if (!_isCleared)
        {
            if (!row.IsFull)
            {
                var newItem = _spawner.GetItem();
                if (newItem != null)
                    row.AddItem(newItem);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemSample"></param>
    /// <returns>Возвращает true, если на поле уже есть хотя бы один ItemController c таким же типом и цветом, что и itemSampler</returns>
    public bool CheckItemInGird(ItemController itemSample)
    {
        if (itemSample == null)
            return false;

        foreach (var row in _grid)
            if (row.CheckItemInRow(itemSample))
                return true;
        return false;
    }

    [ContextMenu("Add new cell")]
    public void AddCell()
    {
        for (int i = _currentRowIndex; i < _grid.Length; i++)
        {
            var row = _grid[_currentRowIndex];
            if (row.CanAddCell())
            {
                row.AddCell(_cell);
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

    [ContextMenu("Clear grid")]
    public void ClearGrid()
    {
        foreach (var row in _grid)
            StartCoroutine(row.ClearRow(_showHideDelay));

        _isCleared = true;
    }

    [ContextMenu("Paint outline")]
    private void PaintOutline()
    {
        _outline.positionCount = 0;
        List<Vector3> positions = new List<Vector3>();
        
        //Получение точек обводки левой стороны поля
        for (int row = 0; row < _grid.Length; row++)
        {
            Vector3 cellPosition = _grid[row].GetCellPosition(0);
            
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

    //public void ClearGrid()
    //{
    //    StartCoroutine(Clear());
    //    _isCleared = true;
    //}

    //private IEnumerator Clear()
    //{
    //    for (int y = 0; y < _grid.Length; y++)
    //    {
    //        for (int x = 0; x < _grid[y].Count; x++)
    //        {
    //            if (!_grid[y]._cells[x].IsEmpty)
    //            {
    //                _grid[y]._cells[x].Clear();

    //                yield return new WaitForSeconds(_showHideDelay);
    //            }
    //        }
    //    }
    //}
}
