using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIView _view;
    [SerializeField] private ScoreSystem _scoreSystem;

    private void OnEnable()
    {
        _scoreSystem.OnPonintsChanged += UpdateHUD;
        GlobalEvents.OnWrongCombination += WrongCombination;
        GlobalEvents.OnCantSpawn += StopSpawnHandle;
        GlobalEvents.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        _scoreSystem.OnPonintsChanged -= UpdateHUD;
        GlobalEvents.OnWrongCombination -= WrongCombination;
        GlobalEvents.OnCantSpawn -= StopSpawnHandle;
        GlobalEvents.OnGameOver -= GameOver;
    }

    private void Start()
    {
        _view.IntitializeHUD();
    }

    private void UpdateHUD(int points)
    {
        _view.UpdatePoints(points);
    }

    private void WrongCombination()
    {
        _view.DesableOneHP();
    }

    private void StopSpawnHandle()
    {
        _view.Alarm(true);
    }

    private void GameOver()
    {
        _view.ShowGameOverScreen(true);
    }
}