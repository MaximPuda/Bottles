using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Add new level", order = 1)]
public class Level : ScriptableObject
{
    [SerializeField] private int _bottlesAmount;

    public int BottlesAmount => _bottlesAmount;
}
