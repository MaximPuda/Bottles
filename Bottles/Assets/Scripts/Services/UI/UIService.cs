using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIService : Service
{
    [SerializeField] private UIController _uiController;

    public UIController UiCTRL => _uiController;

    protected override void InitAllControllers()
    {
        _uiController.Initialize(this);
        _uiController.OnStart();
    }

    protected override void OnWinEnter()
    {
        base.OnWinEnter();
        UiCTRL.ShowWinScreen();
    }

    protected override void OnLoseEnter()
    {
        base.OnLoseEnter();
        UiCTRL.ShowLoseScreen();
    }
}
