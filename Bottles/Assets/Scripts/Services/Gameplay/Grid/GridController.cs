using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : Controller
{
    [SerializeField] private GridCell _cell;
    [SerializeField] private int _columns;
    [SerializeField] private int _rows;
    [SerializeField] private float _showHideDelay = 0.01f;

    private GridCell[,] _grid;
    private float _cellSizeX;
    private float _cellSizeY;

    private bool _isCleared;

    private ItemPool _itemPool;
    private GridSpawner _spawner;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        Create();
        InitPoolAndSpawner();
        FillGrid();
    }

    private void InitPoolAndSpawner()
    {
        Level level = ((GamePlayService)CurrentService).LevelCTRL.CurrentLevel;

        ItemController prefab = level.ItemPrefab;
        if (prefab == null)
        {
            Debug.LogWarning("Item prefab is not assigned to Grid");
            return;
        }

        _itemPool = new ItemPool("MainItemPool", 50, level.ItemPrefab, transform);

        _spawner = new GridSpawner(_itemPool, level.ItemTypes, level.ItemColors);
    }

    private void Create()
    {
        if (_cell == null)
        {
            Debug.LogWarning("Cell prefab is not assigned to Grid");
            return;
        }

        _grid = new GridCell[_columns, _rows];
        _cellSizeX = _cell.SizeX;
        _cellSizeY = _cell.SizeY;

        float startX = _cellSizeX * (_columns - 1) / 2 * -1;

        float offsetY = 0;
        for (int y = 0; y < _rows; y++)
        {
            Transform lineContainer = new GameObject("Row " + y).transform;
            lineContainer.parent = transform;
            lineContainer.localPosition = new Vector2 (0, offsetY);

            float offsetX = startX;
            for (int x = 0; x < _columns; x++)
            {
                var newCell = Instantiate(_cell);
                newCell.CellFreeEvent += AddItemInCell;
                _grid[x, y] = newCell;
                Transform newCellTrans = _grid[x, y].transform;
                newCellTrans.parent = lineContainer;
                newCellTrans.localPosition = new Vector2(offsetX, 0);
                offsetX += _cellSizeX;
            }
            offsetY += _cellSizeY;
        }
    }

    private void OnDisable()
    {
        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _columns; x++)
            {
                if (!_grid[x, y].IsEmpty)
                {
                   _grid[x, y].CellFreeEvent -= AddItemInCell;
                }
            }
        }
    }

    public void FillGrid()
    {
        if (_itemPool == null)
            return;

        StartCoroutine(Fill());
        _isCleared = false;
    }

    private IEnumerator Fill()
    {
        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _columns; x++)
            {
                if (_grid[x, y].IsEmpty)
                {
                    AddItemInCell(_grid[x, y]);

                    yield return new WaitForSeconds(_showHideDelay);
                }
            }
        }
    }

    public void AddItemInCell(GridCell cell)
    {
        if (!_isCleared)
        {
            var newItem = _spawner.GetRandomItem();
            if (newItem != null)
            {
                cell.AddItem(newItem);
            }
        }
    }

    public void ClearGrid()
    {
        StartCoroutine(Clear());
        _isCleared = true;
    }

    private IEnumerator Clear()
    {
        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _columns; x++)
            {
                if (!_grid[x, y].IsEmpty)
                {
                    _grid[x, y].Clear();

                    yield return new WaitForSeconds(_showHideDelay);
                }
            }
        }
    }
}
