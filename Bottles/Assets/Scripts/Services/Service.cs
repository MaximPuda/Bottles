using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Service : MonoBehaviour
{
    [SerializeField] private List<Controller> _controllers;

    public bool isReady { get; private set; }

    public virtual void Initialize()
    {
        InitAllControllers();
        GameManager.Instance.MenuEnterEvent += OnMenuEnter;
        GameManager.Instance.PlayEnterEvent += OnPlayEnter;
        GameManager.Instance.PauseEnterEvent += OnPauseEnter;
        GameManager.Instance.WinEnterEvent += OnWinEnter;
        GameManager.Instance.LoseEnterEvent += OnLoseEnter;
        isReady = true;
        Debug.Log(this.ToString() + " is intiialized!");
    }

    private void OnDestroy()
    {
        ServiceManager.DetachService(this);
        GameManager.Instance.MenuEnterEvent -= OnMenuEnter;
        GameManager.Instance.PlayEnterEvent -= OnPlayEnter;
        GameManager.Instance.PauseEnterEvent -= OnPauseEnter;
        GameManager.Instance.WinEnterEvent -= OnWinEnter;
        GameManager.Instance.LoseEnterEvent -= OnLoseEnter;
    }

    public bool TryGetController<T>(out T controller) where T :  Controller
    {
        foreach (var cont in _controllers)
        {
            if (cont.GetType() == typeof(T))
            {
                controller = (T)cont;
                return true;
            }
        }

        controller = null;
        return false;
    }

    protected virtual void InitAllControllers()
    {
        foreach (var controller in _controllers)
            controller.Initialize();

        foreach (var controller in _controllers)
            controller.OnStart();
    }

    protected virtual void OnMenuEnter() { }
    protected virtual void OnPlayEnter() { Debug.Log(this.ToString() + " in play mode!"); }
    protected virtual void OnPauseEnter() { Debug.Log(this.ToString() + " in pause!"); }
    protected virtual void OnWinEnter() { }
    protected virtual void OnLoseEnter() { }
}
