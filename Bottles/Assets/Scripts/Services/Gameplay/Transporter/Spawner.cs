using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField] private ItemController _prefab;
    [SerializeField] private float _minDistanceBetweenBottles = 0.9f;

    private ItemType[] _types;
    private ColorPalette[] _palettes;

    public int ItemsAmount { get; private set; }

    public event UnityAction ItemSpawnedEvent;
    public event UnityAction<bool> ActivationEvent;

    private Transform _container;
    private Transform _prevBottle;
    private int _showItemsAmount;
    private bool _isStop;

    public void Initialize(int itemsAmount, int showItemsAmount, Transform container, ItemType[] itemTypes, ColorPalette[] palettes)
    {
        ItemsAmount = itemsAmount;
        _showItemsAmount = showItemsAmount;
        _types = itemTypes;
        _palettes = palettes;
        _container = container;
    }

    private void Update()
    {
        if (!_isStop)
        {
            if (ItemsAmount > 0 && (_prevBottle == null || Vector3.Magnitude(transform.position - _prevBottle.position) >= _minDistanceBetweenBottles)
                && _container.transform.childCount < _showItemsAmount)
                SpawnRandom(_container.transform);
        }
    }

    private void SpawnRandom(Transform parent)
    {
        int typeIndex = Random.Range(0, _types.Length);
        int colorIndex = Random.Range(0, _palettes.Length);
        Vector3 newPos = transform.position;
        GameObject newItem = Instantiate(_prefab.gameObject, newPos, Quaternion.identity);
        newItem.transform.parent = parent;

        _prevBottle = newItem.transform;

        ItemController item = newItem.GetComponent<ItemController>();
        item.SetView(_types[typeIndex]);
        item.SetColor(_palettes[colorIndex].ColorName);

        ItemSpawnedEvent?.Invoke();
    }

    private void Stop() => _isStop = true;
}