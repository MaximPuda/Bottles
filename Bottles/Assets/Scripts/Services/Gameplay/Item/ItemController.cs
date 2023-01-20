using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class ItemController : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemView[] _itemViews;

    [Header("Paricles")]
    [SerializeField] private GameObject _stand;
    [SerializeField] private ParticleSystem _crashFX;
    [SerializeField] private ParticleSystem _dustFX;
    [SerializeField] private ParticleSystem _paintFX;

    [Header("Settings")]
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private ItemColor[] _palettes;
    [SerializeField] private Gradient _uniColor;
    [SerializeField] private Animation _anim;

    [SerializeField] private Combo[] _combos;

    public ItemView ActiveView { get; private set; }
    public TypeNames Type => ActiveView.Type;
    public ItemColor Color { get; private set; }
    public Transform Dragable => ActiveView.Dragable;
    public bool IsCollected { get; private set; }

    private Collider2D _collider;
    private ItemPool _pool;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void Inititalize(ItemPool pool)
    {
        _pool = pool;
    }

    public void SetView(TypeNames itemType)
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
                    SetColor(Color.Name);
                
                return;
            }
        }
    }

    public void SetColor(ColorsName colorName)
    {
        if (Type == TypeNames.Bag && colorName == ColorsName.Empty)
            colorName++;

        foreach (var palette in _palettes)
        {
            if (palette.Name == colorName)
            {
                Color = palette;
                break;
            }
        }

        if (Color.Name == ColorsName.Empty)
        {
            ActiveView.EnableFill(false);
            ActiveView.EnableMulti(false);
        }
        else if (Color.Name == ColorsName.Multi)
        {
            ActiveView.EnableFill(false);
            ActiveView.EnableMulti(true);
        }
        else
        {
            ActiveView.EnableFill(true);
            ActiveView.EnableMulti(false);
            ActiveView.OnChangeColor(Color.Color);
        }
    }

    public void OnColllect()
    {
        IsCollected = true;
        _stand.SetActive(false);
        _collider.enabled = false;
        Active(false);
    }

    public void Active(bool active) => ActiveView.OnActive(active);

    public void Hide() => gameObject.SetActive(false);

    public void Reset()
    {
        enabled = true;

        if (ActiveView != null)
        {
            ActiveView.Reset();
            ActiveView.gameObject.SetActive(false);
            ActiveView = null;
        }

        Color = null;
        IsCollected = false;

        _collider.enabled = true;
    }

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
        else _pool.PutItem(this);
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _pool.PutItem(this);
    }

    public bool Interact(ItemController itemSender)
    {
        if (!IsCollected && itemSender != null && itemSender != this)
        {
            //if (_combos == null)
            //    return false;

            //bool isDone = false;
            //foreach (var combo in _combos)
            //{
            //    if (combo != null)
            //    {
            //        isDone = combo.TryDoCombo(itemSender, this);
            //        if (isDone)
            //            return true;
            //    }
            //}

            //return false;

            // Буталка + бутылка той же формы
            if (itemSender.Type != TypeNames.Bag &&
                itemSender.Type != TypeNames.Multi &&
                itemSender.Color.Name != ColorsName.Empty &&
                Color.Name != ColorsName.Empty &&
                itemSender.Type == Type)
            {
                // Если цвета одиннаковые
                if (itemSender.Color.Name != ColorsName.Multi &&
                    itemSender.Color.Name == Color.Name)
                {
                    // Универсальная с универсальным цветом
                    SetView(TypeNames.Multi);
                    SetColor(ColorsName.Multi);
                    _anim.Play("Item_TypeChange");
                }
                else // Если цвета разные
                {
                    // Пустая той же формы
                    SetColor(ColorsName.Empty);
                    _anim.Play("Item_TypeChange");
                }

                itemSender.DestroyItem(false);

                return true;
            }

            // Пустая + полная =  меняет форму полной бутылки наформу пустой
            if (itemSender.Type != Type)
            {
                if (itemSender.Color.Name == ColorsName.Empty)
                {
                    SetView(itemSender.Type);
                    _anim.Play("Item_TypeChange");
                    itemSender.DestroyItem(false);

                    return true;
                }
                else if (Color.Name == ColorsName.Empty)
                {
                    SetColor(itemSender.Color.Name);
                    _anim.Play("Item_TypeChange");
                    itemSender.DestroyItem(false);

                    return true;
                }
            }

            // Пустая + пустая той же формы = универсальная пустая
            if (itemSender.Type == Type &&
                itemSender.Color.Name == ColorsName.Empty &&
                Color.Name == ColorsName.Empty)
            {
                SetView(TypeNames.Multi);
                _anim.Play("Item_TypeChange");
                itemSender.DestroyItem(false);

                return true;
            }

            // Бутылка + бутылка того же цвета = горшок того же цвета
            if (itemSender.Type != TypeNames.Bag &&
                itemSender.Type != ActiveView.Type &&
                itemSender.Color.Name != ColorsName.Multi &&
                itemSender.Color.Name != ColorsName.Empty &&
                itemSender.Color.Name == Color.Name)
            {
                SetView(TypeNames.Bag);
                _anim.Play("Item_TypeChange");
                itemSender.DestroyItem(false);

                return true;
            }

            // Горшок + бутылка = бутылка с цветом горшка
            if (itemSender.Type != Type)
            {
                if (itemSender.Type == TypeNames.Bag)
                {
                    SetColor(itemSender.Color.Name);
                    PlayPaintFX(itemSender.Color.Color);

                    itemSender.DestroyItem(false);
                    _anim.Play("Item_ColorChange");

                    return true;
                }
                else if (Type == TypeNames.Bag)
                {
                    SetView(itemSender.Type);
                    SetColor(Color.Name);
                    PlayPaintFX(Color.Color);

                    itemSender.DestroyItem(false);
                    _anim.Play("Item_ColorChange");

                    return true;
                }
            }

            // Горшок + горшок с таким же цветом = горшок с универсальным цветом
            if (itemSender.Type == TypeNames.Bag &&
                Type == TypeNames.Bag &&
                itemSender.Color.Name == Color.Name)
            {
                SetColor(ColorsName.Multi);
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

    public void PlaySpawnAnim() =>_anim.Play("Item_Spawn");
}
