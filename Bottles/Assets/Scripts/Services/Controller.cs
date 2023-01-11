using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    protected Service CurrentService;
    public virtual void Initialize(Service service)
    {
        CurrentService = service;
    }

    public virtual void OnStart() { }
}
