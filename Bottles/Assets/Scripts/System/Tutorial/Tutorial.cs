using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Canvas), typeof(Animator))]
public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialog;
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private GameObject _blackOut;
    [SerializeField] private TutorialSlide[] _slides;
    
    private Animator _animator;
    private Canvas _canvas;
    private bool _isRunning;
    private int _count = 0;

    public void Initialize(Camera uiCamera)
    {
        _animator = GetComponent<Animator>();
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = uiCamera;

        ServiceManager.TryGetService<PlayerService>(out PlayerService player);
        if (player != null)
        {
            player.PlayerCTRL.InteractEvent += OnSuccess;
        }
    }

    public void StartTutorial()
    {
        _animator.SetTrigger("Start");
        _isRunning = true;
    }

    public void SetSlide()
    {
        if (_slides.Length > 0 && _count < _slides.Length)
        {
            _dialog.text = _slides[_count].Dialog;
            _message.text = _slides[_count].Message;
            _slides[_count].Mask.SetActive(true);

            _blackOut.SetActive(true);

            _count++;
        }
        else Stop();
    }

    public void Next()
    {
        if (_count >= _slides.Length)
            Stop();

        _blackOut.SetActive(false);

        if (_count > 0)
            _slides[_count - 1].Mask.SetActive(false);

        _animator.SetTrigger("Next");
    }

    public void Stop()
    {
        _blackOut.SetActive(false);
        _isRunning = false;
    }
    
    private void OnSuccess()
    {
        Next();
    }
}
