using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class LevelController : Controller
{
    [SerializeField] private PlayableDirector _levelIntro;
    [SerializeField] private ParticleSystemForceField _coinForceField;

    public LevelPrefs CurrentLevel { get; private set; }

    private PlayerController _playerController;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        if (GameManager.Instance.CurrentLevel != null)
        {
            CurrentLevel = Instantiate(GameManager.Instance.CurrentLevel);
            CurrentLevel.Intialize(_coinForceField);
            CurrentLevel.Wagon.WagonCompletedEvent += OnWagonCompleted;
        }
        else
        {
            Debug.LogWarning("Current Level is not setted!");
            return;
        }

        if (ServiceManager.TryGetService<PlayerService>(out PlayerService player))
        {
            _playerController = player.PlayerCTRL;
            _playerController.MovesEndedEvent += OnMovesEnded;
        }
    }
    public override void OnStart()
    {
        base.OnStart();

        if (_levelIntro)
            _levelIntro.Play();
    }

    private void OnDisable()
    {
        CurrentLevel.Wagon.WagonCompletedEvent -= OnWagonCompleted;
    }

    public void ShowWagon()
    {
        CurrentLevel.Wagon.OnStart();
    }
    public void ShowGrid()
    {
        CurrentLevel.Grid.ShowItems();
    }

    public void StartTutorial()
    {
        if (CurrentLevel.Tutorial != null)
            CurrentLevel.Tutorial.StartTutorial();
    }

    private void OnWagonCompleted()
    {
        GameManager.Instance.Win();
    }

    private void OnMovesEnded()
    {
        StartCoroutine(CheckLevelCompleteWithDelay());
    }

    private IEnumerator CheckLevelCompleteWithDelay()
    {
        yield return new WaitForSeconds(1f);
        if (!CurrentLevel.Wagon.IsCompleted)
            GameManager.Instance.Lose();
    }
}
