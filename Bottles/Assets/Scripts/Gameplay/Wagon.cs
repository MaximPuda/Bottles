using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    [SerializeField] private Box[] _boxes;

    private int _closedCount;

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
            Debug.Log("You win!");
        }
    }
}
