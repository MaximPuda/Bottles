using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private Spawner _spawner;

    private void Awake()
    {
        _spawner.Initialize(_level.BottlesAmount);   
    }
}
