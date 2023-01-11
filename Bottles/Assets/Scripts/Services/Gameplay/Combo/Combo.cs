using UnityEngine;

public abstract class Combo: MonoBehaviour
{
    public abstract bool TryDoCombo(ItemController itemSender, ItemController itemTarget);
}
