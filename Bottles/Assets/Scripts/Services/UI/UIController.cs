using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Controller
{
    [SerializeField] private UIView _view;

    private ScoreController _score;
    private TransporterController _transporter;
    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _view.IntitializeHUD();

        if (ServiceManager.TryGetService<GamePlayService>(out GamePlayService gamePlay))
        {
            _score = gamePlay.ScoreCTRL;
            _score.PonintsChangedEvent += UpdatePoints;

            _transporter = gamePlay.TransporterCTRL;
            _transporter.ItemsLeftEvent += UpdateBottlesAmount;
        }    
    }

    private void OnDisable()
    {
        _score.PonintsChangedEvent += UpdatePoints;
        _transporter.ItemsLeftEvent -= UpdateBottlesAmount;
    }

    private void UpdatePoints(int points)
    {
        _view.UpdatePoints(points);
    }

    private void UpdateBottlesAmount(int amount)
    {
        _view.UpdateBottlesAmount(amount);
    }

    public void ShowGameOverScreen()
    {
        _view.ShowGameOverScreen(true);
    }

    public void OnRestartBtnClick() => GameManager.Instance.Restart();

    public void OnBackBtnClick() => GameManager.Instance.BackToMenu();

    public void OnPauseBtnClick() => GameManager.Instance.Pause();
}