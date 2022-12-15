using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _minDistanceBetweenBottles = 0.9f;

    private Bottle[] _bottles;
    private ColorPalette[] _palettes;

    public int BottlesAmount { get; private set; }
    public event UnityAction BottlesSpawnEvent;

    private GameObject _container;

    private Transform _prevBottle;
    private bool _isStop;

    public void Initialize(int bottlesAmount, Bottle[] bottles, ColorPalette[] palettes)
    {
        BottlesAmount = bottlesAmount;
        _bottles = bottles;
        _palettes = palettes;


        _container = Instantiate(new GameObject());
        _container.name = "Bottles";

    }

    private void Update()
    {
        if(!_isStop)
        {
            if (BottlesAmount > 0 && (_prevBottle == null || Vector3.Magnitude(transform.position - _prevBottle.position) >= _minDistanceBetweenBottles))
                SpawnRandom(_container.transform);
        }
    }

    private void SpawnRandom(Transform parent)
    {
        int colorIndex = Random.Range(0, _palettes.Length);
        int bottlesIndex = Random.Range(0, _bottles.Length);
        Vector3 newPos = transform.position;
        GameObject newBottle = Instantiate(_bottles[bottlesIndex].gameObject, newPos, Quaternion.identity);
        newBottle.transform.parent = parent;

        _prevBottle = newBottle.transform;

        Bottle bottle = newBottle.GetComponent<Bottle>();
        bottle.SetColor(_palettes[colorIndex]);

        BottlesSpawnEvent?.Invoke();
    }

    private void Stop() => _isStop = true;
}