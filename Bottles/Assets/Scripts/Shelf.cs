using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;

    private void OnEnable()
    {
        foreach (var cell in _cells)
        {
            cell.OnBottleAdd += TryCombo;
        }
    }

    private void OnDisable()
    {
        foreach (var cell in _cells)
        {
            cell.OnBottleAdd -= TryCombo;
        }
    }

    private void TryCombo()
    {
        if (CheckIsFull())
        {
            int combo = GetCombo();
            Debug.Log(combo);
            Clear();
        }
    }

    private bool CheckIsFull()
    {
        int count = 0;
        foreach (var cell in _cells)
        {
            if (!cell.IsEmpty)
                count++;
        }

        return count == _cells.Length;
    }

    private int GetCombo()
    {
        int combo = 0;

        if (_cells[0].CurrentBottle.Shape == _cells[1].CurrentBottle.Shape &&
            _cells[1].CurrentBottle.Shape == _cells[2].CurrentBottle.Shape)
            combo++;

        if (_cells[0].CurrentBottle.Color == _cells[1].CurrentBottle.Color &&
            _cells[1].CurrentBottle.Color == _cells[2].CurrentBottle.Color)
            combo++;

        return combo;
    }

    private void Clear()
    {
        foreach (var cell in _cells)
        {
            cell.RemoveBottle();
        }
    }
}
