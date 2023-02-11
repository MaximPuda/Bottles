using UnityEngine;

public class GridSpawner
{
    private ItemPool _itemPool;
    private GridController _grid;
    private ItemType[] _types;
    private ItemColor[] _colors;

    private int _accumulatedTypeWeights;
    private int _accumulatedColorWeights;

    public GridSpawner(ItemPool pool, GridController grid, ItemType[] itemTypes, ItemColor[] colors)
    {
        _itemPool = pool;
        _grid = grid;
        _types = itemTypes;
        _colors = colors;

        foreach (var type in _types)
            type.Weight = 0;
        
        foreach (var color in _colors)
            color.Weight = 0;

        CalculateWeights();
    }

    private void CalculateWeights()
    {
        _accumulatedTypeWeights = 0;
        foreach (var type in _types)
        {
            _accumulatedTypeWeights += type.Chance;
            type.Weight = _accumulatedTypeWeights;
        }

        _accumulatedColorWeights = 0;
        foreach (var color in _colors)
        {
            _accumulatedColorWeights += color.Chance;
            color.Weight = _accumulatedColorWeights;
        }
    }

    public ItemController GetItem()
    {
        ItemController newItem = GetRandomItem();
        if (newItem == null)
            return null;

        while (_grid.CheckItemInGird(newItem))
        {
            if (newItem != null)
                newItem.DestroyItem(false);

            newItem = GetRandomItem();
            Debug.Log("Item respawned!");
        }

        return newItem;
    }

    public ItemController GetItem(TypeNames type, ColorNames color)
    {
        ItemController itemToSpawn = _itemPool.GetItem();

        if (itemToSpawn == null)
            return null;

        if (type == TypeNames.None)
        {
            ItemType randomType = _types[GetRandomTypeIndex()];
            itemToSpawn.SetType(randomType.Type);
        }
        else itemToSpawn.SetType(type);

        if (color == ColorNames.None)
        {
            ItemColor randomColor = _colors[GetRandomColorIndex()];
            itemToSpawn.SetColor(randomColor.Name);
        }
        else itemToSpawn.SetColor(color);

        return itemToSpawn;
    }

    private ItemController GetRandomItem()
    {
        ItemController itemToSpawn = _itemPool.GetItem();

        if (itemToSpawn == null)
            return null;

        ItemType randomType = _types[GetRandomTypeIndex()];
        ItemColor randomColor = _colors[GetRandomColorIndex()];

        itemToSpawn.SetType(randomType.Type);
        itemToSpawn.SetColor(randomColor.Name);

        return itemToSpawn;
    }

    private int GetRandomTypeIndex()
    {
        int random = Random.Range(0, _accumulatedTypeWeights);
        for (int i = 0; i < _types.Length; i++)
        {
            if (_types[i].Weight >= random)
                return i;
        }
        return 0;
    }
    private int GetRandomColorIndex()
    {
        int random = Random.Range(0, _accumulatedColorWeights);
        for (int i = 0; i < _colors.Length; i++)
        {
            if (_colors[i].Weight >= random)
                return i;
        }
        return 0;
    }
}