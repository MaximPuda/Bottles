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
        if (itemSender.Type != TypeNames.Bag &&
            itemSender.Type != itemTarget.Type &&
            itemSender.Color.Name != ColorNames.Multi &&
            itemSender.Color.Name != ColorNames.Empty &&
            itemSender.Color.Name == itemTarget.Color.Name)
        {
            itemTarget.SetType(TypeNames.Bag);
            //_anim.Play("Item_TypeChange");
            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
