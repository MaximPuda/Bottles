using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyPaintBoxView : BoxView
{
    [SerializeField] private SpriteRenderer _main;
    [SerializeField] private SpriteRenderer _closed;
    [SerializeField] private SpriteRenderer _fill;
    [SerializeField] private ParticleSystem _filledFX;
    [SerializeField] private float[] _fillWaveTargets;
    [SerializeField] private bool _filledWaveFXEnable;

    private int _collectedItems = 0;
    private int _preinstalled = 0;

    private ItemColor _currentColor;
    
    private float _currentFillLevel = 0;
    private float _targetFillLevel;

    public override void Initialize(BoxController collector)
    {
        base.Initialize(collector);

        foreach (var cell in Cells)
        {
            if (!cell.IsEmpty)
            {
                OnItemAdded(cell.Item);
                _preinstalled++;
            }
        }
    }

    protected override void OnAllItemsCollected(int combo)
    {
        _main.enabled = false;
        _fill.enabled = false;

        Animator.SetTrigger("Close");
    }

    protected override void OnClearItems()
    {
        base.OnClearItems();

        _collectedItems = _preinstalled;
        _targetFillLevel = _preinstalled;

        _filledFX.Stop();
        StartCoroutine(ChangeFillLevel());
    }

    protected override void OnItemAdded(ItemController item)
    {
        base.OnItemAdded(item);

        item.Hide(true);

        if (_currentColor == null)
            _currentColor = item.Color;

        _collectedItems++;

        if (_currentColor.Name == ColorNames.Multi)
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

    private IEnumerator ChangeFillLevel()
    {
        float time = 0;
        _targetFillLevel = 1f / Cells.Length * _collectedItems;
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
