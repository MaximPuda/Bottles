using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyPaintBoxView : ItemsCollectorView
{
    [SerializeField] private SpriteRenderer _main;
    [SerializeField] private SpriteRenderer _closed;
    [SerializeField] private SpriteRenderer _fill;
    [SerializeField] private ParticleSystem _filledFX;
    [SerializeField] private float[] _fillWaveTargets;
    [SerializeField] private bool _filledWaveFXEnable;

    private Cell[] _cells;
    private int _collectedItems = 0;
    private ItemColor _currentColor;
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
                if (i == 0 || _currentColor.Name == ColorsName.Multi)
                    _currentColor = item.Color;

                _collectedItems++;
                _cells[i].AddItem(item);
                _cells[i].HideItem();

                if(_currentColor.Name == ColorsName.Multi)
                {
                    _fill.material.SetFloat("_Multi", 1);
                }
                else
                {
                    _fill.material.SetFloat("_Multi", 0);
                    _fill.material.color = _currentColor.Color;
                    _filledFX.startColor = _currentColor.Color;
                }
                
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
        float rotation = 0f;
        int rotationTargetIndex = 0;
        while (time < 1)
        {
            _currentFillLevel = Mathf.Lerp(_currentFillLevel, _targetFillLevel, time);
            _fill.material.SetFloat("_Level", _currentFillLevel);
            
            if (_filledWaveFXEnable)
            {
                rotation = Mathf.Lerp(rotation, _fillWaveTargets[rotationTargetIndex], time);
                    
                _fill.material.SetFloat("_Rotation", rotation);

                if (Mathf.Abs(_fillWaveTargets[rotationTargetIndex] - rotation) < 0.01f)
                    if (rotationTargetIndex < _fillWaveTargets.Length - 1)
                        rotationTargetIndex++;
            }
            
            time = time + Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
