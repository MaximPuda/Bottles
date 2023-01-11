using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TempItemCollector : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 _positionOffset;
    private bool _isEmpty = true;

    private ItemController _currentItem;
    private Collider2D _collider; 

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            _isEmpty = true;
            _collider.enabled = true;
        }
    }

    public bool Interact(ItemController itemSender)
    {
        if (_isEmpty)
        {
            _currentItem = itemSender;
            _currentItem.transform.parent = transform;
            _currentItem.transform.localPosition = Vector3.zero + _positionOffset;
            _currentItem.OnTempCollect();
            _collider.enabled = false;
            _isEmpty = false;
            return true;
        }

        return false;
    }
}
