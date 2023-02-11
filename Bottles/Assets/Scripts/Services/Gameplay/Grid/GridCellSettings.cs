using UnityEngine;

[System.Serializable]
public class GridCellSettings 
{
    [SerializeField] private Transform _locker;
    [SerializeField] private TypeNames _itemType;
    [SerializeField] private ColorNames _itemColor;

    public Transform Locker => _locker;
    public TypeNames ItemType => _itemType;
    public ColorNames ItemColor => _itemColor;
}
