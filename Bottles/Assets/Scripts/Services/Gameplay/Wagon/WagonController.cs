using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class WagonController : Controller
{
    private Box[] _boxes;

    private int _closedCount;
    private Animator _animator;

    public event UnityAction<int> BoxCloseEvent;
    public event UnityAction WagonCompletedEvent;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _animator = GetComponent<Animator>();

        _boxes = GetComponentsInChildren<Box>();
        foreach (var box in _boxes)
        {
            box.Initialize();
            box.BoxCloseEvent += OnBoxClose;
        }
    }

    private void OnDesable()
    {
        foreach (var box in _boxes)
            box.BoxCloseEvent -= OnBoxClose;
    }

    private void OnBoxClose(int combo)
    {
        _closedCount++;
        BoxCloseEvent?.Invoke(combo);

        if (_closedCount == _boxes.Length)
        {
            _animator.SetTrigger("Completed");
        }
    }

    public void OnWagonComplited()
    {
        WagonCompletedEvent?.Invoke();

        Debug.Log("Wagon is done!");
    }
}
