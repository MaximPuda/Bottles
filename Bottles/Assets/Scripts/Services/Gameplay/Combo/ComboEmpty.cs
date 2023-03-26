public class ComboEmpty : Combo
{
    public override bool TryDoCombo(ItemController itemSender, ItemController itemTarget)
    {
        // ѕуста€ + полна€ =  мен€ет форму полной бутылки на форму пустой
        if (itemSender.Type != TypeNames.Slime &&
            itemSender.Type != itemTarget.Type &&
            itemSender.Color.Name != itemTarget.Color.Name)
        {
            if (itemSender.Color.Name == ColorNames.Empty)
            {
                itemTarget.SetType(itemSender.Type);
                itemTarget.PlayTypeChangeAnim();
                itemSender.DestroyItem(false);

                return true;
            }
            else if (itemTarget.Color.Name == ColorNames.Empty)
            {
                itemTarget.SetColor(itemSender.Color.Name);
                itemTarget.PlayTypeChangeAnim();
                itemSender.DestroyItem(false);

                return true;
            }
        }

        // ѕуста€ + полна€ той же формы = полна€ универсальной формы
        if (itemSender.Type == itemTarget.Type && itemSender.Color.Name != itemTarget.Color.Name)
        {
            if (itemSender.Color.Name == ColorNames.Empty)
            {
                itemTarget.SetType(TypeNames.Multi);
                itemTarget.PlayTypeChangeAnim();
                itemSender.DestroyItem(false);

                return true;
            }
            else if (itemTarget.Color.Name == ColorNames.Empty)
            {
                itemTarget.SetType(TypeNames.Multi);
                itemTarget.SetColor(itemSender.Color.Name);
                itemTarget.PlayTypeChangeAnim();
                itemSender.DestroyItem(false);

                return true;
            }
        }

        // ѕуста€ + пуста€ той же формы = универсальна€ пуста€
        if (itemSender.Type == itemTarget.Type &&
            itemSender.Color.Name == ColorNames.Empty &&
            itemTarget.Color.Name == ColorNames.Empty)
        {
            itemTarget.SetType(TypeNames.Multi);
            itemTarget.PlayTypeChangeAnim();
            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
