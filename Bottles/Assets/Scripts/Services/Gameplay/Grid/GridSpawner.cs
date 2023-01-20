using UnityEngine;

public class GridSpawner
{
    private ItemPool _itemPool;
    private ItemType[] _types;
    private ItemColor[] _colors;

    private int _accumulatedTypeWeights;
    private int _accumulatedColorWeights;

    public GridSpawner(ItemPool pool, ItemType[] itemTypes, ItemColor[] colors)
    {
        _types = itemTypes;
        _colors = colors;
        _itemPool = pool;

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

    public ItemController GetRandomItem()
    {
        ItemController itemToSpawn = _itemPool.GetItem();

        if (itemToSpawn == null)
            return null;

        ItemType randomType = _types[GetRandomTypeIndex()];
        ItemColor randomColor = _colors[GetRandomColorIndex()];

        itemToSpawn.SetView(randomType.Type);
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