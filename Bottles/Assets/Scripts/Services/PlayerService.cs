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

    protected override void OnPlayEnter()
    {
        base.OnPlayEnter();

        _player.enabled = true;
    }

    protected override void OnLoseEnter()
    {
        base.OnLoseEnter();

        _player.enabled = false;
    }

    protected override void OnWinEnter()
    {
        base.OnWinEnter();

        _player.enabled = false;
    }

    protected override void OnPauseEnter()
    {
        base.OnPauseEnter();

        _player.enabled = false;
    }
}
