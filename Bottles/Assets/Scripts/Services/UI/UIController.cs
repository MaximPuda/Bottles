using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Controller
{
    [SerializeField] private UIView _view;

    private ScoreController _score;
    private PlayerController _playerController;
    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _view.IntitializeHUD();

        if (ServiceManager.TryGetService<GamePlayService>(out GamePlayService gamePlay))
        {
            _score = gamePlay.ScoreCTRL;
            _score.PonintsChangedEvent += UpdatePoints;
        }

        if (ServiceManager.TryGetService<PlayerService>(out PlayerService player))
        {
            _playerController = player.PlayerCTRL;
            _playerController.MovesLeftEvent += UpdateMoves;
        }
    }

    private void OnDisable()
    {
        _score.PonintsChangedEvent -= UpdatePoints;
        _playerController.MovesLeftEvent -= UpdateMoves;
    }

    private void UpdatePoints(int points)
    {
        _view.UpdatePoints(points);
    }

    private void UpdateMoves(int amount)
    {
        _view.UpdateMoves(amount);
    }

    public void ShowLoseScreen()
    {
        _view.ShowLoseScreen(true);
    }

    public void ShowWinScreen()
    {
        _view.ShowWinScreen(true, _score.Points);
    }

    public void OnRestartBtnClick()
    {
        _view.ShowLoseScreen(false);
        GameManager.Instance.Restart();
    }

    public void OnBackBtnClick()
    {
        _view.ShowLoseScreen(false);
        GameManager.Instance.BackToMenu();
    }

    public void OnAddMovesClick()
    {
        _view.ShowLoseScreen(false);
        GameManager.Instance.Continue();
    }

    public void OnPauseBtnClick()
    {
        GameManager.Instance.Pause();
    }
}