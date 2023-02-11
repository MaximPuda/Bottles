using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIService : Service
{
    [SerializeField] private MenuUIController _ui;

    public MenuUIController MenuUICTRL => _ui;
    protected override void InitAllControllers()
    {
        _ui.Initialize(this);

        _ui.OnStart();
    }
}
