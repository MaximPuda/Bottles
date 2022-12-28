using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Add new level", order = 1)]
public class Level : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] private int _itemsAmount;
    [SerializeField] private ItemType[] _itemTypes;
    [SerializeField] private ColorPalette[] _palettes;

    [Header("Targets")]
    [SerializeField] private Target[] _targets = new Target[3];

    public int ItemsAmount => _itemsAmount;
    public ItemType[] ItemTypes => _itemTypes;
    public ColorPalette[] ColorPalettes => _palettes;
}
