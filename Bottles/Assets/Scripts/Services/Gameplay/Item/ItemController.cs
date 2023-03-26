using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))][SelectionBase]
public class ItemController : MonoBehaviour, IInteractable
{
    [Header("MAIN")]
    [ContextMenuItem("Apply", "ApplyStartSetting")]
    [SerializeField] private TypeNames _startType;
    [ContextMenuItem("Apply", "ApplyStartSetting")]
    [SerializeField] private ColorNames _startColor;
    [Space(5)]

    [Header("SETTINGS")]
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private ItemView[] _itemViews;
    [SerializeField] private ItemColor[] _palettes;
    [SerializeField] private Animation _anim;
    [Space(5)]

    [Header("PARTICLES")]
    [SerializeField] private GameObject _stand;
    [SerializeField] private ParticleSystem _crashFX;
    [SerializeField] private ParticleSystem _dustFX;
    [SerializeField] private ParticleSystem _paintFX;
    [Space(5)]

    [SerializeField] private Combo[] _combos;

    public ItemView ActiveView { get; private set; }
    public TypeNames Type => ActiveView.Type;
    public ItemColor Color { get; private set; }
    public Transform Dragable => ActiveView.Dragable;
    public Vector3 DropPosition { get; set; }
    public bool IsCollected { get; private set; }

    private Collider2D _collider;
    private ItemPool _pool;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();

        if (_startType != TypeNames.None && _startColor != ColorNames.None)
        {
            SetType(_startType);
            SetColor(_startColor);
        }

        if (ActiveView == null)
        {
            foreach (var view in _itemViews)
            {
                if (view.gameObject.activeSelf == true)
                {
                    ActiveView = view;
                    Color = view.Color;
                }
            }
        }
    }

    public void ApplyStartSetting()
    {
        if (_startType != TypeNames.None && _startColor != ColorNames.None)
        {
            SetType(_startType);
            SetColor(_startColor);
        }
    }

    public void Inititalize(ItemPool pool)
    {
        _pool = pool;
    }
   
    public bool Interact(ItemController itemSender)
    {
        if (!IsCollected && itemSender != null && itemSender != this)
        {
            if (_combos == null)
                return false;

            bool isDone = false;
            foreach (var combo in _combos)
            {
                if (combo != null)
                {
                    isDone = combo.TryDoCombo(itemSender, this);
                    if (isDone)
                        return true;
                }
            }

            return false;

            //// Слайм не взаимодействует ни с чем
            //if (Type == TypeNames.Slime)
            //    return false;

            //// Буталка + бутылка той же формы
            //if (itemSender.Type != TypeNames.Bag &&
            //    itemSender.Type != TypeNames.Multi &&
            //    itemSender.Color.Name != ColorNames.Empty &&
            //    Color.Name != ColorNames.Empty &&
            //    itemSender.Type == Type)
            //{
            //    // Если цвета одиннаковые
            //    if (itemSender.Color.Name != ColorNames.Multi &&
            //        itemSender.Color.Name == Color.Name)
            //    {
            //        // Универсальная с универсальным цветом
            //        SetType(TypeNames.Multi);
            //        SetColor(ColorNames.Multi);
            //        PlayTypeChangeAnim();
            //    }
            //    else // Если цвета разные
            //    {
            //        // Пустая той же формы
            //        SetColor(ColorNames.Empty);
            //        PlayTypeChangeAnim();
            //    }

            //    itemSender.DestroyItem(false);

            //    return true;
            //}

            //// Универсальная форма + универсальная форма того же цвета = униварсальная универсального цвета
            //if (itemSender.Type == TypeNames.Multi &&
            //    Type == TypeNames.Multi &&
            //    itemSender.Color.Name != ColorNames.Multi &&
            //    Color.Name != ColorNames.Multi &&
            //    itemSender.Color.Name == Color.Name)
            //{
            //    SetColor(ColorNames.Multi);
            //    PlayPaintFX(Color.Color);

            //    itemSender.DestroyItem(false);
            //    PlayColorChangeAnim();

            //    return true;
            //}

            //// Пустая + полная =  меняет форму полной бутылки на форму пустой
            //if (itemSender.Type != TypeNames.Slime && 
            //    itemSender.Type != Type && 
            //    itemSender.Color.Name != Color.Name)
            //{
            //    if (itemSender.Color.Name == ColorNames.Empty)
            //    {
            //        SetType(itemSender.Type);
            //        PlayTypeChangeAnim();
            //        itemSender.DestroyItem(false);

            //        return true;
            //    }
            //    else if (Color.Name == ColorNames.Empty)
            //    {
            //        SetColor(itemSender.Color.Name);
            //        PlayTypeChangeAnim();
            //        itemSender.DestroyItem(false);

            //        return true;
            //    }
            //}

            //// Пустая + полная той же формы = полная универсальной формы
            //if (itemSender.Type == Type && itemSender.Color.Name != Color.Name)
            //{
            //    if (itemSender.Color.Name == ColorNames.Empty)
            //    {
            //        SetType(TypeNames.Multi);
            //        PlayTypeChangeAnim();
            //        itemSender.DestroyItem(false);

            //        return true;
            //    }
            //    else if (Color.Name == ColorNames.Empty)
            //    {
            //        SetType(TypeNames.Multi);
            //        SetColor(itemSender.Color.Name);
            //        PlayTypeChangeAnim();
            //        itemSender.DestroyItem(false);

            //        return true;
            //    }
            //}

            //// Пустая + пустая той же формы = универсальная пустая
            //if (itemSender.Type == Type &&
            //    itemSender.Color.Name == ColorNames.Empty &&
            //    Color.Name == ColorNames.Empty)
            //{
            //    SetType(TypeNames.Multi);
            //    PlayTypeChangeAnim();
            //    itemSender.DestroyItem(false);

            //    return true;
            //}

            //// Бутылка + бутылка того же цвета = горшок того же цвета
            //if (Type != TypeNames.Bag &&
            //    itemSender.Type != TypeNames.Slime &&
            //    itemSender.Type != TypeNames.Bag &&
            //    itemSender.Type != ActiveView.Type &&
            //    itemSender.Color.Name != ColorNames.Multi &&
            //    itemSender.Color.Name != ColorNames.Empty &&
            //    itemSender.Color.Name == Color.Name)
            //{
            //    SetType(TypeNames.Bag);
            //    PlayTypeChangeAnim();
            //    itemSender.DestroyItem(false);

            //    return true;
            //}

            //// Горшок + бутылка = бутылка с цветом горшка
            //if (itemSender.Type != Type && itemSender.Type != TypeNames.Slime)
            //{
            //    if( itemSender.Color.Name != Color.Name)
            //    {
            //        if (itemSender.Type == TypeNames.Bag)
            //        {
            //            SetColor(itemSender.Color.Name);
            //            PlayPaintFX(itemSender.Color.Color);

            //            itemSender.DestroyItem(false);
            //            PlayColorChangeAnim();

            //            return true;
            //        }
            //        else if (Type == TypeNames.Bag)
            //        {
            //            SetType(itemSender.Type);
            //            SetColor(Color.Name);
            //            PlayPaintFX(Color.Color);

            //            itemSender.DestroyItem(false);
            //            PlayColorChangeAnim();

            //            return true;
            //        }
            //    }
            //}

            //// Горшок + бутылка с таким же цветом = бутылка с универсальным цветом
            //if (itemSender.Type != Type && itemSender.Type != TypeNames.Slime)
            //{
            //    if (itemSender.Color.Name == Color.Name)
            //    {
            //        if (itemSender.Type == TypeNames.Bag)
            //        {
            //            SetColor(ColorNames.Multi);
            //            PlayPaintFX(Color.Color);

            //            itemSender.DestroyItem(false);
            //            PlayColorChangeAnim();

            //            return true;
            //        }
            //        else if (Type == TypeNames.Bag)
            //        {
            //            SetType(itemSender.Type);
            //            SetColor(ColorNames.Multi);
            //            PlayPaintFX(Color.Color);

            //            itemSender.DestroyItem(false);
            //            PlayColorChangeAnim();

            //            return true;
            //        }
            //    }
            //}

            //// Горшок + горшок с таким же цветом = горшок с универсальным цветом
            //if (itemSender.Type == TypeNames.Bag &&
            //    Type == TypeNames.Bag &&
            //    itemSender.Color.Name == Color.Name)
            //{
            //    SetColor(ColorNames.Multi);
            //    PlayTypeChangeAnim();
            //    itemSender.DestroyItem(false);

            //    return true;
            //}

            //// Слайм + пустая бутылка = слайм уничтожается, а бутылка окрашивается
            //if (itemSender.Type == TypeNames.Slime && Color.Name == ColorNames.Empty)
            //{
            //    SetColor(itemSender.Color.Name);
            //    itemSender.DestroyItem(false);
            //    PlayPaintFX(itemSender.Color.Color);
            //    PlayColorChangeAnim();

            //    return true;
            //}
        }

        return false;
    }

    public void SetType(TypeNames itemType)
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

    public void SetColor(ColorNames colorName)
    {
        if (Type == TypeNames.Bag && colorName == ColorNames.Empty)
            colorName++;

        foreach (var palette in _palettes)
        {
            if (palette.Name == colorName)
            {
                Color = palette;
                break;
            }
        }

        if (Color.Name == ColorNames.Empty)
        {
            ActiveView.EnableFill(false);
            ActiveView.EnableMulti(false);
        }
        else if (Color.Name == ColorNames.Multi)
        {
            ActiveView.EnableFill(false);
            ActiveView.EnableMulti(true);
        }
        else
        {
            ActiveView.EnableFill(true);
            ActiveView.EnableMulti(false);
            ActiveView.OnChangeColor(Color);
        }
    }

    public void OnColllect()
    {
        IsCollected = true;
        _stand.SetActive(false);
        _collider.enabled = false;
        PlayDustFX();
        Active(false);
    }

    public void Active(bool active) => ActiveView.OnActive(active);

    public void Hide(bool hide)
    { 
        if (hide)
            ActiveView.gameObject.SetActive(false);
        else
        {
            ActiveView.gameObject.SetActive(true);
            PlaySpawnAnim();
        }    
    }

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
        else
        {
            if (_pool != null)
                _pool.PutItem(this);
            else Destroy(this.gameObject);
        }
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_pool != null)
            _pool.PutItem(this);
        else Destroy(this.gameObject);
    }

    public void Lock(bool locked)
    {
        _collider.enabled = !locked;
        ActiveView.OnLock(locked);
    }

    public void PlayCrashFX(Color color)
    {
        var fxColor = color;
        fxColor.a = 0.7f;
        _crashFX.startColor = fxColor;

        _crashFX.Play();
    }
    
    public void PlayPaintFX(Color color)
    {
        _paintFX.startColor = color;
        _paintFX.Play();
    }

    public void PlayDustFX() => _dustFX.Play();
    public void PlaySpawnAnim() =>_anim.Play("Item_Spawn");
    public void PlayTypeChangeAnim() => _anim.Play("Item_TypeChange");
    public void PlayColorChangeAnim() => _anim.Play("Item_ColorChange");
}
