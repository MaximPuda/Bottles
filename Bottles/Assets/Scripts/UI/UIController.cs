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

        GlobalEvents.OnBottleSpawn += UpdateBottlesAmount;
        GlobalEvents.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        _scoreSystem.OnPonintsChanged -= UpdatePoints;

        GlobalEvents.OnBottleSpawn -= UpdateBottlesAmount;
        GlobalEvents.OnGameOver -= GameOver;
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

    private void GameOver()
    {
        _view.ShowGameOverScreen(true);
    }
}