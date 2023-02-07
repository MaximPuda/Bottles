using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    private ItemPool _itemPool;
    private ItemType[] _types;
    private ItemColor[] _palettes;

    private Transform _container;
    private int _showItemsAmount;

    private const float TIME_BETWEEN_SPAWN = 0.1f;
    private float _currentTime = 0;

    public void Initialize(ItemPool pool, int showItemsAmount, Transform container, ItemType[] itemTypes, ItemColor[] palettes)
    {
        _showItemsAmount = showItemsAmount;
        _types = itemTypes;
        _palettes = palettes;
        _container = container;
        _itemPool = pool;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_itemPool.Count > 0 && _container.childCount < _showItemsAmount)
        {
            if (_currentTime >= TIME_BETWEEN_SPAWN)
            {
                SpawnRandom(_container);
                _currentTime = 0;
            }
        }
    }

    private void SpawnRandom(Transform parent)
    {
        int typeIndex = Random.Range(0, _types.Length);
        int colorIndex = Random.Range(0, _palettes.Length);
        
        ItemController itemToSpawn = _itemPool.GetItem();
        itemToSpawn.transform.position = transform.position;
        itemToSpawn.transform.parent = _container;
        itemToSpawn.SetType(_types[typeIndex].Type);
        itemToSpawn.SetColor(_palettes[colorIndex].Name);
    }
}