public class ComboSlime : Combo
{
    public override bool TryDoCombo(ItemController itemSender, ItemController itemTarget)
    {
        // Слайм не взаимодействует ни с чем
        if (itemTarget.Type == TypeNames.Slime)
            return false;

        // Слайм + пустая бутылка = слайм уничтожается, а бутылка окрашивается
        if (itemSender.Type == TypeNames.Slime && itemTarget.Color.Name == ColorNames.Empty)
        {
            itemTarget.SetColor(itemSender.Color.Name);
            itemSender.DestroyItem(false);
            itemTarget.PlayPaintFX(itemSender.Color.Color);
            itemTarget.PlayColorChangeAnim();

            return true;
        }

        return false;
    }
}
