using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    [SerializeField] private Transform _dragable;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _main;
    [SerializeField] private SpriteRenderer _fill;
    [SerializeField] private SpriteRenderer _back;
    [SerializeField] private SpriteRenderer _outline;

    [Header("Paricles")]
    [SerializeField] private ParticleSystem _uniFx;
    [SerializeField] private ParticleSystem _crashFX;

    [Header("Settings")]
    [SerializeField] private Transform _chacker;
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private ItemType _type;
    [SerializeField] private ColorPalette _color;
    [SerializeField] private ColorPalette[] _palettes;
    [SerializeField] private Gradient _uniColor;

    [Header("Movemenet")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _minDistanceBetween = 0.08f;

    public Transform Dragable => _dragable;
    public ItemType Type => _type;
    public ColorPalette Color => _color;
    public bool IsCollected { get; set; }
    public bool IsFaced { get; private set; }

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
        TryToMove();

        if (Color.ColorName == ColorsName.All)
        {
            _tempColor = UnityEngine.Color.Lerp(_tempColor, _uniColor.colorKeys[_uniColorIndex].color, _uniColorLerp * Time.deltaTime);
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

    public void SetColor(ColorsName colorName)
    {
        foreach (var palette in _palettes)
        {
            if (palette.ColorName == colorName)
                _color = palette;
        }

        if (Color.ColorName == ColorsName.Empty)
        {
            _fill.enabled = false;
        }
        else if (Color.ColorName == ColorsName.All)
        {
            _uniFx.enableEmission = true;
        }
        else
        {
            _fill.color = Color.Color;
            _uniFx.enableEmission = false;
        }

        var fxColor = Color.Color;
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

    public void TryToMove()
    {
        var hit = Physics2D.Raycast(_chacker.position, Vector2.right);
        if(hit == true) 
        {
            var minDistance = _rb.Distance(hit.collider);
            if (minDistance.distance >= _minDistanceBetween)
            {
                _rb.velocity = Vector2.right * _speed;
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }
        }
    }
   
    public void Crash()
    {
        transform.parent = null;
        _main.enabled = false;
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
