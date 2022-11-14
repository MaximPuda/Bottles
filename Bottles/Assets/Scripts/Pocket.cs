using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pocket : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnEnable()
    {
        foreach (var cell in _cells)
            cell.OnBottleAdd += TryCombo;
    }

    private void OnDisable()
    {
        foreach (var cell in _cells)
            cell.OnBottleAdd -= TryCombo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Bottle>(out Bottle bottle))
            TryAddBottle(bottle);
    }

    private bool TryAddBottle(Bottle bottle)
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            if(_cells[i].IsEmpty)
            {
                _cells[i].Addbottle(bottle);
                return true;
            }
        }

        return false;
    }

    private void TryCombo()
    {
        if (CheckIsFull())
        {
            GlobalEvents.SendOnBottleCombo(GetCombo());
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
