
public class ComboBag : Combo
{
    public override bool TryDoCombo(ItemController itemSender, ItemController itemTarget)
    {
        // Бутылка + бутылка того же цвета = горшок того же цвета
        if (itemTarget.Type != TypeNames.Bag &&
            itemSender.Type != TypeNames.Slime &&
            itemSender.Type != TypeNames.Bag &&
            itemSender.Type != itemTarget.ActiveView.Type &&
            itemSender.Color.Name != ColorNames.Multi &&
            itemSender.Color.Name != ColorNames.Empty &&
            itemSender.Color.Name == itemTarget.Color.Name)
        {
            itemTarget.SetType(TypeNames.Bag);
            itemTarget.PlayTypeChangeAnim();
            itemSender.DestroyItem(false);

            return true;
        }

        // Горшок + бутылка = бутылка с цветом горшка
        if (itemSender.Type != itemTarget.Type && itemSender.Type != TypeNames.Slime)
        {
            if (itemSender.Color.Name != itemTarget.Color.Name)
            {
                if (itemSender.Type == TypeNames.Bag)
                {
                    itemTarget.SetColor(itemSender.Color.Name);
                    itemTarget.PlayPaintFX(itemSender.Color.Color);

                    itemSender.DestroyItem(false);
                    itemTarget.PlayColorChangeAnim();

                    return true;
                }
                else if (itemTarget.Type == TypeNames.Bag)
                {
                    itemTarget.SetType(itemSender.Type);
                    itemTarget.SetColor(itemTarget.Color.Name);
                    itemTarget.PlayPaintFX(itemTarget.Color.Color);

                    itemSender.DestroyItem(false);
                    itemTarget.PlayColorChangeAnim();

                    return true;
                }
            }
        }

        // Горшок + бутылка с таким же цветом = бутылка с универсальным цветом
        if (itemSender.Type != itemTarget.Type && itemSender.Type != TypeNames.Slime)
        {
            if (itemSender.Color.Name == itemTarget.Color.Name)
            {
                if (itemSender.Type == TypeNames.Bag)
                {
                    itemTarget.SetColor(ColorNames.Multi);
                    itemTarget.PlayPaintFX(itemTarget.Color.Color);

                    itemSender.DestroyItem(false);
                    itemTarget.PlayColorChangeAnim();

                    return true;
                }
                else if (itemTarget.Type == TypeNames.Bag)
                {
                    itemTarget.SetType(itemSender.Type);
                    itemTarget.SetColor(ColorNames.Multi);
                    itemTarget.PlayPaintFX(itemTarget.Color.Color);

                    itemSender.DestroyItem(false);
                    itemTarget.PlayColorChangeAnim();

                    return true;
                }
            }
        }

        // Горшок + горшок с таким же цветом = горшок с универсальным цветом
        if (itemSender.Type == TypeNames.Bag &&
            itemTarget.Type == TypeNames.Bag &&
            itemSender.Color.Name == itemTarget.Color.Name)
        {
            itemTarget.SetColor(ColorNames.Multi);
            itemTarget.PlayTypeChangeAnim();
            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
