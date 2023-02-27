using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxController))]
public class ItemPreInstaller : MonoBehaviour
{
    [SerializeField] private ItemSettings[] _settings;

    private BoxController _collector;
    private ItemController _itemPrefab;
    private Level _level;

    public void Initialize(Level level)
    {
        _collector = GetComponent<BoxController>();
        _level = level;
        _itemPrefab = level.ItemPrefab;

        InstallItem();
    }

    private void InstallItem()
    {
        if (_collector == null || _itemPrefab == null || _settings == null)
            return;

        for (int i = 0; i < _settings.Length; i++)
        {
            var set = _settings[i];
            var newItem = Instantiate(_itemPrefab, transform);

            newItem.transform.localPosition = set.StartPosition;
            newItem.SetType(set.ItemType);
            newItem.SetColor(set.ItemColor);

            _collector.Interact(newItem);
        }
    }
}
