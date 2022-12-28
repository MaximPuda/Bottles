using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TempItemCollector : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 _positionOffset;
    private bool _isEmpty = true;
    public bool Interact(ItemController itemSender)
    {
        if (transform.childCount == 0)
            _isEmpty = true;

        if (_isEmpty)
        {
            itemSender.transform.parent = transform;
            itemSender.transform.localPosition = Vector3.zero + _positionOffset;
            itemSender.OnTempCollect();
            _isEmpty = false;
            return true;
        }

        return false;
    }
}
