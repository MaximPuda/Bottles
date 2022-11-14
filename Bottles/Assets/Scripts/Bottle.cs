using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Bottle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bottle;
    [SerializeField] private SpriteRenderer _fill;
    [SerializeField] private ParticleSystem _crashFX;
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private Shape _shape;
    public Shape Shape => _shape;
    public Color Color { get; private set; }

    private float _speed = 2;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.magnitude > 3f)
            Crash();
    }

    public void SetColor(Color color)
    {
        Color = color;
        _fill.color = Color;

        var fxColor = Color;
        fxColor.a = 0.7f;
        _crashFX.startColor = fxColor;
    }

    public void SetSpeed(float speed) => _speed = speed;

    public void SetVelocity(Vector2 direction)
    {
        _rb.velocity = direction * _speed;
    }

    public void ActivePhysic(bool active)
    {
        _rb.simulated = active;
    }

    public void Crash()
    {
        _bottle.enabled = false;
        _fill.enabled = false;
        enabled = false;
        ActivePhysic(false);
        _crashFX.Play();
        StartCoroutine(DestroyWithDelay(_destroyDelay));
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
