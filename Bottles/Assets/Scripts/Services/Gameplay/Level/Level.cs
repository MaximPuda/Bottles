using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] private int _itemsAmount;
    [SerializeField] private ItemType[] _itemTypes;
    [SerializeField] private ColorPalette[] _palettes;

    public int ItemsAmount => _itemsAmount;
    public ItemType[] ItemTypes => _itemTypes;
    public ColorPalette[] ColorPalettes => _palettes;
}
