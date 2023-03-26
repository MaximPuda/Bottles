public class ComboSlime : Combo
{
    public override bool TryDoCombo(ItemController itemSender, ItemController itemTarget)
    {
        // ����� �� ��������������� �� � ���
        if (itemTarget.Type == TypeNames.Slime)
            return false;

        // ����� + ������ ������� = ����� ������������, � ������� ������������
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
