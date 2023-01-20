using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] private int _itemsAmount;
    [SerializeField] private ItemType[] _itemTypes;
    [SerializeField] private ItemColor[] _itemColors;

    [Header("Prefabs")]
    [SerializeField] private ItemController _itemPrefab;

    [Header("Grid")]
    [SerializeField] private GridLine[] _cellsAmount;

    public int ItemsAmount => _itemsAmount;
    public ItemType[] ItemTypes => _itemTypes;
    public ItemColor[] ItemColors => _itemColors;
    public ItemController ItemPrefab => _itemPrefab;
}
