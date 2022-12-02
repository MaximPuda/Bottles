using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float DOUBLE_TAP_TIME = 0.2f;
    
    private Camera _cam;
    private Bottle _activeBottle;
    private Bottle _lastBottle;
    private PlayerData _data;

    public PlayerController Instance { get; private set; }

    private float _lastTapTime = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this.gameObject);

        InitializePlayer();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                float timeSinceLatTap = Time.time - _lastTapTime;

                if (timeSinceLatTap <= DOUBLE_TAP_TIME)
                    DoubleTap(touch);
                else Tap(touch);
                
                _lastTapTime = Time.time; 
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Drag(touch);
            }

            if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
            {
                Drop(touch);
            }
        }
    }

    private void InitializePlayer()
    {
        _cam = Camera.main;
        _data = new PlayerData(3);
    }

    private void Tap(Touch touch)
    {
        _lastBottle = _activeBottle;

        Bottle selected = SelectBootle(touch);
        if (selected != null)
        {
            _activeBottle = selected;
            _activeBottle.Active(true);
        }

        if (_lastBottle != null && _lastBottle != _activeBottle)
            _lastBottle.Active(false);
    }

    private void DoubleTap(Touch touch)
    {
        Bottle selected = SelectBootle(touch);
        if(_activeBottle = selected)
            _activeBottle.Crash();
    }

    private Bottle SelectBootle(Touch touch)
    {
        Vector2 touchWorldPos = _cam.ScreenToWorldPoint(touch.position);
        RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector3.forward);
        Bottle selectedBottle;
        if (hit.collider != null)
        {
            hit.collider.TryGetComponent<Bottle>(out selectedBottle);
            return selectedBottle;
        }
        else return null;
    }

    private void Drag(Touch touch)
    {
        if (_activeBottle != null)
        {
            if (!_activeBottle.IsCollected)
            {
                Vector2 touchWorldPos = _cam.ScreenToWorldPoint(touch.position);
                _activeBottle.Dragable.parent = null;
                _activeBottle.Dragable.position = touchWorldPos;
                _activeBottle.Dragable.rotation = Quaternion.identity;
            }
        }
    }

    private void Drop(Touch touch)
    {
        if (_activeBottle != null)
        {
            _activeBottle.Dragable.parent = _activeBottle.transform;
            _activeBottle.Dragable.localPosition = Vector3.zero;
            Vector2 touchWorldPos = _cam.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector3.forward);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<ICollectable>(out ICollectable Collectable))
                    Collectable.TryAddBottle(_activeBottle);
            }

            if (_activeBottle.IsCollected)
            {
                _activeBottle.Active(false);
                _activeBottle.ActivePhysic(false);
                _lastBottle = _activeBottle;
                _activeBottle = null;
            }    
        }
    }
}
