using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData: Controller
{
    [SerializeField] private string _playerName;
    [SerializeField] private string _lastPlayed;
    [SerializeField] private DateTime _date;
    [SerializeField] private int _maxLifes;
    [SerializeField] private int _lifes;
    [SerializeField] private int _coins;
    [SerializeField] private int _secondsToAddLife = 900;

    public string PlayerName
    {
        set { _playerName = value; }
        get { return _playerName; }
    }

    public int MaxLifes
    {
        set
        {
            _maxLifes = value;
            DataChangedEvent?.Invoke();
        }

        get { return _maxLifes; }
    }

    public int Lifes
    {
        set 
        {
            if (value < _maxLifes)
                _lifes = value;
            else _lifes = _maxLifes;

            if (value < 0)
                _lifes = 0;

            if(_lifes < _maxLifes)
            {
                if(SecondsLeft <= 0)
                    SecondsLeft = _secondsToAddLife;
                
                StartCoroutine(WaitToAddLife());
            }

            DataChangedEvent?.Invoke(); 
        }

        get { return _lifes; }
    }

    public int Coins
    {
        set
        {
            if (value > 0)
                _coins = value;
            else _coins = 0;

            DataChangedEvent?.Invoke();
        }

        get { return _coins; }
    }
    
    public int SecondsLeft { get; private set; }

    public event UnityAction DataChangedEvent;
    
    public override void Initialize(Service service)
    {
        base.Initialize(service);

        LoadData();
    }

    private void OnDisable()
    {
        SaveData();
    }

    [ContextMenu("Save Data")]
    public void SaveData()
    {
        Save save = new Save(PlayerName, Lifes, MaxLifes, SecondsLeft, Coins);

        SaveSystem.Save(save);
    }

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        Save save = SaveSystem.Load();
        if (save == null)
        {
            Debug.LogWarning("Failed to load!");
            ResetData();
            return;
        }

        PlayerName = save.PlayerName;
        _date = DateTime.ParseExact(save.Date, "u", CultureInfo.InvariantCulture);
        _lastPlayed = _date.ToString();
        MaxLifes = save.MaxLifes;
        Lifes = save.Lifes + GetEarnedLifes();
        SecondsLeft = GetTimerLeft(save.SecondsLeft);
        Coins = save.Coins;

        DataChangedEvent?.Invoke();
    }

    public void ResetData()
    {
        PlayerName = "User";
        _date = DateTime.Now;
        MaxLifes = 5;
        Lifes = 3;
        SecondsLeft = 900;
        Coins = 0;

        SaveData();
    }

    [ContextMenu("Delete Data File")]
    public void DeleteDataFile()
    {
        SaveSystem.DeleteSave();
    }

    private IEnumerator WaitToAddLife()
    {
        while(SecondsLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            SecondsLeft--;
            DataChangedEvent?.Invoke();
        }
        Lifes++;
    }    

    private int GetSpandedSec()
    { 
        TimeSpan spent = DateTime.Now.Subtract(_date);
        return (int)spent.TotalSeconds;
    }

    private int GetEarnedLifes()
    {
        int spentSec = GetSpandedSec();
        int earnedLifes = 0;

        if (SecondsLeft > 0)
            earnedLifes = (spentSec + _secondsToAddLife - SecondsLeft) / _secondsToAddLife;
        else
            earnedLifes = spentSec / _secondsToAddLife;

        return earnedLifes;
    }

    private int GetTimerLeft(int savedTimer)
    {
        int spentSec = GetSpandedSec();
        int result = savedTimer - spentSec;

        if (result > 0)
            return result;
        return 0;
    }
}
