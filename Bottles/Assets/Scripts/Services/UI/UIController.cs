using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIView _view;
    [SerializeField] private ScoreSystem _scoreSystem;

    private void OnEnable()
    {
        _scoreSystem.OnPonintsChanged += UpdatePoints;

        EventBus.OnBottleSpawn += UpdateBottlesAmount;
        EventBus.OnGameOver += ShowGameOverScreen;
    }

    private void OnDisable()
    {
        _scoreSystem.OnPonintsChanged -= UpdatePoints;

        EventBus.OnBottleSpawn -= UpdateBottlesAmount;
        EventBus.OnGameOver -= ShowGameOverScreen;
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