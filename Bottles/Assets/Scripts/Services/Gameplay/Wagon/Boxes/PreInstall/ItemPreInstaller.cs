using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemsCollector))]
public class ItemPreInstaller : MonoBehaviour
{
    [SerializeField] private ItemSettings[] _preinstallItems;

    private ItemsCollector _collector;
    private ItemController _itemPrefab;
    private Level _level;

    public void Initialize(Level level)
    {
        _collector = GetComponent<ItemsCollector>();
        _level = level;
        _itemPrefab = level.ItemPrefab;

        InstallItem();
    }

    private void InstallItem()
    {
        if (_collector == null || _itemPrefab == null || _preinstallItems == null)
            return;
        for (int i = 0; i < _preinstallItems.Length; i++)
        {
            var newItem = Instantiate(_itemPrefab);
            newItem.SetType(_preinstallItems[i].ItemType);
            newItem.SetColor(_preinstallItems[i].ItemColor);

            _collector.Interact(newItem);
        }
    }
}
