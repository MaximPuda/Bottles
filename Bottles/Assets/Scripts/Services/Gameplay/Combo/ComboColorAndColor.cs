using UnityEngine;

public class ComboColorAndColor : Combo
{
    public override bool TryDoCombo(ItemController itemSender, ItemController itemTarget)
    {
        if (itemSender == null || itemTarget == null)
        {
            Debug.LogWarning(this + " argument(s) is null!");
            return false;
        }

        // Бутылка + бутылка того же цвета = горшок того же цвета
        if (itemSender.Type != ItemType.Bag &&
            itemSender.Type != itemTarget.Type &&
            itemSender.Color.ColorName != ColorsName.Multi &&
            itemSender.Color.ColorName != ColorsName.Empty &&
            itemSender.Color.ColorName == itemTarget.Color.ColorName)
        {
            itemTarget.SetView(ItemType.Bag);
            //_anim.Play("Item_TypeChange");
            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
