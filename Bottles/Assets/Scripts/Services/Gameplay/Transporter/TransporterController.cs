using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransporterController : Controller
{
    [SerializeField] private Transform _itemsContainer;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private GameObject _handlerPrefab;
    [SerializeField] private float _handlerOffsetY;
    [SerializeField] private float _distanceBetween;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _speed;
    [SerializeField] private int _capacity;
    [SerializeField] private bool _enabled;

    public event UnityAction<int> BottleSpawnEvent;

    private Spawner _spawner;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _spawner = GetComponentInChildren<Spawner>();
        if (_spawner != null)
        {
            Level level = ((GamePlayService)CurrentService).LevelCTRL.CurrentLevel;

            _spawner.Initialize(level.ItemsAmount, _capacity, _itemsContainer, level.ItemTypes, level.ColorPalettes);
            _spawner.ItemSpawnedEvent += OnBottleSpawn;
            _spawner.ActivationEvent += Active;
        }
    }

    private void OnDisable()
    {
        _spawner.ItemSpawnedEvent -= OnBottleSpawn;
        _spawner.ActivationEvent -= Active;
    }

    private void Update()
    {
        if (_enabled)
            TryToMove();
    }

    private void TryToMove()
    {
        if (_targetPoint != null)
        {
            for (int i = 0; i < _itemsContainer.childCount; i++)
            {
                var itemTransform =_itemsContainer.GetChild(i);
                if(i > 0)
                {
                    float newX = _targetPoint.position.x - (_distanceBetween * i);
                    var newPos = new Vector2(newX, _targetPoint.position.y);
                    float dist = newPos.x - itemTransform.position.x;

                    if (dist > 0.01f)
                        itemTransform.position = Vector2.Lerp(itemTransform.position, newPos, _speed * Time.deltaTime);
                }
                else
                {
                    float dist = _targetPoint.position.x - itemTransform.position.x;

                    if (dist > 0.01f)
                        itemTransform.position = Vector2.Lerp(itemTransform.position, _targetPoint.position, _speed * Time.deltaTime);
                }
            }
        }
        else Debug.LogWarning("Target point doesnt set!");
    }

    public void Active(bool active) => _enabled = active;

    private void OnBottleSpawn()
    {
        BottleSpawnEvent?.Invoke(0);
    }
}
