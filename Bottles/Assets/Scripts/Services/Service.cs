using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Service : MonoBehaviour
{
    public bool isReady { get; private set; }

    public event UnityAction InitFinishedEvent;

    public virtual void Initialize()
    {
        InitAllControllers();
        
        GameManager.Instance.MenuEnterEvent += OnMenuEnter;
        GameManager.Instance.PlayEnterEvent += OnPlayEnter;
        GameManager.Instance.PauseEnterEvent += OnPauseEnter;
        GameManager.Instance.WinEnterEvent += OnWinEnter;
        GameManager.Instance.LoseEnterEvent += OnLoseEnter;
        
        isReady = true;
        InitFinishedEvent?.Invoke();
        Debug.Log(this.ToString() + " is intiialized!");
    }

    protected abstract void InitAllControllers();

    private void OnDestroy()
    {
        ServiceManager.DetachService(this);
        GameManager.Instance.MenuEnterEvent -= OnMenuEnter;
        GameManager.Instance.PlayEnterEvent -= OnPlayEnter;
        GameManager.Instance.PauseEnterEvent -= OnPauseEnter;
        GameManager.Instance.WinEnterEvent -= OnWinEnter;
        GameManager.Instance.LoseEnterEvent -= OnLoseEnter;
    }

    protected virtual void OnMenuEnter() { }
    protected virtual void OnPlayEnter() { Debug.Log(this.ToString() + " in play mode!"); }
    protected virtual void OnPauseEnter() { Debug.Log(this.ToString() + " in pause!"); }
    protected virtual void OnWinEnter() { }
    protected virtual void OnLoseEnter() { }
}
