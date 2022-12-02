using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvents
{
    public static event UnityAction<int> OnBottleSpawn;
    public static event UnityAction<int> OnBottleCombo;
    public static event UnityAction OnBottleCrash;
    public static event UnityAction OnWrongCombination;
    public static event UnityAction OnCantSpawn;
    public static event UnityAction OnPlayerDie;
    public static event UnityAction OnGameOver;
    public static event UnityAction OnPause;
    public static event UnityAction OnLevelCompleted;

    public static void SendOnBottleSpawn(int amount) => OnBottleSpawn?.Invoke(amount);
    public static void SendOnBottleCombo(int combo) => OnBottleCombo?.Invoke(combo);
    public static void SendOnBottleCrash() => OnBottleCrash?.Invoke();
    public static void SendOnWrongCombination() => OnWrongCombination?.Invoke(); 
    public static void SendOnCantSpawn() => OnCantSpawn?.Invoke();
    public static void SendOnPlayerDie() => OnPlayerDie?.Invoke();
    public static void SendOnGameOver() => OnGameOver?.Invoke();
    public static void SendOnPause() => OnPause?.Invoke();
    public static void SendOnLevelCompleted() => OnLevelCompleted?.Invoke();
}
