using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GridCellLocker : MonoBehaviour
{
    public event UnityAction UnlockedEvent;

    private int _childCount;
    private bool _isEmpty = true;

    private void Update()
    {
        _childCount = transform.childCount;

        if (_isEmpty && _childCount > 0)
            _isEmpty = false;

        if (!_isEmpty && transform.childCount == 0)
            UnlockedEvent?.Invoke();
    }
}
