using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GridCellLocker : MonoBehaviour
{
    [SerializeField] private GameObject _locker;

    public event UnityAction UnlockedEvent;

    private bool _isEmpty = false;

    private void Update()
    {
        if (transform.childCount == 0)
            UnlockedEvent?.Invoke();
    }
}
