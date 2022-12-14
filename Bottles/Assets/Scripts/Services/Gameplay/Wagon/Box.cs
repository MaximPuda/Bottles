using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(Animator))]
public class Box : MonoBehaviour, ICollectable
{
    [SerializeField] private Cell[] _cells;

    public event UnityAction OnBoxClose;

    private Collider2D _collider;
    private Animator _animator;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        _animator = GetComponent<Animator>();
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

    public bool TryAddBottle(Bottle bottle)
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            if(_cells[i].IsEmpty)
            {
                _cells[i].Addbottle(bottle);
                bottle.IsCollected = true;
                return true;
            }
        }

        return false;
    }

    private void TryCombo()
    {
        if (CheckIsFull())
        {
            int combo = GetCombo();
            EventBus.SendOnBottleCombo(combo);

            if (combo > 0)
            {
                _animator.SetTrigger("Close");
                HideBottles();
                OnBoxClose?.Invoke();
            }    
            else
                Clear();
        }
    }

    private bool CheckIsFull()
    {
        int count = 0;
        foreach (var cell in _cells)
        {
            if(!cell.IsEmpty)
                count++;
        }

        return count == _cells.Length;
    }

    private int GetCombo()
    {
        int shapeMatch = 1;
        int colorMatch = 1;
        int combo = 0;

        Shapes currentShape = _cells[0].CurrentBottle.CurrentShape;
        Colors currentColor = _cells[0].CurrentBottle.CurrentColor;

        for (int i = 1; i < _cells.Length; i++)
        {
            if (currentShape == Shapes.All)
                currentShape = _cells[i].CurrentBottle.CurrentShape;

            if(currentColor == Colors.All)
                currentColor = _cells[i].CurrentBottle.CurrentColor;

            if(_cells[i].CurrentBottle.CurrentShape == currentShape || _cells[i].CurrentBottle.CurrentShape == Shapes.All)
                shapeMatch++;

            if(_cells[i].CurrentBottle.CurrentColor == currentColor || _cells[i].CurrentBottle.CurrentColor == Colors.All)
                colorMatch++;
        }

        if(shapeMatch == _cells.Length)
            combo++;

        if(colorMatch == _cells.Length)
            combo++;

        return combo;
    }

    private void HideBottles()
    {
        foreach (var cell in _cells)
            cell.HideBottle();
    }

    private void Clear()
    {
        foreach (var cell in _cells)
            cell.RemoveBottle();
    }
}
