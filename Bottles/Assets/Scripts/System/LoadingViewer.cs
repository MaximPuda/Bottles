using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LoadingViewer : MonoBehaviour
{
    public static LoadingViewer Instance; 

    private Animator _animator;

    public bool IsReady { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        _animator = GetComponent<Animator>();
    }

    public void In()
    {
        IsReady = false;
        _animator.SetBool("Show", true);
    }

    public void Out()
    {
        _animator.SetBool("Show", false);
    }

    private void OnOutEnd()
    {
        IsReady = false;
    }
}
