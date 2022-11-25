using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Bottle[] _bottles;
    [SerializeField] private Color[] _colors;
    [SerializeField] private bool _fxEnable;
    [SerializeField] private ParticleSystem _stopFXL;
    [SerializeField] private ParticleSystem _stopFXR;
    [SerializeField] private float _minDistanceBetweenBottles = 0.5f;
    [SerializeField] private float _timeToStop = 2;

    public int BottlesAmount { get; private set; }

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

        if (_fxEnable && _waitTime >= _timeToStop)
        {
            GlobalEvents.SendOnCantSpawn();

            TryPlayStopFX(true);
        }
        else
        {
            TryPlayStopFX(false);
        }
    }

    public void Initialize(int bottlesAmount, float minDistance = 0.6f)
    {
        BottlesAmount = bottlesAmount;
        _minDistanceBetweenBottles = minDistance;
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

        BottlesAmount--;
    }

    private void TryPlayStopFX(bool play)
    {
        if (_stopFXL != null)
            _stopFXL.enableEmission = play;
        if (_stopFXR != null)
            _stopFXR.enableEmission = play;
    }
}