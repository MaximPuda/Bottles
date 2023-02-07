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

        // ������ + ������ ��� �� ����� = ������������� ������
        if (itemSender.Type == itemTarget.Type &&
            itemSender.Color.Name == ColorNames.Empty &&
            itemTarget.Color.Name == ColorNames.Empty)
        {
            itemTarget.SetType(TypeNames.Multi);
            //_anim.Play("Item_TypeChange");
            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
