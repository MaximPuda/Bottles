using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Add new level", order = 1)]
public class Level : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] private int _bottlesAmount;

    [Header("Targets")]
    [SerializeField] private Target[] _targets = new Target[3];

    public int BottlesAmount => _bottlesAmount;
}
