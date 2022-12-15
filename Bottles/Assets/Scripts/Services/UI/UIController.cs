using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Controller
{
    [SerializeField] private UIView _view;
    [SerializeField] private ScoreController _scoreSystem;

    private ScoreController _score; 
    public override void Initialize(Service service)
    {
        base.Initialize(service);

        if (ServiceManager.TryGetService<GamePlayService>(out GamePlayService gamePlay))
            if (gamePlay.TryGetController<ScoreController>(out _score))
                _score.PonintsChangedEvent += UpdatePoints;

    }

    private void OnDisable()
    {
        _score.PonintsChangedEvent += UpdatePoints;

        //EventBus.OnBottleSpawn -= UpdateBottlesAmount;
        //EventBus.OnGameOver -= ShowGameOverScreen;
    }

    private void Start()
    {
        _view.IntitializeHUD();
    }

    private void UpdatePoints(int points)
    {
        _view.UpdatePoints(points);
    }

    private void UpdateBottlesAmount(int amount)
    {
        _view.UpdateBottlesAmount(amount);
    }

    private void ShowGameOverScreen()
    {
        _view.ShowGameOverScreen(true);
    }

    public void OnRestartBtnClick() => GameManager.Instance.Restart();

    public void OnBackBtnClick() => GameManager.Instance.BackToMenu();

    public void OnPauseBtnClick() => GameManager.Instance.Pause();
}