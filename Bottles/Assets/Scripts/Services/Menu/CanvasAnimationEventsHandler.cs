using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimationEventsHandler : MonoBehaviour
{
    [SerializeField] private MenuUIController _menu;
    public void OnLevelsIn()
    {
        _menu.ScrollViewMove(1f);
    }   
    
    public void OnLevelsOut()
    {
        _menu.ScrollViewMove(0f);
    }
}
