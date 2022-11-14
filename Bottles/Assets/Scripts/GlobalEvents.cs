using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvents
{
    public static event UnityAction<int> OnBottleCombo;
    public static event UnityAction OnCantSpawn;

    public static void SendOnBottleCombo(int combo) => OnBottleCombo?.Invoke(combo);
    public static void SendOnCantSpawn() => OnCantSpawn?.Invoke();
}
