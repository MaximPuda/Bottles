using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyPaintBoxView : ItemsCollectorView
{
    [SerializeField] private SpriteRenderer _main;
    [SerializeField] private SpriteRenderer _closed;
    [SerializeField] private SpriteRenderer _fill;
    [SerializeField] private ParticleSystem _filledFX;

    private Cell[] _cells;
    private int _collectedItems = 0;
    private float _currentFillLevel = 0;
    private float _targetFillLevel;

    public override void Initialize(ItemsCollector collector)
    {
        base.Initialize(collector);

        _cells = GetComponentsInChildren<Cell>();
    }

    protected override void OnAllItemsCollected(int combo)
    {
        _main.enabled = false;
        _fill.enabled = false;

        Animator.SetTrigger("Close");
    }

    protected override void OnClearItems()
    {
        foreach (var cell in _cells)
            if (cell != null)
            {
                cell.RemoveItem();
                _collectedItems = 0;
                _targetFillLevel = 0;

                _filledFX.Stop();
                StartCoroutine(ChangeFillLevel());
            }
    }

    protected override void OnItemAdded(ItemController item)
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            if (_cells[i].IsEmpty)
            {
                _collectedItems++;
                _cells[i].AddItem(item);
                _cells[i].HideItem();

                _fill.material.color = item.Color.Color;
                _filledFX.startColor = item.Color.Color;
                
                StartCoroutine(ChangeFillLevel());
                _filledFX.Play();

                return;
            }
        }
    }

    private IEnumerator ChangeFillLevel()
    {
        float time = 0;
        _targetFillLevel = 1f / _cells.Length * _collectedItems;
        while (time < 1)
        {
            _currentFillLevel = Mathf.Lerp(_currentFillLevel, _targetFillLevel, time);
            _fill.material.SetFloat("_Level", _currentFillLevel);
            time = time + Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
