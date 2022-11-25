using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isPaused;

    private void OnEnable()
    {
        GlobalEvents.OnPlayerDie += GameOver;
    }

    private void OnDisable()
    {
        GlobalEvents.OnPlayerDie -= GameOver;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        GlobalEvents.SendOnGameOver();
    }

    public void Pause()
    {
        if(_isPaused)
        {
            Time.timeScale = 1;
            _isPaused = false;
        }
        else
        {
            Time.timeScale = 0;
            _isPaused = true;
        }
    }
}
