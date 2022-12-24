using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _dragable;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _main;
    [SerializeField] private SpriteRenderer _fill;
    [SerializeField] private SpriteRenderer _back;
    [SerializeField] private SpriteRenderer _outline;
    [SerializeField] private GameObject _handler;

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

    public Transform Dragable => _dragable;
    public ItemType Type => _type;
    public ColorPalette Color => _color;
    public bool IsCollected { get; private set; }

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
        if (Color.ColorName == ColorsName.All)
        {
            _tempColor = UnityEngine.Color.Lerp(_tempColor, _uniColor.colorKeys[_uniColorIndex].color, _uniColorLerp * Time.deltaTime);
            _uniColorIndexLerp = Mathf.Lerp(_uniColorIndexLerp, 1, _uniColorLerp * Time.deltaTime);
            _fill.color = _tempColor;

            if (_uniColorIndexLerp > 0.99)
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
            _fill.enabled = true;
            _uniFx.enableEmission = true;
        }
        else
        {
            _fill.enabled = true;
            _fill.color = Color.Color;
            _uniFx.enableEmission = false;
        }

        var fxColor = Color.Color;
        fxColor.a = 0.7f;
        _crashFX.startColor = fxColor;
    }

    public void OnColllect()
    {
        IsCollected = true;
        _handler.SetActive(false);
        _collider.enabled = false;
    }

    public void Active(bool active)
    {
        _outline.enabled = active;
        if (active)
            ChangeSortingLayer("Drag");
        else
            ChangeSortingLayer("Default");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
   
    public void DestroyItem(bool withFX)
    {
        transform.parent = null;
        _main.enabled = false;
        _fill.enabled = false;
        _back.enabled = false;
        _outline.enabled = false;
        _handler.SetActive(false);
        _collider.enabled = false;
        enabled = false;

        if (withFX)
        {
            _crashFX.Play();
            StartCoroutine(DestroyWithDelay(_destroyDelay));
        }
        else Destroy(this.gameObject);
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    public bool Interact(Item itemSender)
    {
        if (!IsCollected && itemSender != null && itemSender != this) 
        {
            if (itemSender.Type == ItemType.Bag)
            {
                SetColor(itemSender.Color.ColorName);
                itemSender.DestroyItem(false);
                return true;
            }
            
            if (itemSender.Type == Type)
            {
                SetColor(ColorsName.Empty);
                itemSender.DestroyItem(false);
                return true;
            }

            if (itemSender.Color.ColorName == ColorsName.Empty)
            {
                itemSender.SetColor(Color.ColorName);
                itemSender.transform.parent = transform.parent;
                itemSender.transform.position = transform.position;
                DestroyItem(false);
            }
        }

        return false;
    }

    public void ChangeSortingLayer(string layerName)
    {
        _main.sortingLayerName = layerName;
        _fill.sortingLayerName = layerName;
        _back.sortingLayerName = layerName;
    }
}
