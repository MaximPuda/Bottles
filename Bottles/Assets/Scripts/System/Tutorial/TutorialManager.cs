using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject _blackOut;

    private Tutorial _currentTutorial;
    private Tutorial[] _tutorials;

    public void Initialize()
    {
        _tutorials = GetComponentsInChildren<Tutorial>();
    }

    public void SetTutorial(TutorialList tutorialName)
    {
        if (tutorialName == TutorialList.None)
            return;

        foreach (var tutor in _tutorials)
        {
            if(tutor.TutorialName == tutorialName)
            {
                _currentTutorial = tutor;
                _currentTutorial.Initialize();
            }
        }
    }

    public void StartTutorial()
    {
        if (_currentTutorial != null)
        {
            _currentTutorial.StartTutorial();
        }
    }
}
