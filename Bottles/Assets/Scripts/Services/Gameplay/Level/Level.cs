using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] private string _name;
    [SerializeField] private int _moves;
    [SerializeField] private ItemType[] _itemTypes;
    [SerializeField] private ItemColor[] _itemColors;

    [Header("Prefabs")]
    [SerializeField] private ItemController _itemPrefab;
    [SerializeField] private GameObject _boxesPrefab;

    [Header("Grid")]
    [SerializeField] private GridLine[] _cellsAmount;

    public string LelvelName => _name;
    public int Moves => _moves;
    public ItemType[] ItemTypes => _itemTypes;
    public ItemColor[] ItemColors => _itemColors;
    public ItemController ItemPrefab => _itemPrefab;
    public GameObject BoxesPrefab => _boxesPrefab;
    public GridLine[] Grid => _cellsAmount;
}
