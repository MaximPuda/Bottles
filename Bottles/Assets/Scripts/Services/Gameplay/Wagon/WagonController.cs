using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class WagonController : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private ParticleSystem _winFX;

    public bool IsCompleted { get; private set; }

    public event UnityAction WagonInEvent;
    public event UnityAction WagonOutEvent;
    public event UnityAction<int> BoxCloseEvent;
    public event UnityAction WagonCompletedEvent;


    private BoxController[] _boxes;

    private int _closedCount;
    private Animator _animator;
    private Level _level;

    public  void Initialize(ParticleSystemForceField coinFXForceField)
    {
        _animator = GetComponent<Animator>();

        _boxes = GetComponentsInChildren<BoxController>();
        foreach (var box in _boxes)
        {
            box.Initialize();
            box.SetParticleForceField(coinFXForceField);
            box.AllItemsCollectedEvent += OnBoxClose;
        }

        //var  installers = GetComponentsInChildren<ItemPreInstaller>();
        //foreach (var installer in installers)
        //{
        //    installer.Initialize(_level);
        //}
    }

    public void OnStart()
    {
        _animator.SetTrigger("In");
    }

    private void OnDesable()
    {
        foreach (var box in _boxes)
            box.AllItemsCollectedEvent -= OnBoxClose;
    }

    private void OnWagonIn()
    {
        WagonInEvent?.Invoke();
    }

    private void OnWagonOut()
    {
        WagonOutEvent?.Invoke();
    }

    private void OnBoxClose(int combo)
    {
        _closedCount++;
        BoxCloseEvent?.Invoke(combo);

        if (_closedCount == _boxes.Length)
        {
            IsCompleted = true;
            _animator.SetTrigger("Completed");

            if (_winFX != null)
                _winFX.Play();
            else Debug.Log("WinFX is not assigned in " + name);
        }
    }

    public void OnWagonComplited()
    {
        WagonCompletedEvent?.Invoke();
    }
}
