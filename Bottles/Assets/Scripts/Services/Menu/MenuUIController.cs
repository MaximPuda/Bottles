using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuUIController : Controller
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Animator _animator;
    [SerializeField] private LevelButton _levelButton;
    [SerializeField] private Transform _levelsGrid;

    private int _levelsAmount;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _levelsAmount = GameManager.Instance.LevelsAmount;
        CreateLevelButtons();
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

    public void ScrollViewMove(float ScrollPosition)
    {
        StartCoroutine(ScrollAnimation(ScrollPosition));
    }

    private IEnumerator ScrollAnimation(float endPosition)
    {
        while (Mathf.Abs(endPosition - _scrollRect.horizontalNormalizedPosition) >= 0.01f)
        {
            _scrollRect.horizontalNormalizedPosition = Mathf.Lerp(_scrollRect.horizontalNormalizedPosition, endPosition, 10 * Time.deltaTime);
            yield return null;
        }
    }
}
