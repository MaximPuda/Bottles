using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Controller
{
    public Level CurrentLevel { get; private set; }

    private WagonController _currentWagon;
    public override void Initialize(Service service)
    {
        base.Initialize(service);

        CurrentLevel = GameManager.Instance.CurrentLevel;

        _currentWagon = ((GamePlayService)CurrentService).WagonCTRL;

        _currentWagon.WagonCompletedEvent += OnWagonCompleted;
    }

    private void OnDisable()
    {
        _currentWagon.WagonCompletedEvent -= OnWagonCompleted;
    }

    private void OnWagonCompleted()
    {
        GameManager.Instance.Win();
    }
}
