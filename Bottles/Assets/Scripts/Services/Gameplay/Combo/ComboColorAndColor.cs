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
            itemSender.Color.Name != ColorsName.Multi &&
            itemSender.Color.Name != ColorsName.Empty &&
            itemSender.Color.Name == itemTarget.Color.Name)
        {
            itemTarget.SetView(TypeNames.Bag);
            //_anim.Play("Item_TypeChange");
            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
