using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuUIController : Controller
{
    [SerializeField] private Animator _animator;
    [SerializeField] private MenuUIView _view;

    [SerializeField] private LevelButton _levelButton;
    [SerializeField] private Transform _levelsGrid;

    private int _levelsAmount;
    private PlayerData _playerData;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _levelsAmount = GameManager.Instance.LevelsAmount;
        CreateLevelButtons();

        if (ServiceManager.TryGetService<PlayerService>(out PlayerService player))
        {
            _playerData = player.PlayerDataCTRL;
            _playerData.DataChangedEvent += UpdateLifes;
            _playerData.DataChangedEvent += UpdateCoins;
        }
    }

    private void OnDisable()
    {
        _playerData.DataChangedEvent -= UpdateLifes;
        _playerData.DataChangedEvent -= UpdateCoins;
    }

    private void CreateLevelButtons()
    {
        if (_levelsAmount > 0)
        {
            for (int i = 0; i < _levelsAmount; i++)
            {
                LevelButton newButton = Instantiate(_levelButton, _levelsGrid);
                newButton.Initialize(i);
                newButton.BtnClickEvent += SetLevel;
            }
        }
    }

    public void OnPlayBtnClick()
    {
        GameManager.Instance.Play();
    }

    public void SetLevel(int levelIndex)
    {
        GameManager.Instance.SetLevel(levelIndex);
    }

    public void OnLevelsBtnClick()
    {
        _animator.SetBool("Levels", true);
    }

    public void OnLevelsBackBtnClick()
    {
        _animator.SetBool("Levels", false);
    }

    private void UpdateLifes()
    {
        _view.UpdateLifes(_playerData.Lifes, _playerData.MaxLifes, _playerData.SecondsLeft);
    }

    private void UpdateCoins()
    {
        _view.UpdateCoins(_playerData.Coins);
    }

    public void HideLoading()
    {
        LoadingViewer.Instance.Out();
    }
}
