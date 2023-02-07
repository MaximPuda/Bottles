using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData: Controller
{
    [SerializeField] private string _playerName;
    [SerializeField] private int _maxLifes;
    [SerializeField] private int _lifes;
    [SerializeField] private int _coins;

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

        get { return _maxLifes; }
    }

    public event UnityAction DataChangedEvent;

    private DateTime _date;
    
    public override void Initialize(Service service)
    {
        base.Initialize(service);

        LoadData();
    }

    [ContextMenu("Save Data")]
    public void SaveData()
    {
        var save = new Save(_playerName, _lifes, _maxLifes, _coins);

        SaveSystem.Save(save);
    }

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        Save save = SaveSystem.Load();
        if (save == null)
        {
            Debug.LogWarning("Failed to load!");
            return;
        }

        _playerName = save.PlayerName;
        _date = DateTime.ParseExact(save.Date, "u", CultureInfo.InvariantCulture);
        _maxLifes = save.MaxLifes;
        _lifes = save.Lifes;
        _coins = save.Coins;

        DataChangedEvent?.Invoke();
    }

    [ContextMenu("Delete Data File")]
    public void DeleteDataFile()
    {
        SaveSystem.DeleteSave();
    }
}
