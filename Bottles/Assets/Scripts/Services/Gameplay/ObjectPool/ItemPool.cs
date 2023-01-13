using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPool
{
    public string Name { get; private set; }
    public int Capacity { get; private set; }
    public ItemController ItemPrefab { get; private set; }
    public Transform Container { get; private set; }
    public Transform Parent { get; private set; }
    public int Count => _pool.Count;

    public event UnityAction<int> PoolChangeEvent;

    public event UnityAction PoolEmptyEvent;

    private Queue<ItemController> _pool = new();

    public ItemPool(string name, int capacity, ItemController itemPrefab, Transform parent)
    {
        Name = name;
        Capacity = capacity;
        Parent = parent;
        if (itemPrefab != null)
            ItemPrefab = itemPrefab;
        else
            Debug.LogWarning("No prefab!");

        Container = new GameObject(Name).transform;
        Container.parent = Parent;
        Container.gameObject.SetActive(false);

        for (int i = 0; i < capacity; i++)
        {
            ItemController newItem = GameObject.Instantiate(ItemPrefab);
            newItem.transform.parent = Container;
            
            PutItem(newItem);
        }
    }

    public void PutItem(ItemController item)
    {
        item.transform.parent = Container;
        item.gameObject.SetActive(false);
        
        _pool.Enqueue(item);

        PoolChangeEvent?.Invoke(_pool.Count);
    }

    public ItemController GetItem()
    {
        if (_pool.Count > 0)
        {
            ItemController item = _pool.Dequeue();

            item.gameObject.SetActive(true);

            PoolChangeEvent?.Invoke(_pool.Count);

            if (_pool.Count == 0)
                PoolEmptyEvent?.Invoke();

            return item;
        }

        Debug.LogWarning(_pool + " is empty!");
        return null;
    }
}
