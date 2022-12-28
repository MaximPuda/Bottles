using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class ItemController : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemView[] _itemViews;

    [Header("Paricles")]
    [SerializeField] private GameObject _stand;
    [SerializeField] private ParticleSystem _uniFX;
    [SerializeField] private ParticleSystem _crashFX;
    [SerializeField] private ParticleSystem _dustFX;
    [SerializeField] private ParticleSystem _paintFX;

    [Header("Settings")]
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private ColorPalette[] _palettes;
    [SerializeField] private Gradient _uniColor;
    [SerializeField] private Animation _anim;

    public ItemView ActiveView { get; private set; }
    public ItemType Type => ActiveView.Type;
    public ColorPalette Color { get; private set; }
    public Transform Dragable => ActiveView.Dragable;
    public bool IsCollected { get; private set; }

    private Rigidbody2D _rb;
    private Collider2D _collider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public void SetView(ItemType itemType)
    {
        foreach (var view in _itemViews)
        {
            if (view.Type == itemType)
            {
                if(ActiveView != null)
                    ActiveView.gameObject.SetActive(false);
                
                ActiveView = view;
                ActiveView.gameObject.SetActive(true);

                if (Color != null)
                    SetColor(Color.ColorName);
                
                return;
            }
        }
    }

    public void SetColor(ColorsName colorName)
    {
        if (colorName == null)
        {
            Debug.LogWarning("Color is null");
            return;
        }

        if (Type == ItemType.Bag && colorName == ColorsName.Empty ||
            Type == ItemType.Bag && colorName == ColorsName.All)
            colorName++;

        foreach (var palette in _palettes)
        {
            if (palette.ColorName == colorName)
            {
                Color = palette;
                break;
            }
        }

        if (Color.ColorName == ColorsName.Empty)
        {
            ActiveView.EnableFill(false);
            ActiveView.EnableMulti(false);
            _uniFX.enableEmission = false;
        }
        else if (Color.ColorName == ColorsName.All)
        {
            ActiveView.EnableFill(false);
            ActiveView.EnableMulti(true);
            _uniFX.enableEmission = true;
        }
        else
        {
            ActiveView.EnableFill(true);
            ActiveView.EnableMulti(false);
            ActiveView.OnChangeColor(Color.Color);
            _uniFX.enableEmission = false;
        }
    }

    public void OnColllect()
    {
        IsCollected = true;
        _stand.SetActive(false);
        _collider.enabled = false;
    }

    public void OnTempCollect()
    {
        _stand.SetActive(false);
    }    

    public void Active(bool active) => ActiveView.OnActive(active);

    public void Hide() => gameObject.SetActive(false);
   
    public void DestroyItem(bool withFX)
    {
        transform.parent = null;

        ActiveView.OnDestroyItem();

        _stand.SetActive(false);
        _collider.enabled = false;
        enabled = false;

        if (withFX)
        {
            PlayCrashFX(Color.Color);
            StartCoroutine(DestroyWithDelay(_destroyDelay));
        }
        else Destroy(this.gameObject);
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    public bool Interact(ItemController itemSender)
    {
        if (!IsCollected && itemSender != null && itemSender != this) 
        {
            if (itemSender.Type == ItemType.Bag && 
                ActiveView.Type != ItemType.Bag)
            {
                SetColor(itemSender.Color.ColorName);
                _anim.Play("Item_ColorChange");
                PlayPaintFX(itemSender.Color.Color);
                itemSender.DestroyItem(false);
                return true;
            }
            
            if (itemSender.Type != ItemType.Bag && 
                itemSender.Type != ItemType.All && 
                itemSender.Type == ActiveView.Type)
            {
                if (itemSender.Color.ColorName != ColorsName.Empty &&
                    itemSender.Color.ColorName != ColorsName.All &&
                    itemSender.Color.ColorName == Color.ColorName)
                {
                    SetView(ItemType.All);
                    SetColor(ColorsName.All);
                    _anim.Play("Item_TypeChange");
                }
                else
                {
                    SetColor(ColorsName.Empty);
                    _anim.Play("Item_TypeChange");
                }
                
                itemSender.DestroyItem(false);
                return true;
            }

            if (itemSender.Color.ColorName == ColorsName.Empty &&
                Color.ColorName != ColorsName.Empty)
            {
                SetView(itemSender.Type);
                _anim.Play("Item_TypeChange");
                itemSender.DestroyItem(false);
                return true;
            }

            if (itemSender.Type != ItemType.Bag &&
                itemSender.Type != ActiveView.Type &&
                itemSender.Color.ColorName != ColorsName.All &&
                itemSender.Color.ColorName != ColorsName.Empty && 
                itemSender.Color.ColorName == Color.ColorName)
            {
                SetView(ItemType.Bag);
                _anim.Play("Item_TypeChange");
                itemSender.DestroyItem(false);
                return true;
            }
        }

        return false;
    }

    public void PlayCrashFX(Color color)
    {
        var fxColor = color;
        fxColor.a = 0.7f;
        _crashFX.startColor = fxColor;

        _crashFX.Play();
    }

    public void PlayDustFX() => _dustFX.Play();

    public void PlayPaintFX(Color color)
    {
        _paintFX.startColor = color;
        _paintFX.Play();
    }
}
