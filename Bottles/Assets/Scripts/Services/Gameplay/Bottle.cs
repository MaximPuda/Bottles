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
    [SerializeField] private SpriteRenderer _back;
    [SerializeField] private SpriteRenderer _outline;
    [SerializeField] private ParticleSystem _uniFx;
    [SerializeField] private ParticleSystem _crashFX;
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private Shapes _shape;
    [SerializeField] private ColorsName _color;
    [SerializeField] private Gradient _uniColor;

    public Transform Dragable => _dragable;
    public Shapes CurrentShape => _shape;
    public ColorsName CurrentColor => _color;
    public bool IsCollected { get; set; }

    private Rigidbody2D _rb;
    private Collider2D _collider;

    private Color _tempColor;
    private int _uniColorIndex;
    private float _uniColorIndexLerp = 0;
    private int _uniColorLerp = 4;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (CurrentColor == ColorsName.All)
        {
            _tempColor = Color.Lerp(_tempColor, _uniColor.colorKeys[_uniColorIndex].color, _uniColorLerp * Time.deltaTime);
            _uniColorIndexLerp = Mathf.Lerp(_uniColorIndexLerp, 1, _uniColorLerp * Time.deltaTime);
            _fill.color = _tempColor;

            if (_uniColorIndexLerp > 0.95)
            { 
                _uniColorIndex++;
                _uniColorIndexLerp = 0;
            }    

            if (_uniColorIndex > _uniColor.colorKeys.Length - 1)
                _uniColorIndex = 0;
        }    
    }

    public void SetColor(ColorPalette palette)
    {
        _color = palette.ColorName;

        if (CurrentColor == ColorsName.None)
        {
            _fill.enabled = true;
            _fill.enabled = false;
        }    
        else if (CurrentColor == ColorsName.All)
        {
            _uniFx.enableEmission = true;
        }
        else _fill.color = palette.Color;

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
        _back.enabled = false;
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
