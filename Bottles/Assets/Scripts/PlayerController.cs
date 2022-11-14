using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera _cam;
    private Bottle _target;

    private void Awake()
    {
        _cam = Camera.main;
    }
    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                Vector2 touchWorldPos = _cam.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector3.forward);
                if (hit.collider != null)
                    if (hit.collider.TryGetComponent<Bottle>(out _target))
                        _target.ActivePhysic(false);
            }

            if(touch.phase == TouchPhase.Moved)
            {
                if(_target != null)
                {
                    if (_target.transform.parent == null)
                    {
                        _target.SetVelocity(Vector3.zero);
                        Vector2 touchWorldPos = _cam.ScreenToWorldPoint(touch.position);
                        _target.transform.position = touchWorldPos;
                        _target.transform.rotation = Quaternion.identity;
                    }
                    else _target = null;
                }
            }

            if(touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
            {
                if (_target != null)
                {
                    _target.ActivePhysic(true);
                }

                _target = null;
            }
        }
    }
}
