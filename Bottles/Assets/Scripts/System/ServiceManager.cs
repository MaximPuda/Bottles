using System.Collections.Generic;
using UnityEngine;

public static class ServiceManager
{
    private static List<Service> _services;

    public static void InitAllServices()
    {
        if (_services != null)
        {
            if (_services.Count > 0)
            {
                foreach (var service in _services)
                    service.Initialize();

            }
            else Debug.LogWarning("No attached services!");
        }
        else Debug.LogWarning("No attached services!");

        GameManager.Instance.StartGameManager();
    }

    public static bool IsAllServicesReady()
    {
        int count = 0;
        for (int i = 0; i < _services.Count; i++)
        {
            if (_services[i].isReady)
                count++;
        }

        return count == _services.Count;
    }

    public static void AttachServices(List<Service> services)
    {
        if (services != null)
        {
            _services = services;

            foreach (var service in _services)
                Debug.Log(service.ToString() + " is Attached!");
        }
    }

    public static void DetachService(Service service)
    {
        for (int i = 0; i < _services.Count; i++)
        {
            if (_services[i] == service)
            {
                _services.RemoveAt(i);
                Debug.Log(service.ToString() + " is detached!");
            }
        }
    }

    public static bool TryGetService<T>(out T service) where T : Service
    {
        foreach (var serv in _services)
        {
            if (serv.GetType() == typeof(T))
            {
                service = (T)serv;
                return true;
            }
        }

        service = null;
        return false;
    }
}
