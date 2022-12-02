using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Bottle : MonoBehaviour
{
    [SerializeField] private Transform _dragable;
    [SerializeField] private SpriteRenderer _bottle;
    [SerializeField] private SpriteRenderer _fill;
    [SerializeField] private SpriteRenderer _multicolor;
    [SerializeField] private SpriteRenderer _outline;
    [SerializeField] private ParticleSystem _crashFX;
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private Shapes _shape;
    [SerializeField] private Colors _color;

    public Transform Dragable => _dragable;
    public Shapes Shape => _shape;
    public Colors Color { get; private set; }
    public bool IsCollected { get; set; }

    private Rigidbody2D _rb;
    private Collider2D _collider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public void SetColor(ColorPalette palette)
    {
        Color = palette.ColorName;

        if (Color == Colors.None)
        {
            _multicolor.enabled = true;
            _fill.enabled = false;
        }    
        else
            _fill.color = palette.Color;

        var fxColor = palette.Color;
        fxColor.a = 0.7f;
        _crashFX.startColor = fxColor;
    }

    public void ActivePhysic(bool active)
    {
        _rb.simulated = active;
    }

    public void Active(bool active)
    {
        _outline.enabled = active;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
   
    public void Crash()
    {
        _bottle.enabled = false;
        _fill.enabled = false;
        _multicolor.enabled = false;
        _outline.enabled = false;
        _collider.enabled = false;
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
