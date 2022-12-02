using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    [SerializeField] private Box[] _boxes;

    private int _closedCount;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();    
    }

    private void OnEnable()
    {
        foreach (var box in _boxes)
        {
            box.OnBoxClose += CloseBoxAdd;
        }
    }

    private void OnDesable()
    {
        foreach (var box in _boxes)
            box.OnBoxClose -= CloseBoxAdd;
    }

    private void CloseBoxAdd()
    {
        _closedCount++;


        if(_closedCount == _boxes.Length)
        {
            _animator.SetTrigger("Completed");
            GlobalEvents.SendOnLevelCompleted();
        }
    }

    public void WagonDone()
    {
        GlobalEvents.SendOnGameOver();
    }
}
