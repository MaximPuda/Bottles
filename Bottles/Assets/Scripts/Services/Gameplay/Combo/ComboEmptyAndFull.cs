using UnityEngine;

public class ComboEmptyAndFull : Combo
{
    public override bool TryDoCombo(ItemController itemSender, ItemController itemTarget)
    {
        if (itemSender == null || itemTarget == null)
        {
            Debug.LogWarning(this + " argument(s) is null!");
            return false;
        }

        // Пустая + полная =  меняет форму полной бутылки наформу пустой
        if (itemSender.Type != itemTarget.Type)
        {
            if (itemSender.Color.ColorName == ColorsName.Empty)
            {
                itemTarget.SetView(itemSender.Type);
                //_anim.Play("Item_TypeChange");
                itemSender.DestroyItem(false);

                return true;
            }
            else if (itemTarget.Color.ColorName == ColorsName.Empty)
            {
                itemTarget. SetColor(itemSender.Color.ColorName);
                //_anim.Play("Item_TypeChange");
                itemSender.DestroyItem(false);

                return true;
            }
        }

        return false;
    }
}

