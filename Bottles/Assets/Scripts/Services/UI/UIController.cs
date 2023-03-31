using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Controller
{
    [SerializeField] private UIView _view;

    private ScoreController _score;
    private PlayerController _playerController;
    private PlayerData _playerData;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _view.IntitializeHUD();
        _view.SetLevelName(GameManager.Instance.CurrentLevel.LevelName);

        if (ServiceManager.TryGetService<GamePlayService>(out GamePlayService gamePlay))
        {
            _score = gamePlay.ScoreCTRL;
            _score.PointsChangedEvent += UpdatePoints;
        }

        if (ServiceManager.TryGetService<PlayerService>(out PlayerService player))
        {
            _playerController = player.PlayerCTRL;
            _playerController.MovesLeftEvent += UpdateMoves;

            _playerData = player.PlayerDataCTRL;
            _playerData.DataChangedEvent += UpdateLifes;
        }
    }

    private void OnDisable()
    {
        _score.PointsChangedEvent -= UpdatePoints;
        _playerController.MovesLeftEvent -= UpdateMoves;
        _playerData.DataChangedEvent -= UpdateLifes;
    }

    private void UpdatePoints(int points)
    {
        _view.UpdatePoints(points);
    }

    private void UpdateLifes()
    {
        _view.UpdateLifes(_playerData.Lifes, _playerData.MaxLifes, _playerData.SecondsLeft);
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

    public void HideLoading()
    {
        LoadingViewer.Instance.Out();
    }

    public void OnRestartBtnClick()
    {
        _view.ShowLoseScreen(false);
        GameManager.Instance.Restart();
    }

    public void OnContinueBtnClick()
    {
        GameManager.Instance.PlayNextLevel();
    }

    public void OnBackBtnClick(Button sender)
    {
        sender.interactable = false;
        _view.ShowLoseScreen(false);
        GameManager.Instance.BackToMenu();
    }

    public void OnAddMovesClick(Button sender)
    {
       sender.interactable = false;
        _view.ShowLoseScreen(false);
        GameManager.Instance.Continue();
        sender.gameObject.SetActive(false);
    }

    public void OnPauseBtnClick(Button sender)
    {
        sender.interactable = true;
        GameManager.Instance.Pause(true);
        _view.ShowPauseScreen(true);
    }

    public void OnPauseContinueBtnClick(Button sender)
    {
        sender.interactable = false;
        GameManager.Instance.Pause(false);
        _view.ShowPauseScreen(false);
    }

    public void OnPauseRestartBtnClick(Button sender)
    {
        sender.interactable = false;
        _view.ShowPauseScreen(false);
        GameManager.Instance.Pause(false);
        GameManager.Instance.Restart();
    }

    public void OnPauseBackBtnClick(Button sender)
    {
        sender.interactable = false;
        _view.ShowPauseScreen(false);
        GameManager.Instance.Pause(false);
        GameManager.Instance.BackToMenu();
    }
}