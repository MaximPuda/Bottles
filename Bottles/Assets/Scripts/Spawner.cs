using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Bottle[] _bottles;
    [SerializeField] private Color[] _colors;
    [SerializeField] private float _minDistanceBetweenBottles = 0.5f;
    [SerializeField] private float _timeToStop = 2;

    private Transform _prevBottle;
    private float _waitTime = 0;

    private void Update()
    {
        if (_prevBottle == null || Vector3.Magnitude(transform.position - _prevBottle.position) >= _minDistanceBetweenBottles)
        {
            SpawnRandom();
            _waitTime = 0;
        }
        else _waitTime += Time.deltaTime;

        if (_waitTime >= _timeToStop)
            GlobalEvents.SendOnCantSpawn();

    }

    private void SpawnRandom()
    {
        int colorIndex = Random.Range(0, _colors.Length);
        int bottlesIndex = Random.Range(0, _bottles.Length);
        Vector3 newPos = transform.position;
        GameObject newBottle = Instantiate(_bottles[bottlesIndex].gameObject, newPos, Quaternion.identity);
        _prevBottle = newBottle.transform;

        Bottle bottle = newBottle.GetComponent<Bottle>();
        bottle.SetColor(_colors[colorIndex]);
    }
}
