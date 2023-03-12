using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform _dragable;
    [SerializeField] private TypeNames _type;
    [SerializeField] private ItemColor _color;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _main;
    [SerializeField] private SpriteRenderer _outline;
    [SerializeField] private SpriteRenderer _fill;
    [SerializeField] private SpriteRenderer _multi;
    [SerializeField] private SpriteRenderer _back;

    public TypeNames Type => _type;
    public ItemColor Color => _color;
    public Transform Dragable => _dragable;
    
    public void OnChangeColor(ItemColor color)
    {
        _color = color;
        _fill.color = color.Color;
    }

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

    public void OnLock(bool locked)
    {
        if (locked)
            ChangeSortingLayer("Lock");
        else ChangeSortingLayer("Default");
    }    

    private void ChangeSortingLayer(string layerName)
    {
        _main.sortingLayerName = layerName;
        _fill.sortingLayerName = layerName;
        _back.sortingLayerName = layerName;
        _multi.sortingLayerName = layerName;
    }

    public void Reset() => EnableAllRanderers(true);

    public void OnDestroyItem() => EnableAllRanderers(false);

    private void EnableAllRanderers(bool enable)
    {
        _main.gameObject.SetActive(enable);
        _fill.gameObject.SetActive(enable);
        _back.gameObject.SetActive(enable);
        _multi.gameObject.SetActive(enable);
        _outline.gameObject.SetActive(enable);
    }
}
