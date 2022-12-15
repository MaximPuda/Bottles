using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : Controller
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _speed;
    [SerializeField] private bool _enabled;

    private Spawner _spawner;
    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _spawner = GetComponentInChildren<Spawner>();
        if(_spawner != null)
        {
            _spawner.BottlesSpawnEvent += OnBottleSpawn;
            if (CurrentService.TryGetController<LevelController>(out var levelController))
            {
                var level = levelController.CurrentLevel;
                _spawner.Initialize(level.BottlesAmount, level.BottleTypes, level.ColorPalettes);
            }
        }
    }

    private void OnDisable()
    {
        _spawner.BottlesSpawnEvent -= OnBottleSpawn;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_enabled)
            collision.rigidbody.velocity = _direction * _speed;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_enabled)
            collision.rigidbody.velocity = Vector2.zero;
    }

    private void OnBottleSpawn()
    {

    }
}
