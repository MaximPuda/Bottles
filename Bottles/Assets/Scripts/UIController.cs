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
        GlobalEvents.OnCantSpawn += StopSpawnHandle;
    }

    private void OnDisable()
    {
        _scoreSystem.OnPonintsChanged -= UpdateHUD;
        GlobalEvents.OnCantSpawn -= StopSpawnHandle;
    }

    private void Start()
    {
        _view.IntitializeHUD();
    }

    private void UpdateHUD(int points)
    {
        _view.UpdatePoints(points);
    }

    private void StopSpawnHandle()
    {
        _view.Alarm(true);
    }
}