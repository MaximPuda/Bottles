using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _minDistanceBetweenBottles = 0.9f;

    private Item[] _items;
    private ColorPalette[] _palettes;

    public int ItemsAmount { get; private set; }

    public event UnityAction ItemSpawnedEvent;
    public event UnityAction<bool> ActivationEvent;

    private Transform _container;
    private Transform _prevBottle;
    private int _showItemsAmount;
    private bool _isStop;

    public void Initialize(int bottlesAmount, int showItemsAmount, Transform container, Item[] bottles, ColorPalette[] palettes)
    {
        ItemsAmount = bottlesAmount;
        _showItemsAmount = showItemsAmount;
        _items = bottles;
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
        int colorIndex = Random.Range(0, _palettes.Length);
        int bottlesIndex = Random.Range(0, _items.Length);
        Vector3 newPos = transform.position;
        GameObject newBottle = Instantiate(_items[bottlesIndex].gameObject, newPos, Quaternion.identity);
        newBottle.transform.parent = parent;

        _prevBottle = newBottle.transform;

        Item bottle = newBottle.GetComponent<Item>();
        bottle.SetColor(_palettes[colorIndex].ColorName);

        ItemSpawnedEvent?.Invoke();
    }

    private void Stop() => _isStop = true;
}