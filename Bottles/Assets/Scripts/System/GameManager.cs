using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Level _currentLevel;

    public static GameManager Instance { get; private set; }
    public Level CurrentLevel => _currentLevel;

    public event UnityAction MenuEnterEvent;
    public event UnityAction PlayEnterEvent;
    public event UnityAction PauseEnterEvent;
    public event UnityAction WinEnterEvent;
    public event UnityAction LoseEnterEvent;

    private bool _isReady;
    private GameStates _currrentState;
    private GameStates _nextState;

    public GameStates CurrentState
    {
        get { return _currrentState; }
        private set
        {
            _currrentState = value;
            SendState();
            Debug.Log(CurrentState);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initizilize();
        }
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    private void Initizilize()
    {
        _isReady = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (currentSceneIndex)
        {
            case 0:
                _nextState = GameStates.Menu;
                _isReady = true;
                break;

            case 1:
                _nextState = GameStates.Play;
                _isReady = true;
                break;

            default:
                throw new Exception("Wrong scene loaded!");
        }

        ServiceManager.InitAllServices();
    }

    public void StartGameManager()
    {
        if (_isReady)
            CurrentState = _nextState;
    }

    private void SendState()
    {
        switch (CurrentState)
        {
            case GameStates.Menu:
                MenuEnterEvent?.Invoke();
                break;
            case GameStates.Play:
                PlayEnterEvent?.Invoke();
                break;
            case GameStates.Pause:
                PauseEnterEvent?.Invoke();
                break;
            case GameStates.Win:
                WinEnterEvent?.Invoke();
                break;
            case GameStates.Lose:
                LoseEnterEvent?.Invoke();
                break;
            default:
                throw new ArgumentException("Wrong game state!");
        }
    }

    private IEnumerator LoadSceneAsync(int sceneIndex, GameStates state)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneIndex);
        
        while(!loading.isDone)
            yield return null;

        _nextState = state;
        ServiceManager.InitAllServices();
    }

    public void SetLevel(Level level)
    {
        if (level != null)
            _currentLevel = level;
    }

    public void Play()
    {
        StartCoroutine(LoadSceneAsync(1, GameStates.Play));
    }

    public void Restart()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadSceneAsync(sceneIndex, GameStates.Play));
    }

    public void BackToMenu()
    {
        StartCoroutine(LoadSceneAsync(0, GameStates.Menu));
    }

    public void Win()
    {
        CurrentState = GameStates.Win; 
    }

    public void Lose()
    {
        CurrentState = GameStates.Lose;
    }

    public void Continue()
    {
        CurrentState = GameStates.Play;
    }

    public void Pause()
    {
        if(CurrentState == GameStates.Pause)
        {
            Time.timeScale = 1;
            CurrentState = GameStates.Play;
        }
        else if(CurrentState == GameStates.Play)
        {
            Time.timeScale = 0;
            CurrentState = GameStates.Pause;
        }
    }
}
