using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Controller
{
    [SerializeField] private Level _level;
    [SerializeField] private Spawner _spawner;

    public override void Initialize()
    {
        _spawner.Initialize(_level.BottlesAmount);
    }
}
