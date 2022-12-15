using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Controller
{
    public Level CurrentLevel { get; private set; }

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        CurrentLevel = GameManager.Instance.CurrentLevel;
    }
}
