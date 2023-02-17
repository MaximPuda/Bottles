using System;
using System.Globalization;

[System.Serializable]
public class Save
{
    private string _playerName;
    private string _date;
    private int _lifes;
    private int _maxLifes;
    private int _secondsLeft;
    private int _coins;

    public string PlayerName => _playerName;
    public string Date => _date;
    public int Lifes => _lifes;
    public int MaxLifes => _maxLifes;
    public int SecondsLeft => _secondsLeft;
    public int Coins => _coins;

    public Save(string name, int lifes, int maxLifes, int secondsLeft, int coins)
    {
        _playerName = name;
        _date = DateTime.Now.ToString("u", CultureInfo.InvariantCulture); 
        _lifes = lifes;
        _maxLifes = maxLifes;
        _secondsLeft = secondsLeft;
        _coins = coins;
    }
}
