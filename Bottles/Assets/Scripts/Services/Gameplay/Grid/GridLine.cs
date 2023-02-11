using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridLine
{
    [SerializeField] private GridCellSettings[] _cells;

    public GridCellSettings[] Cells => _cells;
    public int Amount => _cells.Length;
}