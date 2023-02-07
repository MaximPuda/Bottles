using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class WagonController : Controller
{
    [SerializeField] private Transform _container;
    [SerializeField] private ParticleSystem _winFX;
    [SerializeField] private ParticleSystemForceField _coinFXForceField;

    public bool IsCompleted { get; private set; }

    public event UnityAction WagonInEvent;
    public event UnityAction<int> BoxCloseEvent;
    public event UnityAction WagonCompletedEvent;
   
    private ItemsCollector[] _collectors;

    private int _closedCount;
    private Animator _animator;
    private Level _level;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _animator = GetComponent<Animator>();

        _level = ((GamePlayService)CurrentService).LevelCTRL.CurrentLevel;
        SetBoxes();

        _collectors = GetComponentsInChildren<ItemsCollector>();
        foreach (var colllector in _collectors)
        {
            colllector.Initialize();
            colllector.SetParticleForceField(_coinFXForceField);
            colllector.AllItemsCollectedEvent += OnBoxClose;
        }

        var  installers = GetComponentsInChildren<ItemPreInstaller>();
        foreach (var installer in installers)
        {
            installer.Initialize(_level);
        }
    }

    public override void OnStart()
    {
        base.OnStart();
        _animator.SetTrigger("In");
    }

    private void OnDesable()
    {
        foreach (var box in _collectors)
            box.AllItemsCollectedEvent -= OnBoxClose;
    }

    private void SetBoxes()
    {
        GameObject newBoxes = Instantiate(_level.BoxesPrefab, _container);
        newBoxes.transform.localPosition = Vector2.zero;
    }

    private void OnWagonIn()
    {
        WagonInEvent?.Invoke();
    }

    private void OnBoxClose(int combo)
    {
        _closedCount++;
        BoxCloseEvent?.Invoke(combo);

        if (_closedCount == _collectors.Length)
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
