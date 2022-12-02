using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Bottle[] _bottles;
    [SerializeField] private ColorPalette[] _palettes;
    [SerializeField] private float _minDistanceBetweenBottles = 0.5f;

    public int BottlesAmount { get; private set; }

    private Transform _prevBottle;
    private bool _isStop;

    private void OnEnable()
    {
        GlobalEvents.OnLevelCompleted += Stop;
    }

    private void OnDisable()
    {
        GlobalEvents.OnLevelCompleted -= Stop;
    }

    private void Update()
    {
        if(!_isStop)
        {
            if (BottlesAmount > 0 && (_prevBottle == null || Vector3.Magnitude(transform.position - _prevBottle.position) >= _minDistanceBetweenBottles))
                SpawnRandom();
        }
    }

    public void Initialize(int bottlesAmount, float minDistance = 0.9f)
    {
        BottlesAmount = bottlesAmount;
        _minDistanceBetweenBottles = minDistance;
    }

    private void SpawnRandom()
    {
        int colorIndex = Random.Range(0, _palettes.Length);
        int bottlesIndex = Random.Range(0, _bottles.Length);
        Vector3 newPos = transform.position;
        GameObject newBottle = Instantiate(_bottles[bottlesIndex].gameObject, newPos, Quaternion.identity);
        _prevBottle = newBottle.transform;

        Bottle bottle = newBottle.GetComponent<Bottle>();
        bottle.SetColor(_palettes[colorIndex]);

        BottlesAmount--;
        GlobalEvents.SendOnBottleSpawn(BottlesAmount);
    }

    private void Stop() => _isStop = true;
}