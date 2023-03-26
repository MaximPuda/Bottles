public class ComboMulti : Combo
{
    public override bool TryDoCombo(ItemController itemSender, ItemController itemTarget)
    {
        // ”ниверсальна€ форма + универсальна€ форма того же цвета = униварсальна€ универсального цвета
        if (itemSender.Type == TypeNames.Multi &&
            itemTarget.Type == TypeNames.Multi &&
            itemSender.Color.Name != ColorNames.Multi &&
            itemTarget.Color.Name != ColorNames.Multi &&
            itemSender.Color.Name == itemTarget.Color.Name)
        {
            itemTarget.SetColor(ColorNames.Multi);
            itemTarget.PlayPaintFX(itemTarget.Color.Color);

            itemSender.DestroyItem(false);
            itemTarget.PlayColorChangeAnim();

            return true;
        }

        return false;
    }
}
