using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService : Service
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private PlayerData _data;

    public PlayerController PlayerCTRL => _player;
    public PlayerData PlayerDataCTRL => _data;

    protected override void InitAllControllers()
    {
        _player.Initialize(this);
        _data.Initialize(this);

        _player.OnStart();
        _data.OnStart();
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

    protected override void OnMenuEnter()
    {
        base.OnMenuEnter();

        _data.SaveData();
    }
}