using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform _dragable;
    [SerializeField] private ItemType _type;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _main;
    [SerializeField] private SpriteRenderer _outline;
    [SerializeField] private SpriteRenderer _fill;
    [SerializeField] private SpriteRenderer _multi;
    [SerializeField] private SpriteRenderer _back;

    public ItemType Type => _type;
    public Transform Dragable => _dragable;
    
    public void OnChangeColor(Color color) => _fill.color = color;

    public void EnableFill(bool enable) => _fill.enabled = enable;

    public void EnableMulti(bool enable)
    {
        if (_multi != null) 
            _multi.enabled = enable;
    }

    public void OnActive(bool active)
    {
        _outline.enabled = active;
        if (active)
            ChangeSortingLayer("Drag");
        else
            ChangeSortingLayer("Default");
    }

    private void ChangeSortingLayer(string layerName)
    {
        _main.sortingLayerName = layerName;
        _fill.sortingLayerName = layerName;
        _back.sortingLayerName = layerName;
        _multi.sortingLayerName = layerName;
    }

    public void OnDestroyItem()
    {
        _main.enabled = false;
        _fill.enabled = false;
        _back.enabled = false;
        _outline.enabled = false;
    }
}
