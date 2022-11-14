using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{
    [SerializeField] private float _scaleFactor = 0.6f;
    public bool IsEmpty { get; private set; } = true;
    public Bottle CurrentBottle { get; private set; }

    public event UnityAction OnBottleAdd;

    public void Addbottle(Bottle bottle)
    {
        CurrentBottle = bottle;
        CurrentBottle.ActivePhysic(false);
        CurrentBottle.transform.parent = transform;
        CurrentBottle.transform.localPosition = Vector3.zero;
        CurrentBottle.transform.localScale = Vector3.one * _scaleFactor;
        CurrentBottle.transform.localRotation = Quaternion.identity;
        
        IsEmpty = false;
        OnBottleAdd?.Invoke();
    }

    public void RemoveBottle()
    {
        CurrentBottle.Crash();
        CurrentBottle = null;
        IsEmpty = true;
    }
}
