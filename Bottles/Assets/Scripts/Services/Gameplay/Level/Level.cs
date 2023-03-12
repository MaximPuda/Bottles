using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    [Header("SETTINGS")]
    [SerializeField] private string _name;
    [SerializeField] private int _moves;
    [SerializeField] private TutorialList _tutorial;
    [SerializeField] private ItemType[] _itemTypes;
    [SerializeField] private ItemColor[] _itemColors;
    [Space(5)]

    [Header("PREFABS")]
    [SerializeField] private ItemController _itemPrefab;
    [SerializeField] private GameObject _boxesPrefab;
    [Space(5)]

    [Header("GRID")]
    [SerializeField] private GridLine[] _rows;

    public string LelvelName => _name;
    public int Moves => _moves;
    public TutorialList Tutorial => _tutorial;
    public ItemType[] ItemTypes => _itemTypes;
    public ItemColor[] ItemColors => _itemColors;
    public ItemController ItemPrefab => _itemPrefab;
    public GameObject BoxesPrefab => _boxesPrefab;
    public GridLine[] Grid => _rows;
}
