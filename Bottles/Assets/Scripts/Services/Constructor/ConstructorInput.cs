using UnityEngine;

public class ConstructorInput
{
    private const float DOUBLE_TAP_TIME = 0.2f;

    private Camera _cam;

    private ItemController _activeItem;
    private ItemController _lastItem; 

    public void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                Tap(touch);

            if (touch.phase == TouchPhase.Moved)
                Drag(touch);

            if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                Drop(touch);
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
        if (_activeItem != null && _activeItem == selected)
        {
            _activeItem.DestroyItem(true);
            _activeItem = null;
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
            _activeItem.DropPosition = _activeItem.Dragable.position;
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
                        _activeItem = null;
                    }
                }
            }
        }
    }
}
