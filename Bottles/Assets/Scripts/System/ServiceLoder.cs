using UnityEngine;
using System.Collections.Generic;

public class ServiceLoder : MonoBehaviour
{
    [SerializeField] private List<Service> _services;
    
    private void Awake()
    {
        ServiceManager.AttachServices(_services);    
    }
}
