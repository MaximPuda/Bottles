using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _speed;
    [SerializeField] private bool _enabled;

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
}
