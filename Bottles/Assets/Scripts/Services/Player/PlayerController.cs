using UnityEngine;
using UnityEngine.Events;

public class PlayerController : Controller
{
    public event UnityAction<int> MovesLeftEvent;
    
    private const float DOUBLE_TAP_TIME = 0.2f;
    
    private Camera _cam;
    private ItemController _activeItem;
    private ItemController _lastItem;

    private int _moves;

    public int Moves 
    {
        get => _moves;

        private set 
        {
            _moves = value;
            MovesLeftEvent?.Invoke(_moves);
        } 
    }

    private float _lastTapTime = 0;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        _cam = Camera.main;
        Moves = 35;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameStates.Play && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                Tap(touch);


            if (touch.phase == TouchPhase.Moved)
                Drag(touch);

            if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                Drop(touch); 

            if (Moves == 0)
            {
                enabled = false;
                GameManager.Instance.Lose();
            }
        }
    }

    private void Tap(Touch touch)
    {
        _lastItem = _activeItem;

        ItemController selected = SelectItem(touch);
        if (selected != null)
        {
            _activeItem = selected;
            _activeItem.Active(true);
        }

        if (_lastItem != null && _lastItem != _activeItem)
            _lastItem.Active(false);
    }

    private void DoubleTap(Touch touch)
    {
        ItemController selected = SelectItem(touch);
        if(_activeItem != null &&_activeItem == selected)
        {
            _activeItem.DestroyItem(true);
            _activeItem = null;
            Moves--;
        }
    }

    private ItemController SelectItem(Touch touch)
    {
        Vector2 touchWorldPos = _cam.ScreenToWorldPoint(touch.position);
        RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector3.forward);
        ItemController selectedItem;
        if (hit.collider != null)
        {
            hit.collider.TryGetComponent<ItemController>(out selectedItem);
            return selectedItem;
        }
        else return null;
    }

    private void Drag(Touch touch)
    {
        if (_activeItem != null)
        {
            if (!_activeItem.IsCollected)
            {
                Vector2 touchWorldPos = _cam.ScreenToWorldPoint(touch.position);
                _activeItem.Dragable.parent = null;
                _activeItem.Dragable.position = touchWorldPos;
                _activeItem.Dragable.rotation = Quaternion.identity;
            }
        }
    }

    private void Drop(Touch touch)
    {
        if (_activeItem != null)
        {
            _activeItem.Dragable.parent = _activeItem.ActiveView.transform;
            _activeItem.Dragable.localPosition = Vector3.zero;
            Vector2 touchWorldPos = _cam.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector3.forward);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactor))
                {
                    if (interactor.Interact(_activeItem))
                    {
                        Moves--;
                        _activeItem = null;
                    }
                }
            } 
        }
    }
}
