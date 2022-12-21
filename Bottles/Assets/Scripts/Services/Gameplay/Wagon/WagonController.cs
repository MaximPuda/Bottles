using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class WagonController : Controller
{
    private ItemsCollector[] _collectors;

    private int _closedCount;
    private Animator _animator;

    public event UnityAction<int> BoxCloseEvent;
    public event UnityAction WagonCompletedEvent;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _animator = GetComponent<Animator>();

        _collectors = GetComponentsInChildren<ItemsCollector>();
        foreach (var colllector in _collectors)
        {
            colllector.Initialize();
            colllector.AllItemsCollectedEvent += OnBoxClose;
        }
    }

    private void OnDesable()
    {
        foreach (var box in _collectors)
            box.AllItemsCollectedEvent -= OnBoxClose;
    }

    private void OnBoxClose(int combo)
    {
        _closedCount++;
        BoxCloseEvent?.Invoke(combo);

        if (_closedCount == _collectors.Length)
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
