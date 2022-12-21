using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService : Service
{
    [SerializeField] private PlayerController _player;

    public PlayerController PlayerCTRL => _player;

    protected override void InitAllControllers()
    {
        _player.Initialize(this);
        _player.OnStart();
    }
}
