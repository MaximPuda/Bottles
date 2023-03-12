using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelController : Controller
{
    [SerializeField] private PlayableDirector _levelIntro;
    [SerializeField] private TutorialManager _tutManager;
    public Level CurrentLevel { get; private set; }

    private PlayerController _playerController;

    private WagonController _currentWagon;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        CurrentLevel = GameManager.Instance.CurrentLevel;

        _currentWagon = ((GamePlayService)CurrentService).WagonCTRL;
        _currentWagon.WagonCompletedEvent += OnWagonCompleted;

        if (ServiceManager.TryGetService<PlayerService>(out PlayerService player))
        {
            _playerController = player.PlayerCTRL;
            _playerController.MovesEndedEvent += OnMovesEnded;
        }

        _tutManager.Initialize();
        _tutManager.SetTutorial(CurrentLevel.Tutorial);
    }

    private void OnDisable()
    {
        _currentWagon.WagonCompletedEvent -= OnWagonCompleted;
    }

    private void OnWagonCompleted()
    {
        GameManager.Instance.Win();
    }

    private void OnMovesEnded()
    {
        StartCoroutine(CheckLevelCompleteWithDelay());
    }

    public override void OnStart()
    {
        base.OnStart();
     
        if (_levelIntro)
            _levelIntro.Play();
    }

    private IEnumerator CheckLevelCompleteWithDelay()
    {
        yield return new WaitForSeconds(1f);
        if (!_currentWagon.IsCompleted)
            GameManager.Instance.Lose();
    }
}
