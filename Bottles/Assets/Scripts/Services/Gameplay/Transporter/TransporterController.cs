using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransporterController : Controller
{
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
        if(_spawner != null)
        {
            Level level = ((GamePlayService)CurrentService).LevelCTRL.CurrentLevel;
            
            _spawner.Initialize(level.BottlesAmount, _capacity, level.BottleTypes, level.ColorPalettes);
            _spawner.ItemSpawnedEvent += OnBottleSpawn;
            _spawner.ActivationEvent += Active;
        }
    }

    private void OnDisable()
    {
        _spawner.ItemSpawnedEvent -= OnBottleSpawn;
        _spawner.ActivationEvent -= Active;
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.collider.TryGetComponent<Item>(out Item item))
    //    {
    //        if (_enabled && !item.IsFaced)
    //            collision.rigidbody.velocity = (_direction * _speed);
    //        else
    //            collision.rigidbody.velocity = Vector2.zero;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (_enabled)
    //        collision.rigidbody.velocity = Vector2.zero;
    //}

    public void Active(bool active) => _enabled = active;

    private void OnBottleSpawn()
    {
        BottleSpawnEvent?.Invoke(0);
    }
}
