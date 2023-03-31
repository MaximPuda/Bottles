using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelPrefs _currentLevel;
    [SerializeField] private LevelPrefs[] _levels;

    public static GameManager Instance { get; private set; }
    public int LevelsAmount => _levels.Length;
    public LevelPrefs CurrentLevel { get { return _currentLevel; } set { _currentLevel = value; } }

    public event UnityAction MenuEnterEvent;
    public event UnityAction PlayEnterEvent;
    public event UnityAction PauseEnterEvent;
    public event UnityAction WinEnterEvent;
    public event UnityAction LoseEnterEvent;

    private bool _isReady;
    private int _currentLevelIndex;
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
                Debug.LogError("Wrong scene loaded!");
                break;
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
        LoadingViewer.Instance.In();
        yield return new WaitForSeconds(1f);

        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneIndex);
        
        while(!loading.isDone)
            yield return new WaitForSeconds(1f);

       _nextState = state;
        ServiceManager.InitAllServices();
    }

    public void SetLevel(int levelIndex)
    {
        if (_levels == null)
            return;
        
        _currentLevelIndex = levelIndex;
        _currentLevel = _levels[levelIndex];
        Play();

    }

    public void PlayNextLevel()
    {
        if (_levels == null)
            return;

        if (_currentLevelIndex < _levels.Length - 1)
        {
            _currentLevelIndex++;
            _currentLevel = _levels[_currentLevelIndex];
        }

        Play();
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

    public void Pause(bool active)
    {
        if(!active)
        {
            Time.timeScale = 1;
            CurrentState = GameStates.Play;
        }
        else 
        {
            Time.timeScale = 0;
            CurrentState = GameStates.Pause;
        }
    }
}
