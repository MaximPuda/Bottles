using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboEmptyAndEmpty : Combo
{
    public override bool TryDoCombo(ItemController itemSender, ItemController itemTarget)
    {
        if (itemSender == null || itemTarget == null)
        {
            Debug.LogWarning(this + " argument(s) is null!");
            return false;
        }

        // ѕуста€ + пуста€ той же формы = универсальна€ пуста€
        if (itemSender.Type == itemTarget.Type &&
            itemSender.Color.ColorName == ColorsName.Empty &&
            itemTarget.Color.ColorName == ColorsName.Empty)
        {
            itemTarget.SetView(ItemType.Multi);
            //_anim.Play("Item_TypeChange");
            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
