using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TutorialList _tutorialName;
    [SerializeField] private GameObject _blackOut;

    public TutorialList TutorialName => _tutorialName;
    
    private Animator _animator;
    private bool _isRunning;

    public void Initialize()
    {
        _animator = GetComponent<Animator>();

        ServiceManager.TryGetService<GamePlayService>(out GamePlayService gamePlay);
        if (gamePlay != null)
        {
            gamePlay.WagonCTRL.BoxCloseEvent += OnSaccess;
        }
    }

    public void StartTutorial()
    {
        _animator.SetTrigger("Start");
        _blackOut.SetActive(true);
        _isRunning = true;
    }

    public void Next()
    {
        _animator.SetTrigger("Next");
    }

    public void Stop()
    {
        _blackOut.SetActive(false);
        _isRunning = false;
    }
    
    private void OnSaccess(int combo)
    {
        Next();
    }
}
